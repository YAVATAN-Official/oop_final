using gis_final.Models;
using gis_final.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                                    join c in _context.StudentConselors on b.Id equals c.UserId into sc
                                    from u in sc.DefaultIfEmpty()
                                    join aa in _context.Users.ToList() on u?.TeacherId equals aa.Id into ssc
                                    from uu in ssc.DefaultIfEmpty()
                                    where a.Role.Title == "Student"
                                    select new StudentViewModel
                                    {
                                        User = b,
                                        Roles = e,
                                        ConselorName = uu?.Email ?? "Unknown"
                                    };



            return View(teacherViewModels);
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
    }
}
