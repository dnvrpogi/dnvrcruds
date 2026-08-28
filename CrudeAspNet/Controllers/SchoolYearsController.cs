using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CrudeAspNet.Data;
using CrudeAspNet.Models;

namespace CrudeAspNet.Controllers
{
    public class SchoolYearsController : Controller
    {
        private readonly AppDbContext _db;
        public SchoolYearsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index() => View(await _db.SchoolYears.OrderByDescending(year => year.SchoolYearCode).ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SchoolYear schoolYear)
        {
            if (ModelState.IsValid)
            {
                if (await _db.SchoolYears.AnyAsync(existing => existing.SchoolYearCode == schoolYear.SchoolYearCode))
                {
                    ModelState.AddModelError(nameof(SchoolYear.SchoolYearCode), "That school-year code already exists.");
                }
                else
                {
                    try
                    {
                        _db.SchoolYears.Add(schoolYear);
                        await _db.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError(nameof(SchoolYear.SchoolYearCode), "That school-year code already exists or could not be saved.");
                    }
                }
            }
            return View(schoolYear);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var schoolYear = await _db.SchoolYears.FindAsync(id);
            return schoolYear == null ? NotFound() : View(schoolYear);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SchoolYear schoolYear)
        {
            if (id != schoolYear.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                _db.SchoolYears.Update(schoolYear);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schoolYear);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var schoolYear = await _db.SchoolYears.FindAsync(id);
            if (schoolYear != null)
            {
                _db.SchoolYears.Remove(schoolYear);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
