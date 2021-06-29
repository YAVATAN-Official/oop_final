using gis_final.Models;
using gis_final.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Controllers
{
    public class AssistantsController : Controller
    {
        private readonly YasharDbContext _context;

        public AssistantsController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: AssistantController
        public ActionResult Index()
        {
            var teacherViewModels = from a in _context.UserRoles.ToList()
                                    join b in _context.Users.ToList() on a.UserId equals b.Id
                                    join e in _context.Roles.ToList() on a.RoleId equals e.Id
                                    join c in _context.UserTags.ToList() on b.Id equals c.UserId into ut
                                    from u in ut.DefaultIfEmpty()
                                    join d in _context.Tags.ToList() on u?.TagId equals d.Id into tt
                                    from t in tt.DefaultIfEmpty()
                                    where a.Role.Title == "Assistant"
                                    select new AssistantViewModel
                                    {
                                        User = b,
                                        Roles = e,
                                        TagName = t?.Title ?? "Unknown"
                                    };

            return View(teacherViewModels);
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
    }
}
