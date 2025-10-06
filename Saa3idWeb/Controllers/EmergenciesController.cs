using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Saa3idWeb.Data;
using Saa3idWeb.Models;

namespace Saa3idWeb.Controllers
{
    public class EmergenciesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmergenciesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Emergencies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Emergency.ToListAsync());
        }

		[HttpGet("api/emergency")]
		public async Task<IActionResult> IndexAPI()
		{
			return Json(new
			{
				emergencies = await _context.Emergency.ToListAsync()
			});
		}

        // GET: Emergencies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emergency = await _context.Emergency
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emergency == null)
            {
                return NotFound();
            }

            return View(emergency);
        }

        // GET: Emergencies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Emergencies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Latitude,Longitude,CreatedAt,UpdatedAt,UserId")] Emergency emergency)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emergency);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(emergency);
		}

		// POST: Emergencies/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost("api/emergency/create")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateAPI([Bind("Id,Latitude,Longitude,UserId")] Emergency emergency)
		{
			if (ModelState.IsValid)
			{
				_context.Add(emergency);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return Json(new
			{
				emergency = emergency,
				success = true
			});
		}

		// GET: Emergencies/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var emergency = await _context.Emergency.FindAsync(id);
            if (emergency == null)
            {
                return NotFound();
            }
            return View(emergency);
        }

        // POST: Emergencies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
			[Bind("Id,Latitude,Longitude,CreatedAt,UpdatedAt,UserId")] Emergency emergency)
        {
			return await this.OnEdit(id, emergency, (data) => {
				return RedirectToAction(nameof(Index));
			}, (data) => {
				return View(emergency);
			});
        }

		[HttpPut("api/emergencies/edit")]
		public async Task<IActionResult> EditAPI(int id,
			[Bind("Id,Latitude,Longitude,CreatedAt,UpdatedAt,UserId")] Emergency emergency)
		{
			return await this.OnEdit(id, emergency, (data) => {
				return Json(new
				{
					status = "Failed",
				});
			}, (data) => {
				return Json(new
				{
					status = "Success",
				});
			});
		}

		protected async Task<IActionResult> OnEdit(int id,
			Emergency emergency,
			Func<Emergency, IActionResult> modelStateValidCallback,
			Func<Emergency, IActionResult> successCallback)
		{
			if (id != emergency.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					// Update into the DB.
					_context.Update(emergency);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!EmergencyExists(emergency.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return modelStateValidCallback(emergency);
			}
			return successCallback(emergency);
		}

		// GET: Emergencies/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
			if (id == null)
			{
				return NotFound();
			}

			var emergency = await _context.Emergency
				.FirstOrDefaultAsync(m => m.Id == id);
			if (emergency == null)
			{
				return NotFound();
			}

			return View(emergency);
		}

        // POST: Emergencies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			await this.OnDeleteConfirmed(id);

			return RedirectToAction(nameof(Index));
        }

		[HttpPost("api/emergencies/delete")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmedApi(int id)
		{
			await this.OnDeleteConfirmed(id);

			return Json(new
			{
				status = "Success",
				redirect = "home"
			});
		}

		/// <summary>
		/// Event when resource deletion has been confirmed.
		/// </summary>
		/// <param name="id">The ID of the resource.</param>
		protected async Task<int> OnDeleteConfirmed(int id)
		{
			var emergency = await _context.Emergency.FindAsync(id);
			if (emergency != null)
			{
				_context.Emergency.Remove(emergency);
			}

			return await _context.SaveChangesAsync();
		}

		private bool EmergencyExists(int id)
        {
            return _context.Emergency.Any(e => e.Id == id);
        }
    }
}
