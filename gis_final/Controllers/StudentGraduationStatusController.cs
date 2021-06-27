using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gis_final.Models;

namespace gis_final.Controllers
{
    public class StudentGraduationStatusController : Controller
    {
        private readonly YasharDbContext _context;

        public StudentGraduationStatusController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: StudentGraduationStatus
        public async Task<IActionResult> Index()
        {
            var yasharDbContext = _context.StudentGraduationStatuses.Include(s => s.User);
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: StudentGraduationStatus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentGraduationStatus = await _context.StudentGraduationStatuses
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentGraduationStatus == null)
            {
                return NotFound();
            }

            return View(studentGraduationStatus);
        }

        // GET: StudentGraduationStatus/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: StudentGraduationStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,GraduationStatusId,Id")] StudentGraduationStatus studentGraduationStatus)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentGraduationStatus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", studentGraduationStatus.UserId);
            return View(studentGraduationStatus);
        }

        // GET: StudentGraduationStatus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentGraduationStatus = await _context.StudentGraduationStatuses.FindAsync(id);
            if (studentGraduationStatus == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", studentGraduationStatus.UserId);
            return View(studentGraduationStatus);
        }

        // POST: StudentGraduationStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,GraduationStatusId,Id")] StudentGraduationStatus studentGraduationStatus)
        {
            if (id != studentGraduationStatus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentGraduationStatus);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentGraduationStatusExists(studentGraduationStatus.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", studentGraduationStatus.UserId);
            return View(studentGraduationStatus);
        }

        // GET: StudentGraduationStatus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentGraduationStatus = await _context.StudentGraduationStatuses
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentGraduationStatus == null)
            {
                return NotFound();
            }

            return View(studentGraduationStatus);
        }

        // POST: StudentGraduationStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentGraduationStatus = await _context.StudentGraduationStatuses.FindAsync(id);
            _context.StudentGraduationStatuses.Remove(studentGraduationStatus);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentGraduationStatusExists(int id)
        {
            return _context.StudentGraduationStatuses.Any(e => e.Id == id);
        }
    }
}
