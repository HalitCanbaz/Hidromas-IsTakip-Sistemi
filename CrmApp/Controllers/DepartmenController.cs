using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CrmApp.Models;

namespace CrmApp.Controllers
{
    public class DepartmenController : Controller
    {
        private readonly CrmAppDbContext _context;

        public DepartmenController(CrmAppDbContext context)
        {
            _context = context;
        }

        // GET: Departmen
        public async Task<IActionResult> Index()
        {
              return _context.Departman != null ? 
                          View(await _context.Departman.ToListAsync()) :
                          Problem("Entity set 'CrmAppDbContext.Departman'  is null.");
        }

        // GET: Departmen/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Departman == null)
            {
                return NotFound();
            }

            var departman = await _context.Departman
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departman == null)
            {
                return NotFound();
            }

            return View(departman);
        }

        // GET: Departmen/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departmen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DepartmanName")] Departman departman)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departman);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(departman);
        }

        // GET: Departmen/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Departman == null)
            {
                return NotFound();
            }

            var departman = await _context.Departman.FindAsync(id);
            if (departman == null)
            {
                return NotFound();
            }
            return View(departman);
        }

        // POST: Departmen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DepartmanName")] Departman departman)
        {
            if (id != departman.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departman);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmanExists(departman.Id))
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
            return View(departman);
        }

        // GET: Departmen/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Departman == null)
            {
                return NotFound();
            }

            var departman = await _context.Departman
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departman == null)
            {
                return NotFound();
            }

            return View(departman);
        }

        // POST: Departmen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Departman == null)
            {
                return Problem("Entity set 'CrmAppDbContext.Departman'  is null.");
            }
            var departman = await _context.Departman.FindAsync(id);
            if (departman != null)
            {
                _context.Departman.Remove(departman);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmanExists(int id)
        {
          return (_context.Departman?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
