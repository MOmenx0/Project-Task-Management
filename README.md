# Project & Task Management API

ASP.NET Core Web API built with **Clean Architecture**, **.NET 9**, **EF Core**, **SQL Server**, and **JWT authentication**.

## Solution structure

| Layer | Project | Responsibility |
|-------|---------|----------------|
| Domain | `ProjectTaskManagement.Domain` | Entities, enums |
| Application | `ProjectTaskManagement.Application` | CQRS (MediatR), validators, DTOs, interfaces |
| Infrastructure | `ProjectTaskManagement.Infrastructure` | EF Core, repositories, password hashing |
| Web API | `ProjectTaskManagement.Api` | Endpoints, JWT, Swagger, exception handling |

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server or LocalDB

## Setup

1. Clone the repository and open `ProjectTaskManagement.sln`.
2. Update the connection string in `ProjectTaskManagement.Api/appsettings.json` if needed:

```json
"ConnectionStrings": {
  "ProjectTaskManagementConnection": "Server=(localdb)\\mssqllocaldb;Database=ProjectTaskManagement;..."
}
```

3. Run the API (migrations apply automatically on startup):

```bash
cd "Morn Agi Back-End"
dotnet run --project ProjectTaskManagement.Api
```

4. Open Swagger UI at the root URL (e.g. `https://localhost:7074/`).

## Authentication

1. `POST /api/auth/register` — create account (no token returned)  
2. `POST /api/auth/login` — get JWT token  
3. In Swagger, click **Authorize** and enter: `Bearer {your-token}`

## API endpoints

### Auth (anonymous)
- `POST /api/auth/register`
- `POST /api/auth/login`

### Projects (authorized)
- `GET /api/projects`
- `GET /api/projects/{id}`
- `POST /api/projects`
- `PUT /api/projects/{id}`
- `DELETE /api/projects/{id}`

### Tasks (authorized)
- `GET /api/tasks/project/{projectId}`
- `POST /api/tasks`
- `PATCH /api/tasks/{taskId}/status`
- `DELETE /api/tasks/{taskId}`

## Migrations

```bash
dotnet ef migrations add MigrationName --project ProjectTaskManagement.Infrastructure --startup-project ProjectTaskManagement.Api
dotnet ef database update --project ProjectTaskManagement.Infrastructure --startup-project ProjectTaskManagement.Api
```
