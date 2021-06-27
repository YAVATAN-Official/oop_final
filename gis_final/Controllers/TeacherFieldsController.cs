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
    public class TeacherFieldsController : Controller
    {
        private readonly YasharDbContext _context;

        public TeacherFieldsController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: TeacherFields
        public async Task<IActionResult> Index()
        {
            var yasharDbContext = _context.TeacherFields.Include(t => t.Field).Include(t => t.User);
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: TeacherFields/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherField = await _context.TeacherFields
                .Include(t => t.Field)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (teacherField == null)
            {
                return NotFound();
            }

            return View(teacherField);
        }

        // GET: TeacherFields/Create
        public IActionResult Create()
        {
            ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: TeacherFields/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FieldId")] TeacherField teacherField)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherField);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Id", teacherField.FieldId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", teacherField.UserId);
            return View(teacherField);
        }

        // GET: TeacherFields/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherField = await _context.TeacherFields.FindAsync(id);
            if (teacherField == null)
            {
                return NotFound();
            }
            ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Id", teacherField.FieldId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", teacherField.UserId);
            return View(teacherField);
        }

        // POST: TeacherFields/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FieldId")] TeacherField teacherField)
        {
            if (id != teacherField.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherField);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherFieldExists(teacherField.UserId))
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
            ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Id", teacherField.FieldId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", teacherField.UserId);
            return View(teacherField);
        }

        // GET: TeacherFields/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherField = await _context.TeacherFields
                .Include(t => t.Field)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (teacherField == null)
            {
                return NotFound();
            }

            return View(teacherField);
        }

        // POST: TeacherFields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherField = await _context.TeacherFields.FindAsync(id);
            _context.TeacherFields.Remove(teacherField);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherFieldExists(int id)
        {
            return _context.TeacherFields.Any(e => e.UserId == id);
        }
    }
}
