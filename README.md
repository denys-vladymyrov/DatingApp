# DatingApp

A dating application built with **ASP.NET Core (.NET 10)** and **Angular 20**, developed as part of a Udemy course.

> **Status:** early development. Only the backend (Web API) with a SQLite database exists so far. The Angular client has not been added yet.

## Features

Implemented:

- ASP.NET Core Web API with attribute-routed controllers
- Entity Framework Core + SQLite with an initial migration
- Endpoints to list users and get a user by `id`

Planned (the data model is already in `API/Entities`):

- Member profiles with photos
- Likes between members
- Private messaging
- Real-time features (SignalR groups and connections)
- Angular client

## Tech stack

| Layer | Stack |
|-------|-------|
| Backend | ASP.NET Core Web API, .NET 10 |
| ORM | Entity Framework Core 10 |
| Database | SQLite |
| Frontend | Angular 20 *(planned)* |

## Project structure

```
DatingApp/
├── DatingApp.sln
└── API/
    ├── Controllers/        # API controllers (MembersController, etc.)
    ├── Data/
    │   ├── AppDbContext.cs # EF Core DbContext
    │   └── Migrations/     # database migrations
    ├── Entities/           # domain models: AppUser, Member, Photo,
    │                       # MemberLike, Message, Group, Connection
    ├── Properties/
    │   └── launchSettings.json
    ├── appsettings.json    # base configuration
    └── Program.cs          # entry point and service configuration
```

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- EF Core CLI tool:

  ```bash
  dotnet tool install --global dotnet-ef
  ```

### Run

```bash
git clone https://github.com/denys-vladymyrov/DatingApp.git
cd DatingApp

# create/update the SQLite database (API/dating.db)
dotnet ef database update --project API

# run the API
dotnet run --project API
```

The API is available at **https://localhost:5001**.

To run with hot reload:

```bash
dotnet watch --project API
```

## API

| Method | Route | Description |
|--------|-------|-------------|
| `GET` | `/api/members` | List all users |
| `GET` | `/api/members/{id}` | Get a user by `id` (404 if not found) |

Example:

```bash
curl -k https://localhost:5001/api/members
```

## Database

The connection string is set in `API/appsettings.Development.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=dating.db"
}
```

To add a migration after changing the models:

```bash
dotnet ef migrations add <MigrationName> --project API -o Data/Migrations
dotnet ef database update --project API
```

Database files (`*.db`, `*.db-shm`, `*.db-wal`) are excluded from git.

## Author

Denys Vladymyrov — [GitHub](https://github.com/denys-vladymyrov)
