# SubscriptionApi - Recruiter-ready
This is a polished ASP.NET Core Web API implementing the assignment:
- Login -> returns server-side token valid for X hours (configurable)
- Subscribe -> subscribe a user (returns subscriptionId)
- Unsubscribe -> unsubscribe a user
- Status -> check subscription state and timestamps

## What's included
- Full C# project targeting .NET 7
- EF Core (Pomelo MySQL provider)
- Seeded demo Service entry (ServiceId: demo-service, Password: password123)
- Sample EF Core migration files (Migrations folder) and ModelSnapshot
- Swagger, Dockerfile, Postman collection, SQL schema, README

## How to run (development)
1. Ensure .NET 7 SDK and MySQL are available.
2. Update `appsettings.json` -> `ConnectionStrings:StrivoLabDB` with your MySQL credentials.
3. From project folder:
   ```bash
   dotnet restore
   dotnet tool install --global dotnet-ef --version 7.*
   dotnet ef database update
   dotnet run
   ```
   The project will also attempt to apply migrations automatically at startup (see Program.cs).
4. Open Swagger at `https://localhost:7222/swagger` (or http://localhost:5087 in non-https dev)

## Example login (seeded service)
Request:
```
POST /api/auth/login
{ "service_id": "demo-service", "password": "password123" }
```
Response: `{ "token_id": "...", "expiresAt": "2025-11-25T..." }`

