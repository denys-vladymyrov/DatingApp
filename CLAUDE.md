# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project state

DatingApp is in its earliest scaffolding stage: the solution (`DatingApp.sln`) holds a single ASP.NET Core Web API project, `API/`, which still contains the template `WeatherForecast` sample. There is no Angular client, database, or test project yet. The git repository root is this solution folder (remote: `origin` → GitHub `denys-vladymyrov/DatingApp`, branch `main`).

## Commands

Run from the repo root (or target `API/API.csproj` directly):

```bash
dotnet build                      # build the solution
dotnet run --project API          # runs the "https" launch profile at https://localhost:7276
dotnet watch --project API        # run with hot reload
```

There are no tests or linters configured yet.

## Architecture

- **Target framework:** `net10.0`, with nullable reference types and implicit usings enabled.
- **Hosting:** `API/Program.cs` uses minimal hosting with attribute-routed controllers only (`AddControllers` / `MapControllers`). There is no HTTPS redirection, auth, CORS, or other middleware yet.
- **Controllers** live in `API/Controllers/`, use the `API.Controllers` namespace, derive from `ControllerBase`, and are marked `[ApiController]` with `[Route("[controller]")]`.
- **OpenAPI:** `Microsoft.AspNetCore.OpenApi` is referenced in `API.csproj`, but `AddOpenApi`/`MapOpenApi` are not wired up in `Program.cs`.
- **Config:** `appsettings.json` and `appsettings.Development.json` contain logging settings only. The launch profile sets `ASPNETCORE_ENVIRONMENT=Development`.

## Notes

- `README.md` is saved as UTF-16. Re-encode it to UTF-8 before editing it with text tools.
