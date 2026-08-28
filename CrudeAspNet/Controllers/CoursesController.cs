using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrudeAspNet.Data;
using CrudeAspNet.Models;

namespace CrudeAspNet.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;
        public CoursesController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index() => View(await _db.Courses.OrderBy(course => course.CourseCode).ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (ModelState.IsValid)
            {
                if (await _db.Courses.AnyAsync(existing => existing.CourseCode == course.CourseCode))
                {
                    ModelState.AddModelError(nameof(Course.CourseCode), "That course code already exists.");
                }
                else
                {
                    try
                    {
                        _db.Courses.Add(course);
                        await _db.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError(nameof(Course.CourseCode), "That course code already exists or could not be saved.");
                    }
                }
            }
            return View(course);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var course = await _db.Courses.FindAsync(id);
            return course == null ? NotFound() : View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Course course)
        {
            if (id != course.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                _db.Courses.Update(course);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _db.Courses.FindAsync(id);
            if (course != null)
            {
                _db.Courses.Remove(course);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
