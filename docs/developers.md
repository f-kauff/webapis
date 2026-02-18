
# Developer notes

This document describes developer commands and notes used when working on this repository, how to install tools (hooks) and common dotnet commands for creating projects, test, add package of project references.  
Use it as a quick reference for local setup. 

## Pre-commit

install locally the pre-commit hook for formatting files
Run it once per machine
```
dotnet tool install --local CSharpier
dotnet tool install --local Husky
dotnet husky install
# add new hooks
# dotnet husky add pre-commit -c "dotnet husky run --group pre-commit"
```
``

## .NET cli
Official docs: https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet

### Create a new project
```
dotnet new webapi -o src/DeviceRegistrationAPI
dotnet new webapi -o src/StatisticsAPI
dotnet new classlib -o src/Shared
```

### Add a unit test project
```
dotnet new xunit -n DeviceRegistrationAPI.UnitTests -o tests/DeviceRegistrationAPI.UnitTests
dotnet add tests/DeviceRegistrationAPI.UnitTests reference src/DeviceRegistrationAPI/DeviceRegistrationAPI.csproj

dotnet new xunit -n StatisticsAPI.UnitTests -o tests/StatisticsAPI.UnitTests
dotnet add tests/StatisticsAPI.UnitTests reference src/StatisticsAPI/StatisticsAPI.csproj
```

### Add nuget package
```

dotnet nuget add source https://api.nuget.org/v3/index.json -n nuget.org
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

dotnet add package Moq
dotnet add package Moq.EntityFrameworkCore
dotnet add package RichardSzalay.MockHttp
```
### Add project reference
```
dotnet add reference ../Shared/Shared.csproj
# from UnitTests projects
dotnet add reference ../../src/DeviceRegistrationAPI/DeviceRegistrationAPI.csproj 
dotnet add reference ../../src/Shared/Shared.csproj 
```

### Build and run project locally
Go to the src project
```
dotnet restore
dotnet build
dotnet run
http://localhost:5092/openapi/v1.json (checked the port used in the terminal output or in launchSettings.json)
'''
