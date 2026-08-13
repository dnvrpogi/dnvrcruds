# CrudeAspNet (ASP.NET Core 8)

Simple CRUD app converted from Flask to ASP.NET Core MVC using EF Core and SQLite.

Requirements
- .NET 8 SDK

Run

```powershell
.\scripts\start.ps1
```

This restores dependencies, starts the backend and Razor frontend together, waits for the app to be ready, then opens the student list in your default browser at `http://localhost:5000/Students`.

Use a different address if port 5000 is already in use:

```powershell
.\scripts\start.ps1 -Url "http://localhost:5050"
```

Notes
- Database file `crude.db` is created automatically (EnsureCreated).

Tech used
- **Backend:** C# with ASP.NET Core 8 (MVC)
- **ORM/Database:** Entity Framework Core with SQLite
- **Views:** Razor (Razor Pages / MVC views) with Bootstrap 5 for styling
- **Development:** .NET 8 SDK, `dotnet` CLI
- **Source control:** Git (repository at https://github.com/dnvrpogi/dnvrcruds)

Optional commands
- Create EF Core migrations (if you prefer migrations over EnsureCreated):

```powershell
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Enjoy!
