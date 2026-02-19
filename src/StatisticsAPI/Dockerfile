# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/StatisticsAPI/*.csproj ./StatisticsAPI/
RUN dotnet restore ./StatisticsAPI/StatisticsAPI.csproj

COPY src/StatisticsAPI/. ./StatisticsAPI/
COPY src/Shared/. ./Shared/
WORKDIR /src/StatisticsAPI
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080

# Start webAPI
ENTRYPOINT ["dotnet", "StatisticsAPI.dll"]