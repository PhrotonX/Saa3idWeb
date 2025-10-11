using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.IdentityModel.Tokens;
using Saa3idWeb.Auth;
using Saa3idWeb.Data;
using Saa3idWeb.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Saa3idWeb.Controllers
{
	[ApiController]
	public class AuthenticationController : Controller
	{
		private readonly UserManager<User> userManager;
		private readonly IConfiguration configuration;
		private readonly ApplicationDbContext context;

		public AuthenticationController(ApplicationDbContext context, UserManager<User> userManager, IConfiguration configuration)
		{
			this.userManager = userManager;
			this.context = context;
			this.configuration = configuration;
		}

		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody]Saa3idWeb.Auth.Login login)
		{
			//Check if user does not exist.
			var user = await this.userManager.FindByNameAsync(login.Username);
			if(user != null && await this.userManager.CheckPasswordAsync(user, login.Password))
			{
				var userRoles = await this.userManager.GetRolesAsync(user);

				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, user.UserName ?? ""),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};

				foreach(var userRole in userRoles)
				{
					authClaims.Add(new Claim(ClaimTypes.Role, userRole));
				}

				var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JWT:Secret"]));

				var token = new JwtSecurityToken(
					issuer: this.configuration["JWT:ValidIssuer"],
					audience: this.configuration["JWT:ValidAudience"],
					expires: DateTime.Now.AddHours(3),
					claims: authClaims,
					signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

				return Ok(new
				{
					token = new JwtSecurityTokenHandler().WriteToken(token),
					expiration = token.ValidTo
				});
			}

			return Unauthorized();
		}

		[HttpPost]
		[Route("register")]
		//[ValidateAntiForgeryToken]
		[AllowAnonymous]
		public async Task<IActionResult> Register([Bind("UserName,FirstName,MiddleName,LastName,ExtName,Email,Gender,HomeAddress,Neighborhood,City")] Saa3idWeb.Auth.Register model)
		{
			//if (ModelState.IsValid)
			//{
				var userExists = this.userManager.FindByNameAsync(model.UserName).Result;

				if(userExists != null)
				{
					return StatusCode(500, new
					{
						status = "Error",
						redirect = "register",
						message = "User already exists",
						user = model,
					});
				}

			User newUser = new User()
				{
					UserName = model.UserName,
					FirstName = model.FirstName,
					MiddleName = model.MiddleName,
					LastName = model.LastName,
					ExtName = model.ExtName,
					Email = model.Email,
					Gender = model.Gender.First(),
					HomeAddress = model.HomeAddress,
					Neighborhood = model.Neighborhood,
					City = model.City,
					Province = "Pampanga",
					Country = "Philippines",
					UserType = "member",

					SecurityStamp = Guid.NewGuid().ToString(),
				};

				var result = await this.userManager.CreateAsync(newUser, model.Password);
				if (!result.Succeeded)
				{
					return StatusCode(500, new
					{
						result = result.Errors,
						status = "Error",
						redirect = "register",
						message = "Incorrect credentials",
						user = model,
					});
				}
				
				//context.Add(user);
				//await context.SaveChangesAsync();
			//	return Json(new
			//	{
			//		status = "OK",
			//		redirect = "home",
			//		user = model,
			//	});
			//}
			return Json(new
			{
				status = "OK",
				redirect = "home",
				user = model,
			});
		}

		//[HttpPost]
		//[AllowAnonymous]
		//public async Task<IActionResult> Logout()
		//{
			
		//}
	}
}
