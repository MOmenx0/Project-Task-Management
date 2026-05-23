# Project & Task Management API

ASP.NET Core Web API built with **Clean Architecture**, **.NET 9**, **EF Core**, **SQL Server**, and **JWT authentication**.

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-EF%20Core-CC2927?logo=microsoft-sql-server&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![OpenAPI](https://img.shields.io/badge/OpenAPI-Swagger-6BA539?logo=swagger&logoColor=white)](https://swagger.io/)

## Features

Production-oriented capabilities designed for clarity, security, and long-term maintainability:

- **Clean Architecture** — Domain-centric design with strict dependency rules: inner layers never depend on outer layers, keeping business logic isolated and testable.
- **Layered solution** — Four dedicated projects (Domain, Application, Infrastructure, API) with clear boundaries and **Separation of Concerns** across each tier.
- **CQRS with MediatR** — Commands and queries organized by use case (`AuthCases`, `ProjectsCases`, `TasksCases`), keeping read and write workflows explicit and easy to extend.
- **Repository & Unit of Work** — Generic `IBaseRepository<T>` with EF Core implementations and a scoped `IunitOfWork` for consistent, transactional data access.
- **Specification pattern** — Reusable `ISpecifications<T>` and `SpecificationsEvaluator<T>` for composable, expression-based queries without leaking persistence details into handlers.
- **Dependency Injection** — Layer-specific extension methods (`AddApplicationServices`, `AddInfrastrctureServices`, `AddWebServices`) register services by concern and lifetime.
- **JWT authentication & authorization** — Register/login flows with bearer tokens; protected project and task endpoints enforce authenticated access.
- **User-scoped resources** — Projects and tasks are tied to the authenticated user, supporting multi-tenant-style isolation at the data layer.
- **FluentValidation** — Request validators per command with a MediatR **`ValidationBehaviour`** pipeline that fails fast before handlers execute.
- **Unified API responses** — `DataResponse<T>` envelope (`IsSuccess`, `StatusCode`, `ResponseMessage`, `ResponseData`) for consistent success and error payloads across endpoints.
- **Centralized error handling** — `IExceptionHandler` maps validation, not-found, and unauthorized exceptions to appropriate HTTP status codes and structured JSON responses.
- **AutoMapper** — DTO mapping profiles in the Application layer to keep entities decoupled from API contracts.
- **Secure password hashing** — ASP.NET Core `PasswordHasher<User>` behind `IPasswordHasher` for industry-standard credential storage (no plaintext passwords).
- **RESTful minimal APIs** — Resource-oriented routes (`/api/projects`, `/api/tasks`, `/api/auth`) with correct HTTP verbs and status semantics.
- **OpenAPI / Swagger** — Interactive API documentation with JWT bearer security scheme for in-browser testing.
- **EF Core migrations** — Schema versioning via EF Core tools; database migrations applied automatically on application startup.
- **Middleware pipeline** — HTTPS redirection, CORS, global exception handling, authentication, and authorization applied in a predictable order.
- **Defensive coding** — **Ardalis GuardClauses** in handlers and API extensions for explicit null and argument checks.
- **Nullable reference types** — Enabled across projects to catch null-related issues at compile time.

### Architecture at a glance

```
ProjectTaskManagement/
├── ProjectTaskManagement.Domain/          # Entities, enums — zero external dependencies
├── ProjectTaskManagement.Application/     # CQRS, validators, DTOs, interfaces, specifications
├── ProjectTaskManagement.Infrastructure/  # EF Core DbContext, repositories, password hashing
└── ProjectTaskManagement.Api/             # Minimal API endpoints, JWT, Swagger, exception handling
```

Dependency flow follows Clean Architecture: **Api → Application → Domain** and **Infrastructure → Application**, with Infrastructure supplying concrete implementations at composition root.

## Technologies Used

| Category | Technology | Role in this solution |
|----------|------------|------------------------|
| **Runtime & framework** | [.NET 9](https://dotnet.microsoft.com/download/dotnet/9.0) | Target framework for all projects |
| **Web API** | [ASP.NET Core 9](https://learn.microsoft.com/aspnet/core/) | Minimal APIs, middleware, hosting |
| **Architecture** | Clean Architecture + layered design | Maintainable, scalable solution structure |
| **Application patterns** | CQRS, Repository, Unit of Work, Specification | MediatR handlers, `IBaseRepository<T>`, `IunitOfWork`, `ISpecifications<T>` |
| **Mediation** | [MediatR 12](https://github.com/jbogard/MediatR) | Command/query dispatch and pipeline behaviors |
| **Validation** | [FluentValidation 11](https://docs.fluentvalidation.net/) | Declarative request rules + `ValidationBehaviour` |
| **Mapping** | [AutoMapper 13](https://docs.automapper.org/) | Entity ↔ DTO projections |
| **ORM** | [Entity Framework Core 9](https://learn.microsoft.com/ef/core/) | Data access, migrations, SQL Server provider |
| **Database** | [SQL Server](https://www.microsoft.com/sql-server) | Relational persistence (LocalDB supported for development) |
| **Security** | JWT Bearer + `Microsoft.AspNetCore.Authentication.JwtBearer` | Stateless API authentication |
| **Password storage** | `Microsoft.Extensions.Identity.Core` (`PasswordHasher`) | Secure one-way password hashing |
| **API documentation** | OpenAPI, [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore), [NSwag](https://github.com/RicoSuter/NSwag) | Swagger UI and OpenAPI document generation |
| **Guards** | [Ardalis.GuardClauses](https://github.com/ardalis/GuardClauses) | Precondition checks in handlers and extensions |
| **Error handling** | `IExceptionHandler`, `DataResponse<T>`, custom exceptions | Predictable HTTP errors and response shape |
| **Cross-cutting** | CORS, HTTPS redirection, scoped `ICurrentUserService` | Request pipeline and current-user context from JWT claims |

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
