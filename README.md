C# Backend Blueprint:
- .NET 8
- CQRS MediatR
- EF Framework

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
