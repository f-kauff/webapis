
## .NET cli
https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet

### Create a new project
```
dotnet new webapi -o src/DeviceRegistrationAPI
dotnet new webapi -o src/StatisticsAPI
dotnet new classlib -o src/Shared
```

### Add package
```
dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org

```

## Build and run app locally
dotnet restore
dotnet build
dotnet run
http://localhost:5092/openapi/v1.json
