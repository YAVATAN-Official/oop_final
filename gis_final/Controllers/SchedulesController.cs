using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gis_final.Models;
using gis_final.ViewModels;
using Microsoft.AspNetCore.Http;

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
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                var s = from a in _context.Schedules.ToList()
                        join b in _context.TeacherFieldCourses.ToList() on a.TeacherFieldCourseId equals b.Id
                        join c in _context.FieldCourses.ToList() on b.FieldCoursesId equals c.Id
                        join d in _context.Fields.ToList() on c.FieldId equals d.Id
                        join e in _context.Courses.ToList() on c.CourseId equals e.Id
                        join f in _context.Users.ToList() on a.StudentId equals f.Id
                        join g in _context.Users.ToList() on b.UserId equals g.Id
                        join h in _context.YearTerms.ToList() on a.YearTermId equals h.Id
                        select new ScheduleViewModel
                        {
                            Schedule = a,
                            Student = f,
                            Teacher = g,
                            Course = e,
                            YearTerm = h,
                            Field = d,
                            Score = a.Score,
                            ScoreStatus = a.EnumScoreStatusId
                        };
                return View(s);
            }
            else if (HttpContext.Session.GetString("Role") == "Teacher")
            {
                var s = from a in _context.Schedules.ToList()
                        join b in _context.TeacherFieldCourses.ToList() on a.TeacherFieldCourseId equals b.Id
                        join c in _context.FieldCourses.ToList() on b.FieldCoursesId equals c.Id
                        join d in _context.Fields.ToList() on c.FieldId equals d.Id
                        join e in _context.Courses.ToList() on c.CourseId equals e.Id
                        join f in _context.Users.ToList() on a.StudentId equals f.Id
                        join g in _context.Users.ToList() on b.UserId equals g.Id
                        join h in _context.YearTerms.ToList() on a.YearTermId equals h.Id
                        where b.UserId == HttpContext.Session.GetInt32("UserId")
                        select new ScheduleViewModel
                        {
                            Schedule = a,
                            Student = f,
                            Teacher = g,
                            Course = e,
                            YearTerm = h,
                            Field = d,
                            Score = a.Score,
                            ScoreStatus = a.EnumScoreStatusId
                        };
                return View(s);
            }
            return NotFound();
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
            var tfc = from a in _context.TeacherFieldCourses.ToList()
                      join b in _context.FieldCourses.ToList() on a.FieldCoursesId equals b.Id
                      join c in _context.Courses.ToList() on b.CourseId equals c.Id
                      join d in _context.Fields.ToList() on b.FieldId equals d.Id
                      join e in _context.Users.ToList() on a.UserId equals e.Id
                      select new
                      {
                          Id = a.Id,
                          MergedTeacherFieldCourse = e.Email + " - " + d.Title + " - " + c.Title
                      };
            ViewData["TeacherFieldCourseId"] = new SelectList(tfc, "Id", "MergedTeacherFieldCourse");

            var yearTerm = from a in _context.YearTerms.ToList()
                           select new
                           {
                               Id = a.Id,
                               MergedYearTerm = a.Year + " - " + a.TermId
                           };
            ViewData["YearTermId"] = new SelectList(yearTerm, "Id", "MergedYearTerm");
            var student = from a in _context.UserRoles.ToList()
                          join b in _context.Users.ToList() on a.UserId equals b.Id
                          join c in _context.Roles.ToList() on a.RoleId equals c.Id
                          where c.Title == "Assistant" || c.Title == "Student"
                          select new User
                          {
                              Id = b.Id,
                              Email = b.Email
                          };
            ViewData["Student"] = new SelectList(student, "Id", "Email");
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
                var GetStudentDegree = _context.StudentDegrees.FirstOrDefault(x => x.UserId == schedule.StudentId).DegreeId;
                if (GetStudentDegree == EnumDegree.MasterWithoutThesis || GetStudentDegree == EnumDegree.MasterWithThesis)
                {
                    if (schedule.Score >= 70)
                    {
                        schedule.EnumScoreStatusId = EnumScoreStatus.Passed;
                    }
                    else if (schedule.Score == 0)
                    {
                        schedule.EnumScoreStatusId = EnumScoreStatus.Unassigned;
                    }
                    else
                    {
                        schedule.EnumScoreStatusId = EnumScoreStatus.Refused;
                    }
                }
                else
                {
                    if (schedule.Score >= 75)
                    {
                        schedule.EnumScoreStatusId = EnumScoreStatus.Passed;
                    }
                    else if (schedule.Score == 0)
                    {
                        schedule.EnumScoreStatusId = EnumScoreStatus.Unassigned;
                    }
                    else
                    {
                        schedule.EnumScoreStatusId = EnumScoreStatus.Refused;
                    }
                }
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

            var tfc = from a in _context.TeacherFieldCourses.ToList()
                      join b in _context.FieldCourses.ToList() on a.FieldCoursesId equals b.Id
                      join c in _context.Courses.ToList() on b.CourseId equals c.Id
                      join d in _context.Fields.ToList() on b.FieldId equals d.Id
                      join e in _context.Users.ToList() on a.UserId equals e.Id
                      join f in _context.Schedules.ToList() on a.Id equals f.TeacherFieldCourseId
                      where a.Id == schedule.TeacherFieldCourseId
                      select new
                      {
                          Id = a.Id,
                          MergedTeacherFieldCourse = e.Email + " - " + d.Title + " - " + c.Title
                      };
            ViewData["TeacherFieldCourseId"] = new SelectList(tfc, "Id", "MergedTeacherFieldCourse");

            var yearTerm = from a in _context.YearTerms.ToList()
                           join b in _context.Schedules.ToList() on a.Id equals b.YearTermId
                           join c in _context.Users.ToList() on b.StudentId equals c.Id
                           where a.Id == schedule.YearTermId && c.Id == schedule.StudentId
                           select new
                           {
                               Id = a.Id,
                               MergedYearTerm = a.Year + " - " + a.TermId
                           };
            ViewData["YearTermId"] = new SelectList(yearTerm, "Id", "MergedYearTerm");
            var student = from a in _context.UserRoles.ToList()
                          join b in _context.Users.ToList() on a.UserId equals b.Id
                          join c in _context.Roles.ToList() on a.RoleId equals c.Id
                          join d in _context.Schedules.ToList() on a.UserId equals d.StudentId
                          where b.Id == schedule.StudentId && (c.Title == "Assistant" || c.Title == "Student")
                          select new User
                          {
                              Id = b.Id,
                              Email = b.Email
                          };
            ViewData["Student"] = new SelectList(student, "Id", "Email");
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
                    var GetStudentDegree = _context.StudentDegrees.FirstOrDefault(x => x.UserId == schedule.StudentId).DegreeId;
                    if (GetStudentDegree == EnumDegree.MasterWithoutThesis || GetStudentDegree == EnumDegree.MasterWithThesis)
                    {
                        if (schedule.Score >= 70)
                        {
                            schedule.EnumScoreStatusId = EnumScoreStatus.Passed;
                        }
                        else if (schedule.Score == 0)
                        {
                            schedule.EnumScoreStatusId = EnumScoreStatus.Unassigned;
                        }
                        else
                        {
                            schedule.EnumScoreStatusId = EnumScoreStatus.Refused;
                        }
                    }
                    else
                    {
                        if (schedule.Score >= 75)
                        {
                            schedule.EnumScoreStatusId = EnumScoreStatus.Passed;
                        }
                        else if (schedule.Score == 0)
                        {
                            schedule.EnumScoreStatusId = EnumScoreStatus.Unassigned;
                        }
                        else
                        {
                            schedule.EnumScoreStatusId = EnumScoreStatus.Refused;
                        }
                    }
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

            var tfc = from a in _context.TeacherFieldCourses.ToList()
                      join b in _context.FieldCourses.ToList() on a.FieldCoursesId equals b.Id
                      join c in _context.Courses.ToList() on b.CourseId equals c.Id
                      join d in _context.Fields.ToList() on b.FieldId equals d.Id
                      join e in _context.Users.ToList() on a.UserId equals e.Id
                      where a.Id == id
                      select new
                      {
                          MergedTeacherFieldCourse = e.Email + " - " + d.Title + " - " + c.Title,
                          StudentId = a.UserId
                      };
            ViewBag.TeacherFieldCourse = tfc.FirstOrDefault().MergedTeacherFieldCourse;
            var schedule = await _context.Schedules
                .Include(s => s.TeacherFieldCourse)
                .Include(s => s.YearTerm)
                .FirstOrDefaultAsync(m => m.Id == id);

            ViewBag.User = _context.Users.FirstOrDefault(x => x.Id == schedule.StudentId).Email;

            var yearTerm = from a in _context.YearTerms.ToList()
                           where a.Id == schedule.YearTermId
                           select new
                           {
                               MergedYearTerm = a.Year + " - " + a.TermId
                           };
            ViewData["YearTerm"] = yearTerm.FirstOrDefault().MergedYearTerm;
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
