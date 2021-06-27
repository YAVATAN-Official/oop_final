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
    public class UserTagsController : Controller
    {
        private readonly YasharDbContext _context;

        public UserTagsController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: UserTags
        public async Task<IActionResult> Index()
        {
            var yasharDbContext = _context.UserTags.Include(u => u.Tag).Include(u => u.User);
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: UserTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTags = await _context.UserTags
                .Include(u => u.Tag)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userTags == null)
            {
                return NotFound();
            }

            return View(userTags);
        }

        // GET: UserTags/Create
        public IActionResult Create()
        {
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Title");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,TagId")] UserTags userTags)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userTags);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Title", userTags.TagId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userTags.UserId);
            return View(userTags);
        }

        // GET: UserTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTags = await _context.UserTags.FindAsync(id);
            if (userTags == null)
            {
                return NotFound();
            }
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Title", userTags.TagId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userTags.UserId);
            return View(userTags);
        }

        // POST: UserTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,TagId")] UserTags userTags)
        {
            if (id != userTags.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTags);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTagsExists(userTags.UserId))
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
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Title", userTags.TagId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userTags.UserId);
            return View(userTags);
        }

        // GET: UserTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTags = await _context.UserTags
                .Include(u => u.Tag)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userTags == null)
            {
                return NotFound();
            }

            return View(userTags);
        }

        // POST: UserTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userTags = await _context.UserTags.FindAsync(id);
            _context.UserTags.Remove(userTags);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTagsExists(int id)
        {
            return _context.UserTags.Any(e => e.UserId == id);
        }
    }
}
