using Microsoft.EntityFrameworkCore;
using CrudeAspNet.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApiCors", policy =>
    {
        var origins = builder.Configuration.GetSection("AllowedCorsOrigins").Get<string[]>();
        if (origins?.Length > 0)
        {
            policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
        }
    });
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=crude.db;Default Timeout=5",
        sqlite => sqlite.CommandTimeout(5)));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    db.Database.ExecuteSqlRaw("PRAGMA journal_mode = WAL;");
    db.Database.ExecuteSqlRaw("PRAGMA busy_timeout = 5000;");
    db.Database.ExecuteSqlRaw("""
        CREATE TABLE IF NOT EXISTS Courses (
            Id INTEGER NOT NULL CONSTRAINT PK_Courses PRIMARY KEY AUTOINCREMENT,
            CourseCode TEXT NOT NULL,
            CourseName TEXT NOT NULL
        );
        CREATE UNIQUE INDEX IF NOT EXISTS IX_Courses_CourseCode ON Courses (CourseCode);
        CREATE TABLE IF NOT EXISTS SchoolYears (
            Id INTEGER NOT NULL CONSTRAINT PK_SchoolYears PRIMARY KEY AUTOINCREMENT,
            SchoolYearCode TEXT NOT NULL,
            Semester TEXT NOT NULL,
            Status TEXT NOT NULL
        );
        CREATE UNIQUE INDEX IF NOT EXISTS IX_SchoolYears_SchoolYearCode ON SchoolYears (SchoolYearCode);
        CREATE TABLE IF NOT EXISTS Enrollments (
            Id INTEGER NOT NULL CONSTRAINT PK_Enrollments PRIMARY KEY AUTOINCREMENT,
            StudentId INTEGER NOT NULL,
            CourseId INTEGER NOT NULL,
            SchoolYearId INTEGER NOT NULL,
            CONSTRAINT FK_Enrollments_Students_StudentId FOREIGN KEY (StudentId) REFERENCES Students (Id) ON DELETE CASCADE,
            CONSTRAINT FK_Enrollments_Courses_CourseId FOREIGN KEY (CourseId) REFERENCES Courses (Id) ON DELETE CASCADE,
            CONSTRAINT FK_Enrollments_SchoolYears_SchoolYearId FOREIGN KEY (SchoolYearId) REFERENCES SchoolYears (Id) ON DELETE CASCADE
        );
        CREATE UNIQUE INDEX IF NOT EXISTS IX_Enrollments_StudentId_CourseId_SchoolYearId ON Enrollments (StudentId, CourseId, SchoolYearId);
        """);
}

app.UseStaticFiles();
app.UseCors("ApiCors");
app.MapControllers();
app.MapControllerRoute(name: "default", pattern: "{controller=Students}/{action=Index}/{id?}");
app.Run();
