using DeviceRegistrationAPI.Data;
using DeviceRegistrationAPI.Services;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddScoped<IDeviceRegistrationService, DeviceRegistrationService>();

builder.Services.AddDbContext<DeviceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddOpenApi();

// Standardize unexpected errors
//builder.Services.AddProblemDetails();

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
        "/Device/register",
        async (DeviceRegistrationInput input, IDeviceRegistrationService service) =>
        {
            var result = await service.RegisterDeviceAsync(input.UserKey, input.DeviceType);
            if (result.IsSuccessful)
            {
                return Results.Ok(new { statusCode = 200 });
            }

            return Results.BadRequest(new { statusCode = 400, message = result.ErrorMessage });
        }
    )
    .WithName("RegisterDevice");

// Healthcheck endpoint
app.MapGet("/health", () => Results.Ok("OK")).WithName("HealthCheck");

// Apply database migrations at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DeviceDbContext>();
    db.Database.EnsureCreated();
}

app.Run();
