# StudentHub API Instructions

This repository includes its own REST API. It is part of the StudentHub ASP.NET Core application and uses the same `crude.db` SQLite database as the Razor web pages.

The API is not dependent on KierCRUD.

## Requirements

- .NET 8 SDK
- Windows PowerShell
- Access to this repository

## Start the API

From the repository root:

```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
.\scripts\start-api.ps1
```

The default API address is:

```text
http://localhost:5100
```

Health check:

```text
http://localhost:5100/api/health
```

The health response looks like this:

```json
{
  "status": "ok",
  "app": "StudentHub API"
}
```

## Allow Other Computers

Start the API on all network interfaces:

```powershell
.\scripts\start-api.ps1 -Url "http://0.0.0.0:5100"
```

Find the API computer's local IP address:

```powershell
ipconfig
```

Other devices on the same network can then use:

```text
http://YOUR-COMPUTER-IP:5100
```

For example:

```text
http://192.168.1.10:5100/api/students
```

Windows Firewall may need an inbound TCP rule for port `5100`.

## CORS

For a browser-based client, add its URL to `CrudeAspNet/appsettings.json`:

```json
"AllowedCorsOrigins": [
  "http://localhost:3000",
  "http://192.168.1.20:3000"
]
```

Restart the API after changing this setting. Do not use `*` for production clients.

## Endpoints

### Health

```text
GET /api/health
```

### Students

```text
GET    /api/students
GET    /api/students/{id}
POST   /api/students
PUT    /api/students/{id}
DELETE /api/students/{id}
```

Create a student:

```json
{
  "studentName": "Maria Santos",
  "studentId": "S001",
  "email": "maria@example.com"
}
```

PowerShell example:

```powershell
$body = @{
    studentName = "Maria Santos"
    studentId = "S001"
    email = "maria@example.com"
} | ConvertTo-Json

Invoke-RestMethod `
    -Uri "http://localhost:5100/api/students" `
    -Method Post `
    -ContentType "application/json" `
    -Body $body
```

### Courses

```text
GET  /api/courses
POST /api/courses
```

Create a course:

```json
{
  "courseCode": "BSCS",
  "courseName": "Bachelor of Science in Computer Science"
}
```

### School Years

```text
GET  /api/schoolyears
POST /api/schoolyears
```

Create a school year:

```json
{
  "schoolYearCode": "2026-2027",
  "semester": "First semester",
  "status": "Active"
}
```

### Enrollments

```text
GET  /api/enrollments
POST /api/enrollments
```

Create an enrollment using IDs returned by the students, courses, and school-years endpoints:

```json
{
  "studentId": 1,
  "courseId": 1,
  "schoolYearId": 1
}
```

The API validates that all related records exist and rejects duplicate enrollments.

## JavaScript Example

```javascript
const API_BASE_URL = "http://localhost:5100";

const response = await fetch(`${API_BASE_URL}/api/students`);
const students = await response.json();
console.log(students);
```

## Response Status Codes

- `200` successful read or update
- `201` successful create
- `204` successful delete
- `400` invalid input or missing related records
- `404` record not found
- `409` duplicate student, course, school year, or enrollment

## Data and Security Notes

- The API and Razor pages use the same `CrudeAspNet/crude.db` database.
- Do not expose the API directly to the public internet without authentication and HTTPS.
- Keep the API computer and client device on a trusted network when using the HTTP development setup.
- The API must be running before another application can call it.
