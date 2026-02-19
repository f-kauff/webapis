using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using StatisticsAPI.Data;
using StatisticsAPI.Models;
using StatisticsAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddScoped<IStatisticsService, StatisticsService>();

builder.Services.AddDbContext<StatisticsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Global error handling
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var result = new
        {
            statusCode = 400, // should be 500 for unexpected errors, but using 400  for demo
            message = "An unexpected error occurred. Please try again later.",
        };

        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(result);
    });
});

app.MapPost(
        "/Log/auth",
        async (AuthLogInput input, IStatisticsService service) =>
        {
            var result = await service.LogAuthAsync(input);
            if (result.IsSuccessful)
            {
                return Results.Ok(new { statusCode = 200, message = "success" });
            }

            return Results.BadRequest(new { statusCode = 400, message = result.ErrorMessage });
        }
    )
    .WithName("LogAuth");

app.MapGet(
        "/Log/auth/statistics",
        async (string? deviceType, IStatisticsService service) =>
        {
            var stats = await service.GetStatisticsAsync(deviceType);
            return Results.Ok(stats);
        }
    )
    .WithName("GetStatistics");

// Healthcheck endpoint
app.MapGet("/health", () => Results.Ok("OK")).WithName("HealthCheck");

// database migrations is managed by DeviceRegistrationAPI

app.Run();
