using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using gis_final.Models;
using Microsoft.AspNetCore.Routing;

namespace gis_final.Controllers
{
    public class AddressesController : Controller
    {
        private readonly YasharDbContext _context;

        public AddressesController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: Addresses
        public async Task<IActionResult> Index(int? id)
        {
            var yasharDbContext = _context.Addresses.Include(a => a.User).Where(x => x.UserId == id);
            ViewBag.userId = id;
            return View(await yasharDbContext.ToListAsync());
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = _context.Users.FirstOrDefault(x => x.Id == id);
            ViewData["UserEmail"] = user.Email;
            ViewData["UserId"] = user.Id;
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Title, string Country, string City, string State, string Line1,
            string Line2, int PostalCode, int UserId)
        {
            Address address = new Address
            {
                UserId = UserId,
                Title = Title,
                Country = Country,
                City = City,
                State = State,
                Line1 = Line1,
                Line2 = Line2,
                PostalCode = PostalCode
            };
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Addresses", new RouteValueDictionary(new { Id = address.UserId }));
            }
            User user = _context.Users.FirstOrDefault(x => x.Id == address.UserId);
            ViewData["UserEmail"] = user.Email;
            ViewData["UserId"] = user.Id;
            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == address.UserId);
            ViewData["UserEmail"] = user.Email;
            ViewData["UserId"] = user.Id;
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Country,City,State,Line1,Line2,PostalCode,UserId,Id")] Address address)
        {
            if (id != address.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Addresses", new RouteValueDictionary(new { Id = address.UserId }));
            }
            User user = _context.Users.FirstOrDefault(x => x.Id == address.UserId);
            ViewData["UserEmail"] = user.Email;
            ViewData["UserId"] = user.Id;
            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Addresses", new RouteValueDictionary(new { Id = address.UserId }));
        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
