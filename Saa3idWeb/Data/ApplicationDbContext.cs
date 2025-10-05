using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Saa3idWeb.Models;

namespace Saa3idWeb.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		protected ApplicationDbContext()
		{
		}

	    public DbSet<Saa3idWeb.Models.Sample> Sample { get; set; } = default!;
		public DbSet<Saa3idWeb.Models.User> User { get; set; } = default!;
		public DbSet<Saa3idWeb.Models.Location> Location { get; set; } = default!;
		public DbSet<Saa3idWeb.Models.Emergency> Emergency { get; set; } = default!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(e => e.Id);
				entity.Property(e => e.FirstName).IsRequired();
				entity.Property(e => e.MiddleName);
				entity.Property(e => e.LastName).IsRequired();
				entity.Property(e => e.ExtName);
				entity.Property(e => e.Phone).IsRequired();
				entity.Property(e => e.HomeAddress).IsRequired();
				entity.Property(e => e.Password).IsRequired();
				entity.Property(e => e.Neighborhood).IsRequired();
				entity.Property(e => e.City).IsRequired();
				entity.Property(e => e.Province).IsRequired();
				entity.Property(e => e.Country).IsRequired();
			});
		}
	    
		protected void OnSeed(ModelBuilder builder)
		{
			//builder.Entity<Saa3idWeb.Models.Location>().HasData(
			//	new Location
			//	{
			//		Id = 1,
			//		Title = "",
			//		Description = "This is a sample location",
			//		Latitude = ""
			//	}	
			//);
		}
	    
	}
}
