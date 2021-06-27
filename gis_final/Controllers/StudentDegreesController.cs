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
    public class StudentDegreesController : Controller
    {
        private readonly YasharDbContext _context;

        public StudentDegreesController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: StudentDegrees
        public async Task<IActionResult> Index()
        {
            var yasharDbContext = _context.StudentDegrees.Include(s => s.User);
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: StudentDegrees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentDegree = await _context.StudentDegrees
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentDegree == null)
            {
                return NotFound();
            }

            return View(studentDegree);
        }

        // GET: StudentDegrees/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: StudentDegrees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,DegreeId,Id")] StudentDegree studentDegree)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentDegree);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", studentDegree.UserId);
            return View(studentDegree);
        }

        // GET: StudentDegrees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentDegree = await _context.StudentDegrees.FindAsync(id);
            if (studentDegree == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", studentDegree.UserId);
            return View(studentDegree);
        }

        // POST: StudentDegrees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,DegreeId,Id")] StudentDegree studentDegree)
        {
            if (id != studentDegree.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentDegree);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentDegreeExists(studentDegree.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", studentDegree.UserId);
            return View(studentDegree);
        }

        // GET: StudentDegrees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentDegree = await _context.StudentDegrees
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentDegree == null)
            {
                return NotFound();
            }

            return View(studentDegree);
        }

        // POST: StudentDegrees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentDegree = await _context.StudentDegrees.FindAsync(id);
            _context.StudentDegrees.Remove(studentDegree);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentDegreeExists(int id)
        {
            return _context.StudentDegrees.Any(e => e.Id == id);
        }
    }
}
