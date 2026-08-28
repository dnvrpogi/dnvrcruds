using CrudeAspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudeAspNet.Api;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsApiController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnrollmentResponse>>> GetAll() => Ok(await db.Enrollments.AsNoTracking().Include(item => item.Student).Include(item => item.Course).Include(item => item.SchoolYear).OrderBy(item => item.Id).Select(item => new EnrollmentResponse(item.Id, item.StudentId, item.Student!.StudentName, item.CourseId, item.Course!.CourseCode, item.SchoolYearId, item.SchoolYear!.SchoolYearCode)).ToListAsync());

    [HttpPost]
    public async Task<ActionResult<EnrollmentResponse>> Create(EnrollmentRequest request)
    {
        if (!await db.Students.AnyAsync(item => item.Id == request.StudentId) || !await db.Courses.AnyAsync(item => item.Id == request.CourseId) || !await db.SchoolYears.AnyAsync(item => item.Id == request.SchoolYearId)) return BadRequest(new { message = "The student, course, and school year must exist." });
        if (await db.Enrollments.AnyAsync(item => item.StudentId == request.StudentId && item.CourseId == request.CourseId && item.SchoolYearId == request.SchoolYearId)) return Conflict(new { message = "That enrollment already exists." });
        var enrollment = new Models.Enrollment { StudentId = request.StudentId, CourseId = request.CourseId, SchoolYearId = request.SchoolYearId }; db.Enrollments.Add(enrollment); await db.SaveChangesAsync();
        var result = await db.Enrollments.AsNoTracking().Include(item => item.Student).Include(item => item.Course).Include(item => item.SchoolYear).Where(item => item.Id == enrollment.Id).Select(item => new EnrollmentResponse(item.Id, item.StudentId, item.Student!.StudentName, item.CourseId, item.Course!.CourseCode, item.SchoolYearId, item.SchoolYear!.SchoolYearCode)).SingleAsync();
        return Created($"/api/enrollments/{enrollment.Id}", result);
    }
}