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
    public class FieldCoursesController : Controller
    {
        private readonly YasharDbContext _context;

        public FieldCoursesController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: FieldCourses
        public async Task<IActionResult> Index()
        {
            var yasharDbContext = _context.FieldCourses.Include(f => f.Course).Include(f => f.Field);
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: FieldCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fieldCourses = await _context.FieldCourses
                .Include(f => f.Course)
                .Include(f => f.Field)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (fieldCourses == null)
            {
                return NotFound();
            }

            return View(fieldCourses);
        }

        // GET: FieldCourses/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
            ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Id");
            return View();
        }

        // POST: FieldCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FieldId,CourseId,Id")] FieldCourses fieldCourses)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fieldCourses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", fieldCourses.CourseId);
            ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Id", fieldCourses.FieldId);
            return View(fieldCourses);
        }

        // GET: FieldCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fieldCourses = await _context.FieldCourses.FindAsync(id);
            if (fieldCourses == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", fieldCourses.CourseId);
            ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Id", fieldCourses.FieldId);
            return View(fieldCourses);
        }

        // POST: FieldCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FieldId,CourseId,Id")] FieldCourses fieldCourses)
        {
            if (id != fieldCourses.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fieldCourses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FieldCoursesExists(fieldCourses.CourseId))
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
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", fieldCourses.CourseId);
            ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Id", fieldCourses.FieldId);
            return View(fieldCourses);
        }

        // GET: FieldCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fieldCourses = await _context.FieldCourses
                .Include(f => f.Course)
                .Include(f => f.Field)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (fieldCourses == null)
            {
                return NotFound();
            }

            return View(fieldCourses);
        }

        // POST: FieldCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fieldCourses = await _context.FieldCourses.FindAsync(id);
            _context.FieldCourses.Remove(fieldCourses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FieldCoursesExists(int id)
        {
            return _context.FieldCourses.Any(e => e.CourseId == id);
        }
    }
}
