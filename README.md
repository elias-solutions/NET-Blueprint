C# Backend Blueprint:
- .NET 10
- CQRS with own lightweight abstractions (`ICommandHandler` / `IQueryHandler`, registered via `AddCqrs()`) — no MediatR
- Entity Framework Core 10 with PostgreSQL (Npgsql)
- two API hosts sharing the same Application/Infrastructure: Controller API and Minimal API (API versioning, Swagger)
- xUnit integration tests with Testcontainers

Projects
- `NET.Backend.Blueprint.Domain` — entities
- `NET.Backend.Blueprint.Application` — CQRS handlers, request/response models, repository abstraction
- `NET.Backend.Blueprint.Infrastructure` — EF Core DbContext, repository, migrations
- `NET.Backend.Blueprint.Web` — shared web setup (authentication, authorization, error handling, OpenAPI)
- `NET.Backend.Blueprint.Api` — controller-based API
- `NET.Backend.Blueprint.Minimal.Api` — minimal API endpoints (incl. rate limiting)
- `NET.Backend.Blueprint.Extensions` — shared helpers
- `NET.Backend.Blueprint.Integration.xUnit.Tests` / `NET.Backend.Blueprint.Minimal.Integration.xUnit.Tests` — integration tests per API

Preparation
- start PostgreSQL from the Blueprint root: `docker compose up -d` (database `blueprint_net`)
- migrations are applied on API startup; add new ones with
  `dotnet ef migrations add <Name> --project NET.Backend.Blueprint.Infrastructure --startup-project NET.Backend.Blueprint.Infrastructure --output-dir DataAccess/Migrations`

Test
- `dotnet test` — integration tests start their own PostgreSQL via Testcontainers (Docker required)

Docker
- images are built from the published output (`publish/<Project>`), see `NET.Backend.Blueprint.Api/Dockerfile` and `NET.Backend.Blueprint.Minimal.Api/Dockerfile`
- from the Blueprint root: `./build-images.sh` (build missing images, migrate, compose up) or `./build-images.sh -f` (force rebuild)
- `docker compose up -d` starts PostgreSQL + all APIs (Api: http://localhost:5102/swagger, Minimal Api: http://localhost:5206/swagger)
