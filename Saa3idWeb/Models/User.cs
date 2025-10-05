namespace Saa3idWeb.Models
{
	public class User
	{
		public int Id { get; set; }
		public required string FirstName { get; set; }
		public string? MiddleName { get; set; } = null;
		public required string LastName { get; set; }
		public string? ExtName { get; set; } = null;
		public required string Phone { get; set; }
		public required string Password { get; set; }
		public required string HomeAddress { get; set; } //House, Street, Subdivision, etc..
		public required string Neighborhood { get; set; }
		public required string City { get; set; }
		public required string Province { get; set; } = "Pampanga";
		public required string Country { get; set; } = "Philippines";
	}
}
