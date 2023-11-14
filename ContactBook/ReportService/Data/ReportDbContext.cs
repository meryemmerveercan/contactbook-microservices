using ReportService.Entities;
using Microsoft.EntityFrameworkCore;

namespace ReportService.Data
{
	public class ReportDbContext : DbContext
	{
		public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options)
		{

		}

		public DbSet<ReportDetail> ReportDetails { get; set; }
		public DbSet<Report> Reports { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseInMemoryDatabase("reportdb");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{

		}
	}
}

