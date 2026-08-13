# StudentHub CRUD

A student-records CRUD application built with ASP.NET Core 8 MVC, Entity Framework Core, and SQLite.

## Features

- Create student records
- View all students
- Edit student information
- Delete student records
- Server-side validation for name, student ID, and email

## Requirements

- .NET 8 SDK

## Run the app

From the repository root, run:

```powershell
.\scripts\start.ps1
```

The launcher restores packages, starts the application, and opens the student directory in your default browser.

To use a different port:

```powershell
.\scripts\start.ps1 -Url "http://localhost:5050"
```

You can also run `scripts\start.cmd` from Command Prompt or by double-clicking it in File Explorer.

## Project structure

```text
CrudeAspNet/          ASP.NET Core MVC application
  Controllers/        CRUD controller
  Models/             Student data model
  Views/              Student directory and forms
  Data/               Entity Framework database context
scripts/              Windows launch scripts
```

The SQLite database is created automatically when the app starts.
