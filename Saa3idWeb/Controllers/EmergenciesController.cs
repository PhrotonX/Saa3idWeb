using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Saa3idWeb.Data;
using Saa3idWeb.Models;
using Saa3idWeb.Util;

namespace Saa3idWeb.Controllers
{
	//[ApiController]
    public class EmergenciesController : Controller
    {
        private readonly ApplicationDbContext _context;
		private EmergencyNotifier notifier;

        public EmergenciesController(ApplicationDbContext context, EmergencyNotifier notifier)
        {
            _context = context;
			this.notifier = notifier;
        }

		[AllowAnonymous]
        // GET: Emergencies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Emergency.ToListAsync());
        }

		[AllowAnonymous]
		[HttpGet("api/emergency"), ActionName("API.Emergency.Index")]
		public async Task<IActionResult> IndexAPI()
		{
			return Json(new
			{
				emergencies = await _context.Emergency.ToListAsync()
			});
		}

		[AllowAnonymous]
        // GET: Emergencies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
			return await this.OnGetDetails(id, (emergencyId, emergency) =>
			{
				return View(emergency);
			});
        }

		[AllowAnonymous]
		[HttpGet("api/emergency/{id}")]
		public async Task<IActionResult> DetailsAPI(int? id)
		{
			return await this.OnGetDetails(id, (emergencyId, emergency) =>
			{
				return Json(new
				{
					status = "OK",
					emergency = emergency,
				});
			});
		}

		protected async Task<IActionResult> OnGetDetails(int? id, Func<int?, Emergency, IActionResult> successCallback)
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

			return successCallback(id, emergency);
		}

		[AllowAnonymous]
		[HttpGet("api/emergency/search")]
		public async Task<IActionResult> SearchAPI(float? lng, float? lat, int? userId)
		{
			return Json(new
			{
				results = await this.OnSearch(lat, lng, userId)
			});
		}

		protected async Task<List<Emergency>> OnSearch(float? lng, float? lat, int? userId)
		{
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder();

			selectQueryBuilder.AddField("*");

			if (lat != null)
			{
				selectQueryBuilder.AddCondition($"Latitude LIKE {lat}", "AND");
			}
			if (lng != null)
			{
				selectQueryBuilder.AddCondition($"Longitude LIKE {lng}", "AND");
			}
			if(userId != null)
			{
				selectQueryBuilder.AddCondition($"UserId = {userId}", "AND");
			}

			selectQueryBuilder.SetTable("emergency");

			String query = selectQueryBuilder.Build();

			return await _context.Emergency.FromSqlRaw<Emergency>(query).ToListAsync();
		}

		[AllowAnonymous]
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
		[AllowAnonymous]
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
		[AllowAnonymous]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateAPI([Bind("Id,Latitude,Longitude,UserId")] Emergency emergency)
		{
			if (ModelState.IsValid)
			{
				_context.Add(emergency);
				await _context.SaveChangesAsync();

				await this.notifier.Notify(emergency);

				return Json(new
				{
					status = "OK",
					emergency = emergency,
					success = true
				});
			}

			return Json(new
			{
				status = "Error",
				emergency = emergency,
				success = false
			});
		}

		[AllowAnonymous]
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
		[AllowAnonymous]
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

		[HttpPut("api/emergency/edit"), ]
		[AllowAnonymous]
		public async Task<IActionResult> EditApi(int id,
			[Bind("Id,Latitude,Longitude,CreatedAt,UpdatedAt,UserId")] Emergency emergency)
		{
			return await this.OnEdit(id, emergency, (data) => {
				return Json(new
				{
					status = "OK",
				});
			}, (data) => {
				return Json(new
				{
					status = "error",
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

		[Authorize]
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

        // DELETE: Emergencies/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmed(int id)
        {
			await this.OnDeleteConfirmed(id);

			return RedirectToAction(nameof(Index));
        }

		[HttpDelete("api/emergency/delete"), ActionName("DeleteConfirmedApi")]
		//[ValidateAntiForgeryToken]
		[Authorize]
		public async Task<IActionResult> DeleteConfirmedApi(int id)
		{
			await this.OnDeleteConfirmed(id);

			return Json(new
			{
				status = "OK",
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
