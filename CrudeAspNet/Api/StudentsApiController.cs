using CrudeAspNet.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudeAspNet.Api;

[ApiController]
[Route("api/students")]
public class StudentsApiController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<StudentResponse>>> GetAll() =>
        Ok(await db.Students.AsNoTracking().OrderBy(student => student.StudentName)
            .Select(student => new StudentResponse(student.Id, student.StudentName, student.StudentId, student.Email)).ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StudentResponse>> Get(int id)
    {
        var student = await db.Students.AsNoTracking().FirstOrDefaultAsync(student => student.Id == id);
        return student == null ? NotFound() : Ok(new StudentResponse(student.Id, student.StudentName, student.StudentId, student.Email));
    }

    [HttpPost]
    public async Task<ActionResult<StudentResponse>> Create(StudentRequest request)
    {
        if (await db.Students.AnyAsync(student => student.StudentId == request.StudentId))
            return Conflict(new { message = "That student ID already exists." });
        var student = new Models.Student { StudentName = request.StudentName, StudentId = request.StudentId, Email = request.Email };
        db.Students.Add(student);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = student.Id }, new StudentResponse(student.Id, student.StudentName, student.StudentId, student.Email));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, StudentRequest request)
    {
        var student = await db.Students.FindAsync(id);
        if (student == null) return NotFound();
        student.StudentName = request.StudentName; student.StudentId = request.StudentId; student.Email = request.Email;
        await db.SaveChangesAsync();
        return Ok(new StudentResponse(student.Id, student.StudentName, student.StudentId, student.Email));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var student = await db.Students.FindAsync(id);
        if (student == null) return NotFound();
        db.Students.Remove(student); await db.SaveChangesAsync(); return NoContent();
    }
}