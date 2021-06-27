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
    public class YearTermsController : Controller
    {
        private readonly YasharDbContext _context;

        public YearTermsController(YasharDbContext context)
        {
            _context = context;
        }

        // GET: YearTerms
        public async Task<IActionResult> Index()
        {
            return View(await _context.YearTerms.ToListAsync());
        }

        // GET: YearTerms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearTerm = await _context.YearTerms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yearTerm == null)
            {
                return NotFound();
            }

            return View(yearTerm);
        }

        // GET: YearTerms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: YearTerms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Year,TermId,Id")] YearTerm yearTerm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(yearTerm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(yearTerm);
        }

        // GET: YearTerms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearTerm = await _context.YearTerms.FindAsync(id);
            if (yearTerm == null)
            {
                return NotFound();
            }
            return View(yearTerm);
        }

        // POST: YearTerms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Year,TermId,Id")] YearTerm yearTerm)
        {
            if (id != yearTerm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yearTerm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YearTermExists(yearTerm.Id))
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
            return View(yearTerm);
        }

        // GET: YearTerms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yearTerm = await _context.YearTerms
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yearTerm == null)
            {
                return NotFound();
            }

            return View(yearTerm);
        }

        // POST: YearTerms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var yearTerm = await _context.YearTerms.FindAsync(id);
            _context.YearTerms.Remove(yearTerm);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YearTermExists(int id)
        {
            return _context.YearTerms.Any(e => e.Id == id);
        }
    }
}
