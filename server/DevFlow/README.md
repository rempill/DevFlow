# DevFlow Backend

Clean Architecture scaffold for the DevFlow backend.

## Projects

- DevFlow.Domain: Entities, enums, value objects, and core interfaces.
- DevFlow.Application: Use cases and DTOs (empty scaffold).
- DevFlow.Infrastructure: External services (empty scaffold).
- DevFlow.Persistence: Database access (empty scaffold).
- DevFlow.API: HTTP host (empty scaffold).

## Build

```bash
dotnet build DevFlow.sln
```

## Run (API)

```bash
dotnet run --project DevFlow.API\DevFlow.API.csproj
```

## GitHub Adapter

The infrastructure layer provides an Octokit-backed adapter. Register it in the API startup:

```csharp
builder.Services.AddApplicationServices();
builder.Services.AddGitHubAdapter("DevFlow", Environment.GetEnvironmentVariable("GITHUB_TOKEN"));
```

Set `GITHUB_TOKEN` in your environment for authenticated calls.

