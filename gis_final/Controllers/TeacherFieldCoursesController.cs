using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gis_final.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using gis_final.ViewModels;

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
        public async Task<IActionResult> Index(int? teacherId)
        {
            if (teacherId == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("Role") == "Admin")
            {

                ViewBag.teacherId = teacherId;
                var yasharDbContext = _context.TeacherFieldCourses.Include(t => t.User).Include(x => x.FieldCourses).ThenInclude(f => f.Field).Include(fc => fc.FieldCourses).ThenInclude(c => c.Course).Where(x => x.UserId == teacherId);
                return View(await yasharDbContext.ToListAsync());
            }
            else if (HttpContext.Session.GetString("Role") == "Teacher")
            {

                ViewBag.teacherId = HttpContext.Session.GetInt32("UserId");
                var yasharDbContext = _context.TeacherFieldCourses.Include(t => t.User).Include(x => x.FieldCourses)
                    .ThenInclude(f => f.Field).Include(fc => fc.FieldCourses).ThenInclude(c => c.Course).Where(x => x.UserId == HttpContext.Session.GetInt32("UserId"));
                return View(await yasharDbContext.ToListAsync());
            }
            return RedirectToAction("Login", "Home");
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
            FieldCourses fc = _context.FieldCourses.Include(f => f.Field).Include(c => c.Course).FirstOrDefault(x => x.Id == fcId);
            ViewBag.Field = fc.Field.Title;
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
                    Time = time,
                    UserId = UserId
                };
                _context.Add(tfc);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "TeacherFieldCourses", new RouteValueDictionary(new { teacherId = UserId }));
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
                Time = time,
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

        // GET: TeacherFieldCourses/Edit/5
        public async Task<IActionResult> GetStudents(int? id)
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

            var getCourseStudents = from s in _context.Schedules.ToList()
                                    join u in _context.Users.ToList() on s.StudentId equals u.Id
                                    join tfc in _context.TeacherFieldCourses.ToList() on s.TeacherFieldCourseId equals tfc.Id
                                    join fc in _context.FieldCourses.ToList() on tfc.FieldCoursesId equals fc.Id
                                    join f in _context.Fields.ToList() on fc.FieldId equals f.Id
                                    join c in _context.Courses.ToList() on fc.CourseId equals c.Id
                                    join yt in _context.YearTerms.ToList() on s.YearTermId equals yt.Id
                                    where s.TeacherFieldCourseId == id
                                    select new CourseStudentsViewModel
                                    {
                                        Course = c,
                                        Field = f,
                                        Student = u,
                                        Term = yt.TermId,
                                        Year = yt.Year
                                    };

            return View(getCourseStudents);
        }

        // GET: TeacherFieldCourses/Edit/5
        public async Task<IActionResult> GetResearchAssistants(int? id)
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
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                var getResearchAssistants = from tfcra in _context.TeacherFieldCourseResearchAssistants.ToList()
                                            join tfc in _context.TeacherFieldCourses.ToList() on tfcra.TeacherFieldCourseId equals tfc.Id
                                            join fc in _context.FieldCourses.ToList() on tfc.FieldCoursesId equals fc.Id
                                            join f in _context.Fields.ToList() on fc.FieldId equals f.Id
                                            join c in _context.Courses.ToList() on fc.CourseId equals c.Id
                                            join ua in _context.Users.ToList() on tfcra.AssistantId equals ua.Id
                                            join ut in _context.Users.ToList() on tfc.UserId equals ut.Id
                                            select new GetResearchAssistantViewModel
                                            {
                                                Assistant = ua,
                                                Teacher = ut,
                                                TeacherFieldCourseResearchAssistant = tfcra,
                                                Field = f,
                                                Course = c
                                            };

                return View(getResearchAssistants);
            }
            else if (HttpContext.Session.GetString("Role") == "Teacher")
            {
                var getResearchAssistants = from tfcra in _context.TeacherFieldCourseResearchAssistants.ToList()
                                            join tfc in _context.TeacherFieldCourses.ToList() on tfcra.TeacherFieldCourseId equals tfc.Id
                                            join fc in _context.FieldCourses.ToList() on tfc.FieldCoursesId equals fc.Id
                                            join f in _context.Fields.ToList() on fc.FieldId equals f.Id
                                            join c in _context.Courses.ToList() on fc.CourseId equals c.Id
                                            join ua in _context.Users.ToList() on tfcra.AssistantId equals ua.Id
                                            join ut in _context.Users.ToList() on tfc.UserId equals ut.Id
                                            where ut.Id == HttpContext.Session.GetInt32("UserId")
                                            select new GetResearchAssistantViewModel
                                            {
                                                Assistant = ua,
                                                Teacher = ut,
                                                TeacherFieldCourseResearchAssistant = tfcra,
                                                Field = f,
                                                Course = c
                                            };

                return View(getResearchAssistants);
            }
            return NotFound();
        }

        // GET: TeachersController
        public ActionResult AssignResearchAssistant()
        {

            var getAssistant = from ur in _context.UserRoles.ToList()
                               join r in _context.Roles.ToList() on ur.RoleId equals r.Id
                               join u in _context.Users.ToList() on ur.UserId equals u.Id
                               where r.Title == "Assistant"
                               select new { Id = u.Id, Email = u.Email };

            ViewData["AssistantId"] = new SelectList(getAssistant, "Id", "Email");

            var tfcs = from tfc in _context.TeacherFieldCourses.ToList()
                       join u in _context.Users.ToList() on tfc.UserId equals u.Id
                       join fc in _context.FieldCourses.ToList() on tfc.FieldCoursesId equals fc.Id
                       join f in _context.Fields.ToList() on fc.FieldId equals f.Id
                       join c in _context.Courses.ToList() on fc.CourseId equals c.Id
                       select new
                       {
                           Id = tfc.Id,
                           MergedTeacherFieldCourse = u.Email + "-" + f.Title + "-" + c.Title
                       };
            ViewData["TeacherFieldCourseId"] = new SelectList(tfcs, "Id", "MergedTeacherFieldCourse");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddResearchAssistant(int TeacherFieldCourseId = 0, int AssistantId = 0)
        {
            if (TeacherFieldCourseId == 0 || AssistantId == 0 || HttpContext.Session.GetString("Role") != "Admin")
            {
                return NotFound();
            }

            TeacherFieldCourseResearchAssistant getUsers = await _context.TeacherFieldCourseResearchAssistants
                .FirstOrDefaultAsync(x => x.TeacherFieldCourseId == TeacherFieldCourseId && x.AssistantId == AssistantId);
            if (getUsers != null)
            {
                _context.TeacherFieldCourseResearchAssistants.Remove(getUsers);
                await _context.SaveChangesAsync();
            }

            var newTeacherResearchAssistant = new TeacherFieldCourseResearchAssistant { AssistantId = AssistantId, TeacherFieldCourseId = TeacherFieldCourseId };
            _context.TeacherFieldCourseResearchAssistants.Add(newTeacherResearchAssistant);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetResearchAssistants", "TeacherFieldCourses", new RouteValueDictionary(new { Id = TeacherFieldCourseId }));
        }

        // GET: TeachersController
        public async Task<ActionResult> DeleteAssistant(int? id)
        {
            if (id == null || HttpContext.Session.GetString("Role") != "Admin")
            {
                return NotFound();
            }

            TeacherFieldCourseResearchAssistant tra = await _context.TeacherFieldCourseResearchAssistants.FirstOrDefaultAsync(x => x.Id == id);
            if (tra == null)
            {
                return NotFound();
            }
            _context.TeacherFieldCourseResearchAssistants.Remove(tra);
            await _context.SaveChangesAsync();

            return RedirectToAction("GetResearchAssistants", "TeacherFieldCourses", new RouteValueDictionary(new { Id = tra.TeacherFieldCourseId }));
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
            ViewBag.teacherId = teacherFieldCourse.UserId;
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

            return RedirectToAction("Index", "TeacherFieldCourses", new RouteValueDictionary(new { teacherId = teacherFieldCourse.UserId }));

        }

        private bool TeacherFieldCourseExists(int id)
        {
            return _context.TeacherFieldCourses.Any(e => e.Id == id);
        }
    }
}
