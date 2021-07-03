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
    public class TeacherFieldCoursesController : Controller
    {
        private readonly YasharDbContext _context;

        public TeacherFieldCoursesController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: TeacherFieldCourses
        public async Task<IActionResult> Index()
        {
            var yasharDbContext = _context.TeacherFieldCourses.Include(t => t.User);
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: TeacherFieldCourses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherFieldCourse = await _context.TeacherFieldCourses
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacherFieldCourse == null)
            {
                return NotFound();
            }

            return View(teacherFieldCourse);
        }

        // GET: TeacherFieldCourses/Create
        public IActionResult Create(int? teacherId, int? fcId)
        {
            if (teacherId == null || fcId == null)
            {
                return NotFound();
            }

            User teacher = _context.Users.FirstOrDefault(x => x.Id == teacherId);
            ViewData["User"] = teacher.Email;
            ViewBag.teacherId = teacherId;
            ViewBag.fcId = fcId;
            FieldCourses fc = _context.FieldCourses.Include(f => f.Field).Include(c => c.Course).FirstOrDefault(x => x.FieldId == fcId);
            ViewBag.Fielc = fc.Field.Title;
            ViewBag.Course = fc.Course.Title;
            return View();
        }

        // POST: TeacherFieldCourses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int UserId, int FieldCoursesId, string time, EnumDays DayId, EnumStatus StatusId)
        {
            if (ModelState.IsValid)
            {
                TeacherFieldCourse tfc = new TeacherFieldCourse
                {
                    FieldCoursesId = FieldCoursesId,
                    DayId = DayId,
                    StatusId = StatusId,
                    time = time,
                    UserId = UserId
                };
                _context.Add(tfc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            User teacher = _context.Users.FirstOrDefault(x => x.Id == UserId);
            ViewData["User"] = teacher.Email;
            ViewBag.teacherId = UserId;
            ViewBag.fcId = FieldCoursesId;
            FieldCourses fc = _context.FieldCourses.Include(f => f.Field).Include(c => c.Course).FirstOrDefault(x => x.FieldId == FieldCoursesId);
            ViewBag.Fielc = fc.Field.Title;
            ViewBag.Course = fc.Course.Title;
            TeacherFieldCourse tfc1 = new TeacherFieldCourse
            {
                FieldCoursesId = FieldCoursesId,
                DayId = DayId,
                StatusId = StatusId,
                time = time,
                UserId = UserId
            };
            return View(tfc1);
        }

        // GET: TeacherFieldCourses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherFieldCourse = await _context.TeacherFieldCourses.FindAsync(id);
            if (teacherFieldCourse == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", teacherFieldCourse.UserId);
            return View(teacherFieldCourse);
        }

        // POST: TeacherFieldCourses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,FieldCoursesId,time,DayId,StatusId,Id")] TeacherFieldCourse teacherFieldCourse)
        {
            if (id != teacherFieldCourse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherFieldCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherFieldCourseExists(teacherFieldCourse.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", teacherFieldCourse.UserId);
            return View(teacherFieldCourse);
        }

        // GET: TeacherFieldCourses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherFieldCourse = await _context.TeacherFieldCourses
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacherFieldCourse == null)
            {
                return NotFound();
            }

            return View(teacherFieldCourse);
        }

        // POST: TeacherFieldCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherFieldCourse = await _context.TeacherFieldCourses.FindAsync(id);
            _context.TeacherFieldCourses.Remove(teacherFieldCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherFieldCourseExists(int id)
        {
            return _context.TeacherFieldCourses.Any(e => e.Id == id);
        }
    }
}
