#Web APIs demo

## DeviceRegistrationAPI
**Technology**: .NET 10 webapi  
**Role**: Register new user/devicetype  
**Data access**: read/write to the database

## StatisticsAPI
**Technology**: .NET 10 webapi  
**Data access**: read access to the database, write via DeviceRegistrationAPI

## Shared
**Technology**: .NET 10 class library  
**Role**: Define shared data models

## Database
**Technology**: postgresql  
**Role**: Persist the userKey/deviceType information
