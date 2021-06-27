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
    public class SchedulesController : Controller
    {
        private readonly YasharDbContext _context;

        public SchedulesController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var yasharDbContext = _context.Schedules.Include(s => s.TeacherFieldCourse).Include(s => s.YearTerm);
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.TeacherFieldCourse)
                .Include(s => s.YearTerm)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            ViewData["TeacherFieldCourseId"] = new SelectList(_context.TeacherFieldCourses, "Id", "Id");
            ViewData["YearTermId"] = new SelectList(_context.YearTerms, "Id", "Id");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherFieldCourseId,StudentId,YearTermId,Score,EnumScoreStatusId,Id")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherFieldCourseId"] = new SelectList(_context.TeacherFieldCourses, "Id", "Id", schedule.TeacherFieldCourseId);
            ViewData["YearTermId"] = new SelectList(_context.YearTerms, "Id", "Id", schedule.YearTermId);
            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["TeacherFieldCourseId"] = new SelectList(_context.TeacherFieldCourses, "Id", "Id", schedule.TeacherFieldCourseId);
            ViewData["YearTermId"] = new SelectList(_context.YearTerms, "Id", "Id", schedule.YearTermId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherFieldCourseId,StudentId,YearTermId,Score,EnumScoreStatusId,Id")] Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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
            ViewData["TeacherFieldCourseId"] = new SelectList(_context.TeacherFieldCourses, "Id", "Id", schedule.TeacherFieldCourseId);
            ViewData["YearTermId"] = new SelectList(_context.YearTerms, "Id", "Id", schedule.YearTermId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.TeacherFieldCourse)
                .Include(s => s.YearTerm)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.Id == id);
        }
    }
}
