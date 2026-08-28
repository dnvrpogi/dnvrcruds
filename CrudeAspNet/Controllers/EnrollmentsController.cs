using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrudeAspNet.Data;
using CrudeAspNet.Models;

namespace CrudeAspNet.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly AppDbContext _db;
        public EnrollmentsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index() => View(await _db.Enrollments
            .AsNoTracking()
            .Include(enrollment => enrollment.Student)
            .Include(enrollment => enrollment.Course)
            .Include(enrollment => enrollment.SchoolYear)
            .OrderBy(enrollment => enrollment.Student.StudentName)
            .ToListAsync());

        public async Task<IActionResult> Create()
        {
            await LoadOptions();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                var alreadyEnrolled = await _db.Enrollments.AnyAsync(existing =>
                    existing.StudentId == enrollment.StudentId &&
                    existing.CourseId == enrollment.CourseId &&
                    existing.SchoolYearId == enrollment.SchoolYearId);

                if (alreadyEnrolled)
                {
                    ModelState.AddModelError(string.Empty, "This student is already enrolled in that course for the selected school year.");
                }
                else
                {
                    try
                    {
                        _db.Enrollments.Add(enrollment);
                        await _db.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError(string.Empty, "The enrollment could not be saved. Please verify that the selected student, course, and school year still exist.");
                    }
                }
            }
            await LoadOptions();
            return View(enrollment);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var enrollment = await _db.Enrollments.FindAsync(id);
            if (enrollment == null) return NotFound();
            await LoadOptions();
            return View(enrollment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Enrollment enrollment)
        {
            if (id != enrollment.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                var alreadyEnrolled = await _db.Enrollments.AnyAsync(existing =>
                    existing.Id != enrollment.Id &&
                    existing.StudentId == enrollment.StudentId &&
                    existing.CourseId == enrollment.CourseId &&
                    existing.SchoolYearId == enrollment.SchoolYearId);

                if (alreadyEnrolled)
                {
                    ModelState.AddModelError(string.Empty, "This student is already enrolled in that course for the selected school year.");
                }
                else
                {
                    try
                    {
                        _db.Enrollments.Update(enrollment);
                        await _db.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError(string.Empty, "The enrollment could not be saved. Please verify that the selected student, course, and school year still exist.");
                    }
                }
            }
            await LoadOptions();
            return View(enrollment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var enrollment = await _db.Enrollments.FindAsync(id);
            if (enrollment != null)
            {
                _db.Enrollments.Remove(enrollment);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task LoadOptions()
        {
            ViewBag.Students = new SelectList(await _db.Students.AsNoTracking().OrderBy(student => student.StudentName).ToListAsync(), "Id", "StudentName");
            ViewBag.Courses = new SelectList(await _db.Courses.AsNoTracking().OrderBy(course => course.CourseCode).ToListAsync(), "Id", "CourseCode");
            ViewBag.SchoolYears = new SelectList(await _db.SchoolYears.AsNoTracking().OrderByDescending(year => year.SchoolYearCode).ToListAsync(), "Id", "SchoolYearCode");
        }
    }
}
