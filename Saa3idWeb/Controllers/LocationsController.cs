using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Saa3idWeb.Data;
using Saa3idWeb.Models;
using Saa3idWeb.Util;

namespace Saa3idWeb.Controllers
{
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {
            return View(await _context.Location.ToListAsync());
        }

		[HttpGet("api/location")]
		public async Task<IActionResult> IndexAPI()
		{
			return Json(new
			{
				locations = await _context.Location.ToListAsync<Location>(),
			});
		}

		// GET: Locations/Details/5
		public async Task<IActionResult> Details(int? id)
        {
			return await this.OnGetDetails(id, (location) =>
			{
				return View(location);
			});
		}

		[HttpGet("api/location/{id}")]
		public async Task<IActionResult> DetailsAPI(int? id)
		{
			return await this.OnGetDetails(id, (location) =>
			{
				return Json(new
				{
					status = "OK",
					redirect = "location",
					location = location,
				});
			});
		}

		protected async Task<IActionResult> OnGetDetails(int? id, Func<Location, IActionResult> successCallback)
		{
			if (id == null)
			{
				return NotFound();
			}

			var location = await _context.Location
				.FirstOrDefaultAsync(m => m.Id == id);
			if (location == null)
			{
				return NotFound();
			}

			return View(location);
		}

		[HttpGet("api/location/search")]
		public async Task<IActionResult> SearchApi(String? title, String? description, float? latitude, float? longitude)
		{
			return Json(new
			{
				results = await this.OnSearch(title, description, latitude, longitude),
			});
		}

		protected async Task<List<Location>> OnSearch(String? title = null, String? description = null, float? lat = null, float? lng = null)
		{
			SelectQueryBuilder selectQueryBuilder = new SelectQueryBuilder();

			selectQueryBuilder.AddField("*");

			if (title != null && title != "" && title != " ")
			{
				selectQueryBuilder.AddCondition($"Title LIKE '%{title}%'", "AND");
			}
			if(description != null && description != "" && description != " ")
			{
				selectQueryBuilder.AddCondition($"Description LIKE '%{description}%'", "AND");
			}
			if(lat != null)
			{
				selectQueryBuilder.AddCondition($"Latitude LIKE {lat}", "AND");
			}
			if(lng != null)
			{
				selectQueryBuilder.AddCondition($"Longitude LIKE {lng}");
			}

			selectQueryBuilder.SetTable("location");

			String query = selectQueryBuilder.Build();

			return await _context.Location.FromSqlRaw<Location>(query).ToListAsync();
		}

        // GET: Locations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Latitude,Longitude,LocationType")] Location location)
        {
			return await this.OnCreate(location, (data) => {
				return RedirectToAction(nameof(Index));
			}, (data) => {
				return View(location);
			});
        }

		[HttpPost("api/location/create")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateAPI([Bind("Title,Description,Latitude,Longitude,LocationType")] Location location)
		{
			return await this.OnCreate(location, (data) => {
				return Json(new
				{
					location = location,
					redirect = "location",
					status = "OK",
				});
			}, (data) => {
				return Json(new
				{
					location = location,
					status = "Error",
					redirect = "location/create",
				});
			});
		}

		[HttpPost("api/location/create")]
		protected async Task<IActionResult> OnCreate(Location location,
			Func<Location, IActionResult> redirectCallback,
			Func<Location, IActionResult> failureCallback)
		{
			if (ModelState.IsValid)
			{
				_context.Add(location);
				await _context.SaveChangesAsync();
				return redirectCallback(location);
			}
			return failureCallback(location);
		}

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        // PUT: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,Latitude,Longitude,LocationType")] Location location)
        {
			return await this.OnEdit(id, location, (dataId, data) =>
			{
				return RedirectToAction(nameof(Index));
			}, (dataId, data) =>
			{
				return View(location);
			});
        }

		[HttpPut("api/location/edit/{id}")]
		public async Task<IActionResult> EditAPI(int id, Location location)
		{
			return await this.OnEdit(id, location, (dataId, data) =>
			{
				return Json(new
				{
					location = data,
					status = "OK",
					redirect = "location",
				});
			}, (dataId, data) =>
			{
				return Json(new
				{
					location = data,
					status = "Error",
					redirect = "location/edit",
				});
			});
		}

		protected async Task<IActionResult> OnEdit(int id, Location location,
			Func<int, Location, IActionResult> successCallback,
			Func<int, Location, IActionResult> failedCallback)
		{
			if (id != location.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(location);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!LocationExists(location.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return successCallback(id, location);
			}
			// Go back to edit form if failed.
			return failedCallback(id, location);
		}

		// GET: Locations/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.Id == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // DELETE: Locations/Delete/5
        [HttpDelete, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			return await this.OnDeleteConfirmed(id, () =>
			{
				return RedirectToAction(nameof(Index));
			});
		}

		[HttpDelete("api/location/delete/{id}")]
		public async Task<IActionResult> DeleteConfirmedAPI(int id)
		{
			return await this.OnDeleteConfirmed(id, () =>
			{
				return Json(new
				{
					status = "OK",
					redirect = "home",
				});
			});
		}

		protected async Task<IActionResult> OnDeleteConfirmed(int id,
			Func<IActionResult> redirectCallback)
		{
			var location = await _context.Location.FindAsync(id);
			if (location != null)
			{
				_context.Location.Remove(location);
			}

			await _context.SaveChangesAsync();
			return redirectCallback();
		}

        private bool LocationExists(int id)
        {
            return _context.Location.Any(e => e.Id == id);
        }
    }
}
