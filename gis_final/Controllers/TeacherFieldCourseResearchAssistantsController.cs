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
    public class TeacherFieldCourseResearchAssistantsController : Controller
    {
        private readonly YasharDbContext _context;

        public TeacherFieldCourseResearchAssistantsController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: TeacherFieldCourseResearchAssistants
        public async Task<IActionResult> Index()
        {
            var yasharDbContext = _context.TeacherFieldCourseResearchAssistants.Include(t => t.TeacherFieldCourse);
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: TeacherFieldCourseResearchAssistants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherFieldCourseResearchAssistant = await _context.TeacherFieldCourseResearchAssistants
                .Include(t => t.TeacherFieldCourse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacherFieldCourseResearchAssistant == null)
            {
                return NotFound();
            }

            return View(teacherFieldCourseResearchAssistant);
        }

        // GET: TeacherFieldCourseResearchAssistants/Create
        public IActionResult Create()
        {
            ViewData["TeacherFieldCourseId"] = new SelectList(_context.TeacherFieldCourses, "Id", "Id");
            return View();
        }

        // POST: TeacherFieldCourseResearchAssistants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssistantId,TeacherFieldCourseId,Id")] TeacherFieldCourseResearchAssistant teacherFieldCourseResearchAssistant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherFieldCourseResearchAssistant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeacherFieldCourseId"] = new SelectList(_context.TeacherFieldCourses, "Id", "Id", teacherFieldCourseResearchAssistant.TeacherFieldCourseId);
            return View(teacherFieldCourseResearchAssistant);
        }

        // GET: TeacherFieldCourseResearchAssistants/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherFieldCourseResearchAssistant = await _context.TeacherFieldCourseResearchAssistants.FindAsync(id);
            if (teacherFieldCourseResearchAssistant == null)
            {
                return NotFound();
            }
            ViewData["TeacherFieldCourseId"] = new SelectList(_context.TeacherFieldCourses, "Id", "Id", teacherFieldCourseResearchAssistant.TeacherFieldCourseId);
            return View(teacherFieldCourseResearchAssistant);
        }

        // POST: TeacherFieldCourseResearchAssistants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssistantId,TeacherFieldCourseId,Id")] TeacherFieldCourseResearchAssistant teacherFieldCourseResearchAssistant)
        {
            if (id != teacherFieldCourseResearchAssistant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherFieldCourseResearchAssistant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherFieldCourseResearchAssistantExists(teacherFieldCourseResearchAssistant.Id))
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
            ViewData["TeacherFieldCourseId"] = new SelectList(_context.TeacherFieldCourses, "Id", "Id", teacherFieldCourseResearchAssistant.TeacherFieldCourseId);
            return View(teacherFieldCourseResearchAssistant);
        }

        // GET: TeacherFieldCourseResearchAssistants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherFieldCourseResearchAssistant = await _context.TeacherFieldCourseResearchAssistants
                .Include(t => t.TeacherFieldCourse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacherFieldCourseResearchAssistant == null)
            {
                return NotFound();
            }

            return View(teacherFieldCourseResearchAssistant);
        }

        // POST: TeacherFieldCourseResearchAssistants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherFieldCourseResearchAssistant = await _context.TeacherFieldCourseResearchAssistants.FindAsync(id);
            _context.TeacherFieldCourseResearchAssistants.Remove(teacherFieldCourseResearchAssistant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherFieldCourseResearchAssistantExists(int id)
        {
            return _context.TeacherFieldCourseResearchAssistants.Any(e => e.Id == id);
        }
    }
}
