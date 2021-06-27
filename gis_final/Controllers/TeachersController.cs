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
            var teacherViewModels = from a in _context.UserRoles.ToList()
                    join b in _context.Users.ToList() on a.UserId equals b.Id
                    join e in _context.Roles.ToList() on a.RoleId equals e.Id
                    join c in _context.UserTags.ToList() on b.Id equals c.UserId into ut
                    from u in ut.DefaultIfEmpty()
                    join d in _context.Tags.ToList() on u?.TagId equals d.Id into tt
                    from t in tt.DefaultIfEmpty()
                    where a.Role.Title == "Teacher"
                    select new TeacherViewModel
                    {
                        User = b,
                        Roles = e,
                        TagName = t?.Title ?? String.Empty
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
        public async Task<ActionResult> AddTag(int id, int userId)
        {
            var checkUserTag = await _context.UserTags.Where(x => x.UserId == userId).ToListAsync();
            if (checkUserTag.Count() > 0)
            {
                UserTags userTags = await _context.UserTags.FirstOrDefaultAsync(x => x.UserId == userId);
                userTags.TagId = id;
                _context.UserTags.Update(userTags);
                await _context.SaveChangesAsync();
            }
            else
            {
                var ut = new UserTags { TagId = id, UserId = userId };
                _context.UserTags.Add(ut);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: TeachersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TeachersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TeachersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TeachersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TeachersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TeachersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TeachersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
