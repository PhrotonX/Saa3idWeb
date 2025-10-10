using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

		[HttpPost("register")]
		//[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register([Bind("FirstName,MiddleName,LastName,ExtName,Email,Password,HomeAddress,Neighborhood,City,Province,Country")] User user)
		{
			if (ModelState.IsValid)
			{
				//@TODO: Implement Password Hashing Algorithm
				//@TODO: Implement Validation
				//@TODO: Implement Session Handling
				
				context.Add(user);
				await context.SaveChangesAsync();
				return Json(new
				{
					status = "OK",
					redirect = "home",
					user
				});
			}
			return Json(new
			{
				status = "Error",
				redirect = "register",
				user
			});
		}
	}
}
