using gis_final.Models;
using gis_final.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Controllers
{
    public class StudentsController : Controller
    {
        private readonly YasharDbContext _context;

        public StudentsController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: TeachersController
        public ActionResult Index()
        {
            var teacherViewModels = from a in _context.UserRoles.ToList()
                                    join b in _context.Users.ToList() on a.UserId equals b.Id
                                    join e in _context.Roles.ToList() on a.RoleId equals e.Id
                                    join sg in _context.StudentGraduationStatuses.ToList() on b.Id equals sg.UserId
                                    join c in _context.StudentConselors on b.Id equals c.UserId into sc
                                    from u in sc.DefaultIfEmpty()
                                    join aa in _context.Users.ToList() on u?.TeacherId equals aa.Id into ssc
                                    from uu in ssc.DefaultIfEmpty()
                                    join sf in _context.StudentFields.ToList() on b.Id equals sf.UserId into sfs
                                    from sfx in sfs.DefaultIfEmpty()
                                    join aaa in _context.Fields.ToList() on sfx?.FieldId equals aaa.Id into ssf
                                    from uus in ssf.DefaultIfEmpty()
                                    where a.Role.Title == "Student" || a.Role.Title == "Assistant"
                                    select new StudentViewModel
                                    {
                                        User = b,
                                        Roles = e,
                                        ConselorName = uu?.Email ?? "Unknown",
                                        FieldName = uus?.Title ?? "Unknown",
                                        GraduationStatus = sg.GraduationStatusId
                                    };



            return View(teacherViewModels);
        }

        // GET: TeachersController
        public async Task<ActionResult> ToggleGraduationStatus(int id)
        {
            StudentGraduationStatus sgs = await _context.StudentGraduationStatuses.FirstOrDefaultAsync(x => x.UserId == id);
            if (sgs.GraduationStatusId == EnumStudentGraduationStatus.Graduate)
            {
                sgs.GraduationStatusId = EnumStudentGraduationStatus.Undergraduate;
            }
            else
            {
                sgs.GraduationStatusId = EnumStudentGraduationStatus.Graduate;
            }
            _context.StudentGraduationStatuses.Update(sgs);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: TeachersController
        public async Task<ActionResult> AssignConselor(int id)
        {
            ViewBag.StudentId = id;
            return View(await _context.UserRoles.Include(u => u.User).Include(r => r.Role).Where(x => x.Role.Title == "Teacher").ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AssignConselor(int teacherId = 0, int studentId = 0)
        {
            if (teacherId == 0 || studentId == 0)
            {
                return NotFound();
            }

            StudentConselor getStudent = await _context.StudentConselors.FirstOrDefaultAsync(x => x.UserId == studentId);
            if (getStudent != null)
            {
                _context.StudentConselors.Remove(getStudent);
                await _context.SaveChangesAsync();
            }

            var newConselor = new StudentConselor { UserId = studentId, TeacherId = teacherId };
            _context.StudentConselors.Add(newConselor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> RemoveConselor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudentConselor getUser = await _context.StudentConselors.Where(x => x.UserId == id).FirstOrDefaultAsync();
            if (getUser != null)
            {
                _context.StudentConselors.Remove(getUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: TeachersController
        public async Task<ActionResult> AssignField(int id)
        {
            ViewBag.StudentId = id;
            return View(await _context.Fields.Include(x => x.Faculty).ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AssignField(int fieldId = 0, int studentId = 0)
        {
            if (fieldId == 0 || studentId == 0)
            {
                return NotFound();
            }

            StudentField getStudent = await _context.StudentFields.FirstOrDefaultAsync(x => x.UserId == studentId);
            if (getStudent != null)
            {
                _context.StudentFields.Remove(getStudent);
                await _context.SaveChangesAsync();
            }

            var newField = new StudentField { UserId = studentId, FieldId = fieldId };
            _context.StudentFields.Add(newField);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> RemoveField(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudentField getUser = await _context.StudentFields.Where(x => x.UserId == id).FirstOrDefaultAsync();
            if (getUser != null)
            {
                _context.StudentFields.Remove(getUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<ActionResult> MyConselors()
        {
            var checkUser = await _context.StudentConselors.Include(x => x.User).Where(x => x.UserId == HttpContext.Session.GetInt32("UserID")).FirstOrDefaultAsync();
            if (checkUser != null)
            {
                return NotFound();
            }
            var getConselor = from a in _context.StudentConselors.ToList()
                              join b in _context.Users.ToList() on a.UserId equals b.Id
                              join c in _context.Users.ToList() on a.TeacherId equals c.Id
                              where b.Id == HttpContext.Session.GetInt32("UserId")
                              select c;
            return View(getConselor);
        }

        public ActionResult Schedule()
        {
            if (HttpContext.Session.GetString("Role") == "Student" || HttpContext.Session.GetString("Role") == "Assistant")
            {
                var s = from a in _context.Schedules.ToList()
                        join b in _context.TeacherFieldCourses.ToList() on a.TeacherFieldCourseId equals b.Id
                        join c in _context.FieldCourses.ToList() on b.FieldCoursesId equals c.Id
                        join d in _context.Fields.ToList() on c.FieldId equals d.Id
                        join e in _context.Courses.ToList() on c.CourseId equals e.Id
                        join f in _context.Users.ToList() on a.StudentId equals f.Id
                        join g in _context.Users.ToList() on b.UserId equals g.Id
                        join h in _context.YearTerms.ToList() on a.YearTermId equals h.Id
                        where a.StudentId == HttpContext.Session.GetInt32("UserId")
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


        public ActionResult AddSchedule()
        {
            if (HttpContext.Session.GetString("Role") == "Student" || HttpContext.Session.GetString("Role") == "Assistant")
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
                return View();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> AddSchedule(int TeacherFieldCourseId, int YearTermId)
        {
            if (HttpContext.Session.GetString("Role") == "Student" || HttpContext.Session.GetString("Role") == "Assistant")
            {
                var schedule = new Schedule
                {
                    EnumScoreStatusId = EnumScoreStatus.Unassigned,
                    Score = 0,
                    StudentId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId")),
                    TeacherFieldCourseId = TeacherFieldCourseId,
                    YearTermId = YearTermId
                };

                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Schedule));
            }
            return NotFound();
        }
         
        public async Task<ActionResult> DeleteSchedule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("Role") == "Student" || HttpContext.Session.GetString("Role") == "Assistant")
            {
                var schedule = await _context.Schedules.FirstOrDefaultAsync(x => x.Id == id);
                if (schedule == null)
                {
                    return NotFound();
                }
                 
                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Schedule));
            }
            return NotFound();
        }

    }
}
