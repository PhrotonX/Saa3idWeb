namespace Saa3idWeb.Models
{
	public class Emergency
	{
		public required int Id { get; set; }
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime UpdatedAt { get; set; } = DateTime.Now;
		public required int UserId { get; set; }
	}
}
