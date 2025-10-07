using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Saa3idWeb.Data;
using Saa3idWeb.Models;

namespace Saa3idWeb.Controllers
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

		[HttpGet("api/hotline/")]
		public async Task<IActionResult> IndexAPI()
		{
			return Json(new
			{
				status = "OK",
				hotlines = await _context.Hotline.ToListAsync(),
			});
		}

        // GET: Hotlines/Details/5
        public async Task<IActionResult> Details(int? id)
        {

			return await this.OnGetDetails(id, (hotlineId, hotline) =>
			{
				return View(hotline);
			});
        }

		[HttpGet("api/hotline/{id}")]
		public async Task<IActionResult> DetailsAPI(int? id)
		{
			return await this.OnGetDetails(id, (hotlineId, hotline) =>
			{
				return Json(new
				{
					status = "OK",
					hotline = hotline,
				});
			});
		}

		public async Task<IActionResult> OnGetDetails(int? id, Func<int?, Hotline, IActionResult> successCallback)
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

			return successCallback(id, hotline);
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
			return await this.OnCreate(hotline, (data) => {
				return RedirectToAction(nameof(Index));
			}, (data) => {
				return View(data);
			});
        }

		[HttpPost("api/hotline/create")]
		public async Task<IActionResult> CreateAPI([Bind("Id,Type,Number,Neighborhood,City,Province")] Hotline hotline)
		{
			return await this.OnCreate(hotline, (data) => {
				return Json(new
				{
					status = "success",
					hotline = data,
					redirect = "hotline/view",
				});
			}, (data) => {
				return View(data);
			});
		}

		protected async Task<IActionResult> OnCreate(Hotline hotline, Func<Hotline, IActionResult> modelValidCallback,
			Func<Hotline, IActionResult> modelInvalidCallback)
		{
			if (ModelState.IsValid)
			{
				_context.Add(hotline);
				await _context.SaveChangesAsync();
				return modelValidCallback(hotline);
			}

			return modelInvalidCallback(hotline);
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

        // PUT: Hotlines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Number,Neighborhood,City,Province")] Hotline hotline)
        {
			return await this.OnEdit(id, hotline, (hotlineID, hotline) => {
				return RedirectToAction(nameof(Index));
			}, (hotlineId, hotline) => {
				return View(hotline);
			});
        }

		[HttpPut("api/hotline/update")]
		public async Task<IActionResult> EditAPI(int id, [Bind("Id,Type,Number,Neighborhood,City,Province")] Hotline hotline)
		{
			return await this.OnEdit(id, hotline, (hotlineID, hotline) => {
				return Json(new
				{
					status = "OK",
					redirect = "hotlines",
					hotline = hotline,
				});
			}, (hotlineId, hotline) => {
				return Json(new
				{
					status = "error",
				});
			});
		}

		public async Task<IActionResult> OnEdit(int id, Hotline hotline,
			Func<int, Hotline, IActionResult> modelValidCallback,
			Func<int, Hotline, IActionResult> modelInvalidCallback)
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
				return modelValidCallback(id, hotline);
			}

			return modelInvalidCallback(id, hotline);
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

        // DELETE: Hotlines/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			return await this.OnDeleteConfirmed(id, (hotlineId) =>
			{
				return RedirectToAction(nameof(Index));
			});
		}

		[HttpDelete("api/hotline/delete")]
		public async Task<IActionResult> DeleteConfirmedAPI(int id)
		{
			return await this.OnDeleteConfirmed(id, (hotlineId) =>
			{
				return Json(new
				{
					status = "OK",
					redirect = "hotlines",
				});
			});
		}

		protected async Task<IActionResult> OnDeleteConfirmed(int id, Func<int, IActionResult> successCallback)
		{
			var hotline = await _context.Hotline.FindAsync(id);
			if (hotline != null)
			{
				_context.Hotline.Remove(hotline);
			}

			await _context.SaveChangesAsync();
			return successCallback(id);
		}


		private bool HotlineExists(int id)
        {
            return _context.Hotline.Any(e => e.Id == id);
        }
    }
}
