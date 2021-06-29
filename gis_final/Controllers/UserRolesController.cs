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
    public class UserRolesController : Controller
    {
        private readonly YasharDbContext _context;

        public UserRolesController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: UserRoles
        public async Task<IActionResult> Index()
        {
            var yasharDbContext = _context.UserRoles.Include(u => u.Role).Include(u => u.User);
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: UserRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRoles = await _context.UserRoles
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userRoles == null)
            {
                return NotFound();
            }

            return View(userRoles);
        }

        // GET: UserRoles/Create
        public IActionResult Create()
        {
            fillSelectLists();
            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId,View,Create,Update,Delete,Confirm")] UserRoles userRoles)
        {
            if (ModelState.IsValid)
            {
                List<UserRoles> checkUserRoles = await _context.UserRoles.Where(x => x.UserId == userRoles.UserId && x.RoleId == userRoles.RoleId).ToListAsync();
                foreach (var item in checkUserRoles)
                {
                    if (item.RoleId == userRoles.RoleId)
                    {
                        fillSelectLists(userRoles);
                        ViewBag.Alert = "This role previously assigned!";
                        return View();
                    }
                }
                _context.Add(userRoles);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            fillSelectLists(userRoles);
            return View(userRoles);
        }

        // GET: UserRoles/Edit/5
        public async Task<IActionResult> Edit(int? id, int? roleId)
        {
            if (id == null || roleId == null)
            {
                return NotFound();
            }

            var userRoles = await _context.UserRoles.Where(x => x.UserId == id && x.RoleId == roleId).FirstOrDefaultAsync();
            if (userRoles == null)
            {
                return NotFound();
            }
            User selectedUser = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            ViewData["Email"] = selectedUser.Email;
            fillSelectLists(userRoles);
            return View(userRoles);
        }

        // POST: UserRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int newRoleId, [Bind("UserId,RoleId,View,Create,Update,Delete,Confirm")] UserRoles userRoles)
        {
            if (id != userRoles.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.UserRoles.Remove(userRoles);
                    await _context.SaveChangesAsync();
                    userRoles.RoleId = newRoleId;
                    _context.Add(userRoles);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRolesExists(userRoles.UserId))
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
            fillSelectLists(userRoles);
            return View(userRoles);
        }

        // GET: UserRoles/Delete/5
        public async Task<IActionResult> Delete(int? id, int? roleId)
        {
            if (id == null || roleId == null)
            {
                return NotFound();
            }

            var userRoles = await _context.UserRoles
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userRoles == null)
            {
                return NotFound();
            }

            return View(userRoles);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int roleId)
        {
            var userRoles = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == id && x.RoleId == roleId);
            _context.UserRoles.Remove(userRoles);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRolesExists(int id)
        {
            return _context.UserRoles.Any(e => e.UserId == id);
        }

        private void fillSelectLists()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
        }

        private void fillSelectLists(UserRoles userRoles)
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Title", userRoles.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", userRoles.UserId);
        }
    }
}
