using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saa3idWeb.Data;
using Saa3idWeb.Models;

namespace Saa3idWeb.Controllers.Auth
{
	[ApiController]
	public class RegisterController : Controller
	{
		private readonly ApplicationDbContext _context;

		public RegisterController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpPost("api/register")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register([Bind("FirstName,MiddleName,LastName,ExtName,Email,Password,HomeAddress,Neighborhood,City,Province,Country")] User user)
		{
			if (ModelState.IsValid)
			{
				//@TODO: Implement Password Hashing Algorithm
				//@TODO: Implement Validation
				//@TODO: Implement Session Handling
				
				_context.Add(user);
				await _context.SaveChangesAsync();
				return Json(new
				{
					status = "OK",
					redirect = "home",
					user = user
				});
			}
			return Json(new
			{
				status = "Error",
				redirect = "register",
				user = user
			});
		}
	}
}
