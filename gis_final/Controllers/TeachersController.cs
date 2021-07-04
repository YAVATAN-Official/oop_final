using gis_final.Models;
using gis_final.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Controllers
{
    public class TeachersController : Controller
    {
        private readonly YasharDbContext _context;

        public TeachersController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: TeachersController
        public ActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                var teacherViewModels = from a in _context.UserRoles.ToList()
                                        join b in _context.Users.ToList() on a.UserId equals b.Id
                                        join e in _context.Roles.ToList() on a.RoleId equals e.Id
                                        join c in _context.UserTags.ToList() on b.Id equals c.UserId into ut
                                        from u in ut.DefaultIfEmpty()
                                        join d in _context.Tags.ToList() on u?.TagId equals d.Id into tt
                                        from t in tt.DefaultIfEmpty()
                                        join tf in _context.TeacherFields.ToList() on b.Id equals tf.UserId into tfs
                                        from tfx in tfs.DefaultIfEmpty()
                                        join aaa in _context.Fields.ToList() on tfx?.FieldId equals aaa.Id into ssf
                                        from uus in ssf.DefaultIfEmpty()
                                        where a.Role.Title == "Teacher"
                                        select new TeacherViewModel
                                        {
                                            User = b,
                                            Roles = e,
                                            TagName = t?.Title ?? "Unknown",
                                            FieldName = uus?.Title ?? "Unknown"
                                        };

                return View(teacherViewModels);
            }
            else if (HttpContext.Session.GetString("Role") == "Teacher")
            {
                var teacherViewModels = from a in _context.UserRoles.ToList()
                                        join b in _context.Users.ToList() on a.UserId equals b.Id
                                        join e in _context.Roles.ToList() on a.RoleId equals e.Id
                                        join c in _context.UserTags.ToList() on b.Id equals c.UserId into ut
                                        from u in ut.DefaultIfEmpty()
                                        join d in _context.Tags.ToList() on u?.TagId equals d.Id into tt
                                        from t in tt.DefaultIfEmpty()
                                        join tf in _context.TeacherFields.ToList() on b.Id equals tf.UserId into tfs
                                        from tfx in tfs.DefaultIfEmpty()
                                        join aaa in _context.Fields.ToList() on tfx?.FieldId equals aaa.Id into ssf
                                        from uus in ssf.DefaultIfEmpty()
                                        where a.UserId == HttpContext.Session.GetInt32("UserId")
                                        select new TeacherViewModel
                                        {
                                            User = b,
                                            Roles = e,
                                            TagName = t?.Title ?? "Unknown",
                                            FieldName = uus?.Title ?? "Unknown"
                                        };

                return View(teacherViewModels);
            }
            return RedirectToAction("Login", "Home");
        }

        // GET: TeachersController
        public async Task<ActionResult> AddTag(int id)
        {
            ViewBag.UserId = id;
            return View(await _context.Tags.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AddTag(int id = 0, int userId = 0)
        {
            if (id == 0 || userId == 0)
            {
                return NotFound();
            }

            UserTags getUserTag = await _context.UserTags.Where(x => x.UserId == userId).FirstOrDefaultAsync();
            if (getUserTag != null)
            {
                _context.UserTags.Remove(getUserTag);
                await _context.SaveChangesAsync();
            }

            var newUserTag = new UserTags { TagId = id, UserId = userId };
            _context.UserTags.Add(newUserTag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> RemoveTag(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserTags getUserTag = await _context.UserTags.Where(x => x.UserId == id).FirstOrDefaultAsync();
            if (getUserTag != null)
            {
                _context.UserTags.Remove(getUserTag);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: TeachersController
        public async Task<ActionResult> AssignField(int id)
        {
            ViewBag.TeacherId = id;
            return View(await _context.Fields.Include(x => x.Faculty).ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> AssignField(int fieldId = 0, int teacherId = 0)
        {
            if (fieldId == 0 || teacherId == 0)
            {
                return NotFound();
            }

            TeacherField getTeacher = await _context.TeacherFields.FirstOrDefaultAsync(x => x.UserId == teacherId);
            if (getTeacher != null)
            {
                _context.TeacherFields.Remove(getTeacher);
                await _context.SaveChangesAsync();
            }

            var newField = new TeacherField { UserId = teacherId, FieldId = fieldId };
            _context.TeacherFields.Add(newField);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> RemoveField(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TeacherField getUser = await _context.TeacherFields.Where(x => x.UserId == id).FirstOrDefaultAsync();
            if (getUser != null)
            {
                _context.TeacherFields.Remove(getUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> AssignFieldCourses(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.teacherId = id;
            return View(await _context.FieldCourses.Include(x => x.Field).Include(f => f.Course).ToListAsync());
        }


        // GET: TeachersController
        public async Task<ActionResult> ConselorForStudents(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (HttpContext.Session.GetString("Role") == "Admin")
            {
                ViewBag.TeacherId = id;
                return View(await _context.StudentConselors.Include(x => x.User).Where(x => x.TeacherId == id).ToListAsync());
            }
            else if (HttpContext.Session.GetString("Role") == "Teacher")
            {
                ViewBag.TeacherId = HttpContext.Session.GetInt32("UserId");
                return View(await _context.StudentConselors.Include(x => x.User).Where(x => x.TeacherId == HttpContext.Session.GetInt32("UserId")).ToListAsync());
            }
            return RedirectToAction("Login", "Home");
        }

    }
}
