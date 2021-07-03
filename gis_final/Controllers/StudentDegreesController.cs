using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gis_final.Models;
using Microsoft.AspNetCore.Routing;

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
        public async Task<IActionResult> Index(int? id)
        {
            var yasharDbContext = _context.StudentDegrees.Include(s => s.User).Where(x => x.UserId == id);
            ViewBag.userId = id;
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
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = _context.Users.FirstOrDefault(x => x.Id == id);
            ViewData["UserEmail"] = user.Email;
            ViewData["UserId"] = user.Id;
            return View();
        }

        // POST: StudentDegrees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int userId, EnumDegree degreeId)
        {
            if (ModelState.IsValid)
            {
                StudentDegree studentDegree = new StudentDegree { UserId = userId, DegreeId = degreeId };
                _context.Add(studentDegree);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "StudentDegrees", new RouteValueDictionary(new { Id = studentDegree.UserId }));
            }

            StudentDegree studentDegree1 = await _context.StudentDegrees.FirstOrDefaultAsync(x => x.UserId == userId);

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", userId);
            return View();
        }

        // GET: StudentDegrees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudentDegree studentDegree = await _context.StudentDegrees.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);

            User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == studentDegree.UserId);
            ViewData["UserEmail"] = user.Email;
            ViewData["UserId"] = user.Id;
            return View();
        }

        // POST: StudentDegrees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int userId, [Bind("UserId,DegreeId,Id")] StudentDegree studentDegree)
        {
            if (id != studentDegree.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    studentDegree.UserId = userId;
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
                return RedirectToAction("Index", "StudentDegrees", new RouteValueDictionary(new { Id = studentDegree.UserId }));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", studentDegree.UserId);
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
            return RedirectToAction("Index", "StudentDegrees", new RouteValueDictionary(new { Id = studentDegree.UserId }));
        }

        private bool StudentDegreeExists(int id)
        {
            return _context.StudentDegrees.Any(e => e.Id == id);
        }
    }
}
