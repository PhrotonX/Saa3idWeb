namespace Saa3idWeb.Models
{
	public class Location
	{
		public int Id { get; set; }
		public required string Title { get; set; }
		public string? Description { get; set; } = null;
		public required DateTime CreatedAt { get; set; } = DateTime.Now;
		public required DateTime UpdatedAt { get; set; } = DateTime.Now;
		public required decimal Latitude { get; set; }
		public required decimal Longitude { get; set; }

		/// <summary>
		/// Location type one of the following: aid, evacuation_center, hospital, refugee_camp, and among others.
		/// </summary>
		public required string LocationType { get; set; }
	}
}
