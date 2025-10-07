using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Saa3idWeb.Data;
using Saa3idWeb.Models;

namespace Saa3idWeb
{
    public class HotlinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HotlinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hotlines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hotline.ToListAsync());
        }

        // GET: Hotlines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotline = await _context.Hotline
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotline == null)
            {
                return NotFound();
            }

            return View(hotline);
        }

        // GET: Hotlines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotlines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Number,Neighborhood,City,Province")] Hotline hotline)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hotline);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hotline);
        }

        // GET: Hotlines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotline = await _context.Hotline.FindAsync(id);
            if (hotline == null)
            {
                return NotFound();
            }
            return View(hotline);
        }

        // POST: Hotlines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Number,Neighborhood,City,Province")] Hotline hotline)
        {
            if (id != hotline.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HotlineExists(hotline.Id))
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
            return View(hotline);
        }

        // GET: Hotlines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hotline = await _context.Hotline
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hotline == null)
            {
                return NotFound();
            }

            return View(hotline);
        }

        // POST: Hotlines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hotline = await _context.Hotline.FindAsync(id);
            if (hotline != null)
            {
                _context.Hotline.Remove(hotline);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HotlineExists(int id)
        {
            return _context.Hotline.Any(e => e.Id == id);
        }
    }
}
