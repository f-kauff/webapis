# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY src/DeviceRegistrationAPI/*.csproj ./DeviceRegistrationAPI/
RUN dotnet restore ./DeviceRegistrationAPI/DeviceRegistrationAPI.csproj

COPY src/DeviceRegistrationAPI/. ./DeviceRegistrationAPI/
COPY src/Shared/. ./Shared/
WORKDIR /src/DeviceRegistrationAPI
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble-chiseled AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080

# Start webAPI
ENTRYPOINT ["dotnet", "DeviceRegistrationAPI.dll"]