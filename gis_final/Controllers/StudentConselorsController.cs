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
    public class StudentConselorsController : Controller
    {
        private readonly YasharDbContext _context;

        public StudentConselorsController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: StudentConselors
        public async Task<IActionResult> Index()
        {
            var yasharDbContext = _context.StudentConselors.Include(s => s.User);
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: StudentConselors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentConselor = await _context.StudentConselors
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentConselor == null)
            {
                return NotFound();
            }

            return View(studentConselor);
        }

        // GET: StudentConselors/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: StudentConselors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,TeacherId,Id")] StudentConselor studentConselor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentConselor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", studentConselor.UserId);
            return View(studentConselor);
        }

        // GET: StudentConselors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentConselor = await _context.StudentConselors.FindAsync(id);
            if (studentConselor == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", studentConselor.UserId);
            return View(studentConselor);
        }

        // POST: StudentConselors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,TeacherId,Id")] StudentConselor studentConselor)
        {
            if (id != studentConselor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentConselor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentConselorExists(studentConselor.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", studentConselor.UserId);
            return View(studentConselor);
        }

        // GET: StudentConselors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentConselor = await _context.StudentConselors
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentConselor == null)
            {
                return NotFound();
            }

            return View(studentConselor);
        }

        // POST: StudentConselors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentConselor = await _context.StudentConselors.FindAsync(id);
            _context.StudentConselors.Remove(studentConselor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentConselorExists(int id)
        {
            return _context.StudentConselors.Any(e => e.Id == id);
        }
    }
}
