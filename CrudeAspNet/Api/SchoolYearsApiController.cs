using CrudeAspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudeAspNet.Api;

[ApiController]
[Route("api/schoolyears")]
public class SchoolYearsApiController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolYearResponse>>> GetAll() => Ok(await db.SchoolYears.AsNoTracking().OrderByDescending(year => year.SchoolYearCode).Select(year => new SchoolYearResponse(year.Id, year.SchoolYearCode, year.Semester, year.Status)).ToListAsync());

    [HttpPost]
    public async Task<ActionResult<SchoolYearResponse>> Create(SchoolYearRequest request)
    {
        if (await db.SchoolYears.AnyAsync(year => year.SchoolYearCode == request.SchoolYearCode)) return Conflict(new { message = "That school-year code already exists." });
        var year = new Models.SchoolYear { SchoolYearCode = request.SchoolYearCode, Semester = request.Semester, Status = request.Status }; db.SchoolYears.Add(year); await db.SaveChangesAsync();
        return Created($"/api/schoolyears/{year.Id}", new SchoolYearResponse(year.Id, year.SchoolYearCode, year.Semester, year.Status));
    }
}