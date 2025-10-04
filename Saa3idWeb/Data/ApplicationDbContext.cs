using Microsoft.EntityFrameworkCore;
using Saa3idWeb.Models;

namespace Saa3idWeb.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		protected ApplicationDbContext()
		{
		}
	    public DbSet<Saa3idWeb.Models.Sample> Sample { get; set; } = default!;
	}
}
