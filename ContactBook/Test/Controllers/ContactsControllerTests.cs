using ContactService.Controllers;
using ContactService.Data;
using ContactService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Test.Controllers
{
	public class ContactsControllerTests
	{
		ApplicationDbContext dbContext;
		ContactsController contactController;
		Guid contactId = Guid.NewGuid();
		Guid contactInfoId = Guid.NewGuid();

		public ContactsControllerTests()
		{
			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
				 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

			dbContext = new ApplicationDbContext(optionsBuilder.Options);
			dbContext.Contacts.Add(new Contact
			{
				Id = contactId,
				Name = "John",
				Surname = "Black",
				Company = "A Company"
			});
			dbContext.SaveChanges();

			contactController = new ContactsController(dbContext);
		}

		[Fact]
		public void getlist_result_test()
		{
			var result = contactController.Get();

			Assert.NotNull(result);
			Assert.IsAssignableFrom<IEnumerable<Contact>>(result);
		}

		[Fact]
		public void getcontact_resultnotfound_test()
		{
			var result = contactController.Get(Guid.NewGuid());

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void getcontact_resultfound_test()
		{
			var result = contactController.Get(contactId);

			Assert.IsType<OkObjectResult>(result);
		}

		[Fact]
		public void postcontact_result_test()
		{
			var contact = new Contact()
			{
				Id = Guid.NewGuid(),
				Name = "John",
				Surname = "Black",
				Company = "A Company"
			};

			var result = contactController.Post(contact);

			Assert.IsType<ObjectResult>(result);
		}

		[Fact]
		public void deletecontact_notfound_test()
		{
			var result = contactController.Delete(Guid.NewGuid());

			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void deletecontact_test()
		{
			var result = contactController.Delete(contactId);

			Assert.IsType<NoContentResult>(result);
		}

		[Fact]
		public void addinformation_result_test()
		{
			var contactInfo = new ContactInformation()
			{
				Id = contactInfoId,
				ContactId = contactId,
				InformationType = ContactService.Enums.ContactInformationType.Phone,
				Content = "4443322"
			};

			var result = contactController.AddInformation(contactInfo);

			Assert.IsType<OkObjectResult>(result);
		}

		[Fact]
		public void deletecontactinfo_notfound_test()
		{
			var result = contactController.Delete(contactInfoId);

			Assert.IsType<NotFoundResult>(result);
		}
	}
}
