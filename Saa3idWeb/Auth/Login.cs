using System.ComponentModel.DataAnnotations;

namespace Saa3idWeb.Auth
{
	public class Login
	{
		public required string Username { get; set; }
		public required string Password { get; set; }
	}
}
