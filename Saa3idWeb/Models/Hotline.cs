namespace Saa3idWeb.Models
{
	public class Hotline
	{
		public required int Id { get; set; }
		public required string Type { get; set; }
		public required string Number { get; set; }
		public string? Neighborhood { get; set; } = null;
		public required string City { get; set; }
		public required string Province { get; set; }
	}
}
