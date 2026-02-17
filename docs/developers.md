

## Pre-commit

install locally the pre-commit hook for formatting files

```
dotnet tool install --local CSharpier
dotnet tool install --local Husky
dotnet husky install

# add new hooks
# dotnet husky add pre-commit -c "dotnet husky run --group pre-commit"
```


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
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```
## Add project reference
```
dotnet add reference ../Shared/Shared.csproj

```

## Build and run project locally
dotnet restore
dotnet build
dotnet run
http://localhost:5092/openapi/v1.json

