using CrudeAspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudeAspNet.Api;

[ApiController]
[Route("api/courses")]
public class CoursesApiController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseResponse>>> GetAll() => Ok(await db.Courses.AsNoTracking().OrderBy(course => course.CourseCode).Select(course => new CourseResponse(course.Id, course.CourseCode, course.CourseName)).ToListAsync());

    [HttpPost]
    public async Task<ActionResult<CourseResponse>> Create(CourseRequest request)
    {
        if (await db.Courses.AnyAsync(course => course.CourseCode == request.CourseCode)) return Conflict(new { message = "That course code already exists." });
        var course = new Models.Course { CourseCode = request.CourseCode, CourseName = request.CourseName }; db.Courses.Add(course); await db.SaveChangesAsync();
        return Created($"/api/courses/{course.Id}", new CourseResponse(course.Id, course.CourseCode, course.CourseName));
    }
}