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
    public class StudentFieldsController : Controller
    {
        private readonly YasharDbContext _context;

        public StudentFieldsController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: StudentFields
        public async Task<IActionResult> Index()
        {
            var yasharDbContext = _context.StudentFields.Include(s => s.Field).Include(s => s.User);
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: StudentFields/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentField = await _context.StudentFields
                .Include(s => s.Field)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (studentField == null)
            {
                return NotFound();
            }

            return View(studentField);
        }

        // GET: StudentFields/Create
        public IActionResult Create()
        {
            ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: StudentFields/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FieldId")] StudentField studentField)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentField);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Id", studentField.FieldId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", studentField.UserId);
            return View(studentField);
        }

        // GET: StudentFields/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentField = await _context.StudentFields.FindAsync(id);
            if (studentField == null)
            {
                return NotFound();
            }
            ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Id", studentField.FieldId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", studentField.UserId);
            return View(studentField);
        }

        // POST: StudentFields/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FieldId")] StudentField studentField)
        {
            if (id != studentField.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentField);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentFieldExists(studentField.UserId))
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
            ViewData["FieldId"] = new SelectList(_context.Fields, "Id", "Id", studentField.FieldId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", studentField.UserId);
            return View(studentField);
        }

        // GET: StudentFields/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentField = await _context.StudentFields
                .Include(s => s.Field)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (studentField == null)
            {
                return NotFound();
            }

            return View(studentField);
        }

        // POST: StudentFields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentField = await _context.StudentFields.FindAsync(id);
            _context.StudentFields.Remove(studentField);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentFieldExists(int id)
        {
            return _context.StudentFields.Any(e => e.UserId == id);
        }
    }
}
