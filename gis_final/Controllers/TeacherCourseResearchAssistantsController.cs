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
    public class TeacherCourseResearchAssistantsController : Controller
    {
        private readonly YasharDbContext _context;

        public TeacherCourseResearchAssistantsController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: TeacherCourseResearchAssistants
        public async Task<IActionResult> Index()
        {
            var yasharDbContext = _context.TeacherCourseResearchAssistants.Include(t => t.TeacherFieldCourse);
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: TeacherCourseResearchAssistants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherCourseResearchAssistant = await _context.TeacherCourseResearchAssistants
                .Include(t => t.TeacherFieldCourse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacherCourseResearchAssistant == null)
            {
                return NotFound();
            }

            return View(teacherCourseResearchAssistant);
        }

        // GET: TeacherCourseResearchAssistants/Create
        public IActionResult Create()
        {
            ViewData["TeacherFieldCourseId"] = new SelectList(_context.TeacherFieldCourses, "Id", "Id");
            return View();
        }

        // POST: TeacherCourseResearchAssistants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResearchAssistantId,TeacherFieldCourseId,Id")] TeacherCourseResearchAssistant teacherCourseResearchAssistant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherCourseResearchAssistant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherFieldCourseId"] = new SelectList(_context.TeacherFieldCourses, "Id", "Id", teacherCourseResearchAssistant.TeacherFieldCourseId);
            return View(teacherCourseResearchAssistant);
        }

        // GET: TeacherCourseResearchAssistants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherCourseResearchAssistant = await _context.TeacherCourseResearchAssistants.FindAsync(id);
            if (teacherCourseResearchAssistant == null)
            {
                return NotFound();
            }
            ViewData["TeacherFieldCourseId"] = new SelectList(_context.TeacherFieldCourses, "Id", "Id", teacherCourseResearchAssistant.TeacherFieldCourseId);
            return View(teacherCourseResearchAssistant);
        }

        // POST: TeacherCourseResearchAssistants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResearchAssistantId,TeacherFieldCourseId,Id")] TeacherCourseResearchAssistant teacherCourseResearchAssistant)
        {
            if (id != teacherCourseResearchAssistant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherCourseResearchAssistant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherCourseResearchAssistantExists(teacherCourseResearchAssistant.Id))
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
            ViewData["TeacherFieldCourseId"] = new SelectList(_context.TeacherFieldCourses, "Id", "Id", teacherCourseResearchAssistant.TeacherFieldCourseId);
            return View(teacherCourseResearchAssistant);
        }

        // GET: TeacherCourseResearchAssistants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherCourseResearchAssistant = await _context.TeacherCourseResearchAssistants
                .Include(t => t.TeacherFieldCourse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacherCourseResearchAssistant == null)
            {
                return NotFound();
            }

            return View(teacherCourseResearchAssistant);
        }

        // POST: TeacherCourseResearchAssistants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherCourseResearchAssistant = await _context.TeacherCourseResearchAssistants.FindAsync(id);
            _context.TeacherCourseResearchAssistants.Remove(teacherCourseResearchAssistant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherCourseResearchAssistantExists(int id)
        {
            return _context.TeacherCourseResearchAssistants.Any(e => e.Id == id);
        }
    }
}
