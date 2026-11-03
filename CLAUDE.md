# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project state

DatingApp is an early-stage dating app (ASP.NET Core + Angular, Udemy course). The repo has two parts:

- `API/` — ASP.NET Core Web API (the only project in `DatingApp.sln`), backed by SQLite via EF Core.
- `client/` — Angular 22 SPA (standalone, outside the .sln).

There are no test projects or spec files yet. The template `WeatherForecast` sample is still present in the API. The git repository root is this solution folder (remote: `origin` → GitHub `denys-vladymyrov/DatingApp`, branch `main`).

## Conventions

- All repository content is in English: code, comments, docs, and commit messages.

## Commands

API — run from the repo root (or target `API/API.csproj` directly):

```bash
dotnet build                      # build the solution
dotnet run --project API          # runs the "https" launch profile at https://localhost:5001
dotnet watch --project API        # run with hot reload
dotnet ef database update --project API                                  # apply migrations
dotnet ef migrations add <Name> --project API -o Data/Migrations         # add a migration
```

`dotnet ef` requires the global tool: `dotnet tool install --global dotnet-ef`.

Client — run from `client/`:

```bash
npm install
npm start                         # ng serve at http://localhost:4200
npm run build                     # production build
npm test                          # ng test (Vitest, no specs yet)
```

No linters are configured; the client has Prettier (`client/.prettierrc`).

## Architecture

### API

- **Target framework:** `net10.0`, with nullable reference types and implicit usings enabled.
- **Port:** `https://localhost:5001` (from `API/Properties/launchSettings.json`). Do not use ports in the 7261–7360 range on this machine: Windows (winnat/Hyper-V) reserves it and binding fails with an access error.
- **Hosting:** `API/Program.cs` uses minimal hosting with attribute-routed controllers (`AddControllers` / `MapControllers`) and registers `AppDbContext`. CORS allows any method/header from `http://localhost:4200` and `https://localhost:4200`. There is no HTTPS redirection, auth, or other middleware yet.
- **Data access:** EF Core 10 with SQLite (`Microsoft.EntityFrameworkCore.Sqlite`). `API/Data/AppDbContext.cs` uses a primary constructor and currently exposes only `DbSet<AppUser> Users`. Migrations live in `API/Data/Migrations/` (only `Initial` so far). The connection string `DefaultConnection` (`Data Source=dating.db`) is in `appsettings.Development.json` (there is none for other environments yet); database files are gitignored.
- **Entities** live in `API/Entities/` (`AppUser`, `Member`, `Photo`, `MemberLike`, `Message`, `Group`, `Connection`). Only `AppUser` is mapped in the DbContext so far; the rest are not yet registered or migrated. `Member` shares its key with `AppUser` (`[ForeignKey(nameof(Id))]`), and navigation properties are marked `[JsonIgnore]`.
- **Controllers** live in `API/Controllers/`, derive from `ControllerBase`, are marked `[ApiController]`, and inject dependencies via primary constructors. `MembersController` (`[Route("api/[controller]")]`) exposes `GET api/members` and `GET api/members/{id}`, returning `AppUser` entities directly (no DTOs yet). The template `WeatherForecastController` uses `[Route("[controller]")]`.
- **Config:** the launch profile sets `ASPNETCORE_ENVIRONMENT=Development`.

### Client

- **Stack:** Angular 22 (`@angular/build` application builder), TypeScript 6, standalone components with signals; no NgModules. Component selector prefix is `app`.
- **Bootstrap:** `src/main.ts` → `bootstrapApplication(App, appConfig)`. `src/app/app.config.ts` provides the router (`app.routes.ts`, no routes yet) and `provideHttpClient()`.
- **Root component:** `src/app/app.ts` injects `HttpClient` via `inject()` and loads members from `https://localhost:5001/api/members` in `ngOnInit` into a `members` signal (typed `any`, no models or services yet). The API URL is hard-coded.
- **Styling:** Tailwind CSS 4 via PostCSS (`.postcssrc.json`) plus the daisyUI 5 plugin, both enabled in `src/styles.css`. daisyUI component classes (e.g. `card`, `list`, `list-row`) bring their own styles from `node_modules/daisyui`, including pseudo-element decorations like the `list-row::after` separator. Static assets live in `public/`.
- **IDE:** the user works in JetBrains Rider; `client/` is attached to the solution as an attached folder so it is indexed.
