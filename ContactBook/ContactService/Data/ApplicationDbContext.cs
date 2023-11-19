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
		public ApplicationDbContext()
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

			modelBuilder.Entity<Contact>().HasData(
						 new Contact
						 {
							 Id = Guid.Parse("64734e2b-bc91-45dd-ab07-652e77c161e9"),
							 Name = "John",
							 Surname = "Black",
							 Company = "A Company"
						 });
		}

		public void EnsureSeed()
		{
			throw new NotImplementedException();
		}
	}
}

