using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrudeAspNet.Data;
using CrudeAspNet.Models;

namespace CrudeAspNet.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _db;
        public StudentsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var students = await _db.Students.ToListAsync();
            return View(students);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                _db.Add(student);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var student = await _db.Students.FindAsync(id);
            if (student == null) return NotFound();
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                _db.Update(student);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _db.Students.FindAsync(id);
            if (s != null)
            {
                _db.Students.Remove(s);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
