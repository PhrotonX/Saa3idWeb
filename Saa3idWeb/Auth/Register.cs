using System.ComponentModel.DataAnnotations;

namespace Saa3idWeb.Auth
{
	public class Register
	{
		[Required(ErrorMessage = "Username is required"), StringLength(255, ErrorMessage = "Username has a minimum of 3 and a maximum of 255 characters.", MinimumLength = 3)]
		public required string UserName { get; set; }
		[Required(ErrorMessage = "First name is required."), StringLength(255, ErrorMessage = "First name has a minimum of 3 and a maximum of 255 characters.", MinimumLength = 3)]
		public required string FirstName { get; set; }
		[StringLength(255, ErrorMessage = "Middle name has a maximum of 255 characters.")]
		public string? MiddleName { get; set; } = null;
		[Required(ErrorMessage = "Last name is required."), StringLength(255, ErrorMessage = "Last name has a minimum of 3 and a maximum of 255 characters.", MinimumLength = 3)]
		public required string LastName { get; set; }
		[StringLength(255, ErrorMessage = "Ext name has a maximum of 255 characters.")]
		public string? ExtName { get; set; } = null;
		[AllowedValues(['F', 'M']), Required(ErrorMessage = "Gender is required")]
		public char? Gender { get; set; } = null;

		[Required(ErrorMessage = "Password is required."), StringLength(255, ErrorMessage = "Password has a minimum of 8 and a maximum of 255 characters.", MinimumLength = 8)]
		public required string Password { get; set; }

		[Phone(ErrorMessage = "Invalid Phone Number")]
		public string? PhoneNumber { get; set; } = "";

		[Required(ErrorMessage = "Email is required."), EmailAddress(ErrorMessage = "Must be a proper type of email"), StringLength(255, ErrorMessage = "First name has a minimum of 3 and a maximum of 255 characters.", MinimumLength = 3)]
		public required string Email { get; set; }
		[Required(ErrorMessage = "House/Street/Subdivision is required."), StringLength(255, ErrorMessage = "House/Street/Subdivision has a minimum of 3 and a maximum of 255 characters.", MinimumLength = 3)]
		public required string HomeAddress { get; set; } //House, Street, Subdivision, etc..

		[Required(ErrorMessage = "Neighborhood is required."), StringLength(255, ErrorMessage = "Neighborhood has a minimum of 3 and a maximum of 255 characters.", MinimumLength = 3)]
		public required string Neighborhood { get; set; }
		[Required(ErrorMessage = "City is required."), StringLength(255, ErrorMessage = "City has a minimum of 3 and a maximum of 255 characters.", MinimumLength = 3)]
		public required string City { get; set; }
		[Required(ErrorMessage = "Province is required."), StringLength(255, ErrorMessage = "Province has a minimum of 3 and a maximum of 255 characters.", MinimumLength = 3)]
		public required string Province { get; set; } = "Pampanga";
		[Required(ErrorMessage = "Country is required."), StringLength(255, ErrorMessage = "Country has a minimum of 3 and a maximum of 255 characters.", MinimumLength = 3)]
		public required string Country { get; set; } = "Philippines";
		[Required(ErrorMessage = "User member is required."), StringLength(255, ErrorMessage = "House/Street/Subdivision has a maximum of 255 characters.")]
		public required string UserType { get; set; } = "member";
	}
}
