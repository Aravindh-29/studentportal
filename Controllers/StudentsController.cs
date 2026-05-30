// ── StudentsController.cs ─────────────────────────────────────────────────────
// Drop this into Controllers/StudentsController.cs
// Adjust namespace and model fields to match yours if they differ.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;

namespace StudentPortal.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET /Students
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students.ToListAsync();
            return View(students);
        }

        // GET /Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // GET /Students/Create
        public IActionResult Create() => View();

        // POST /Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (!ModelState.IsValid) return View(student);

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Student \"{student.Name}\" added successfully.";
            return RedirectToAction(nameof(Index));
        }

        // GET /Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST /Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id) return NotFound();
            if (!ModelState.IsValid) return View(student);

            try
            {
                _context.Students.Update(student);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Student \"{student.Name}\" updated successfully.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Students.Any(s => s.Id == id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET /Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            return View(student);
        }

        // POST /Students/Delete/5
        // The view uses asp-action="Delete" asp-route-id=... — this matches.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Student \"{student.Name}\" deleted.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
