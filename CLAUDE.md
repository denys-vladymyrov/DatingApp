# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project state

DatingApp is an early-stage dating app (ASP.NET Core + Angular 20, Udemy course). The solution (`DatingApp.sln`) holds a single ASP.NET Core Web API project, `API/`, backed by SQLite via EF Core. There is no Angular client or test project yet. The template `WeatherForecast` sample is still present. The git repository root is this solution folder (remote: `origin` → GitHub `denys-vladymyrov/DatingApp`, branch `main`).

## Conventions

- All repository content is in English: code, comments, docs, and commit messages.

## Commands

Run from the repo root (or target `API/API.csproj` directly):

```bash
dotnet build                      # build the solution
dotnet run --project API          # runs the "https" launch profile at https://localhost:7276
dotnet watch --project API        # run with hot reload
dotnet ef database update --project API                                  # apply migrations
dotnet ef migrations add <Name> --project API -o Data/Migrations         # add a migration
```

`dotnet ef` requires the global tool: `dotnet tool install --global dotnet-ef`. There are no tests or linters configured yet.

## Architecture

- **Target framework:** `net10.0`, with nullable reference types and implicit usings enabled.
- **Hosting:** `API/Program.cs` uses minimal hosting with attribute-routed controllers (`AddControllers` / `MapControllers`) and registers `AppDbContext`. There is no HTTPS redirection, auth, CORS, or other middleware yet.
- **Data access:** EF Core 10 with SQLite (`Microsoft.EntityFrameworkCore.Sqlite`). `API/Data/AppDbContext.cs` uses a primary constructor and currently exposes only `DbSet<AppUser> Users`. Migrations live in `API/Data/Migrations/`. The connection string `DefaultConnection` (`Data Source=dating.db`) is in `appsettings.Development.json` (there is none for other environments yet); database files are gitignored.
- **Entities** live in `API/Entities/` (`AppUser`, `Member`, `Photo`, `MemberLike`, `Message`, `Group`, `Connection`). Only `AppUser` is mapped in the DbContext so far; the rest are not yet registered or migrated. `Member` shares its key with `AppUser` (`[ForeignKey(nameof(Id))]`), and navigation properties are marked `[JsonIgnore]`.
- **Controllers** live in `API/Controllers/`, derive from `ControllerBase`, are marked `[ApiController]`, and inject dependencies via primary constructors. `MembersController` uses `[Route("api/[controller]")]`; the template `WeatherForecastController` uses `[Route("[controller]")]`.
- **Config:** the launch profile sets `ASPNETCORE_ENVIRONMENT=Development`.
