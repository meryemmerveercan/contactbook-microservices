using ContactService.Entities;
using ContactService.Enums;
using Microsoft.EntityFrameworkCore;

namespace ContactService.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<ContactInformation> ContactInformations { get; set; }
		public DbSet<Contact> Contacts { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseInMemoryDatabase("contactdb");
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
				.Entity<ContactInformation>()
				.Property(e => e.InformationType)
				.HasConversion(
					v => v.ToString(),
					v => (ContactInformationType)Enum.Parse(typeof(ContactInformationType), v));
		}
	}
}

