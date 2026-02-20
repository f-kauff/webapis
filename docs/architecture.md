# Web APIs demo

## DeviceRegistrationAPI
**Technology**: .NET 10 webapi  
**Role**: Register new user/devicetype  
**Data access**: read/write to the database

It exposes an endpoint to register userkeys and deviceTypes, it writes directly to the database. In kubernetes this service is only accessible by the StatisticsAPIs.

## StatisticsAPI
**Technology**: .NET 10 webapi  
** Role **: Acts as the public gateway, retrieves statistics and forward registration requests to DeviceRegistrationAPI.
**Data access**: read access to the database, write via DeviceRegistrationAPI

It exposes a public endpoint to register user keys and device types (internally calling the DeviceRegistrationAPI), and an endpoint to retrieve statistics for device types. It has read-only access to the database for statistics retrieval.

## Shared
**Technology**: .NET 10 class library  
**Role**: Define shared data models

A class library containing common DTOs, request/response models.

## Database
**Technology**: postgresql  
**Role**: Persist the userKey/deviceType information

A single instance database is used to store the user key and device type data.
It is accessible by both APIs using the same credentials for the demo. However, the Statistics API could use a separate login with read-only permissions.



