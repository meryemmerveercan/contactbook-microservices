using ContactService.Data;
using ContactService.Entities;
using ContactService.Enums;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportService.Models;

namespace ContactService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ContactsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public ContactsController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public IEnumerable<Contact> Get()
		{
			return _context.Contacts.Include(x => x.ContactInformations).ToList();
		}

		[HttpGet("{id}")]
		public Contact Get(Guid id)
		{
			return _context.Contacts.Where(x => x.Id == id).Include(x => x.ContactInformations).FirstOrDefault()!;
		}

		[HttpPost]
		public IActionResult Post([FromBody] Contact contact)
		{
			try
			{
				_context.Contacts.Add(contact);
				_context.SaveChanges();

				return StatusCode(StatusCodes.Status201Created, contact);
			}
			catch (Exception ex)
			{
				return (IActionResult)ex;
			}
		}

		[HttpDelete("{id}")]
		public void Delete(Guid id)
		{
			var contact = _context.Contacts.Where(x => x.Id == id).FirstOrDefault()!;
			if (contact != null)
			{
				_context.Contacts.Remove(contact);
				_context.SaveChanges();
			}
		}

		[HttpPost("add-information")]
		public IActionResult AddInformation([FromBody] ContactInformation contactInformation)
		{
			var contact = _context.Contacts.Include(p => p.ContactInformations).FirstOrDefault(p => p.Id == contactInformation.ContactId);

			if (contact == null)
			{
				return NotFound("Contact not found");
			}

			_context.ContactInformations.Add(contactInformation);
			_context.SaveChanges();

			contact.ContactInformations.Add(contactInformation);
			_context.SaveChanges();

			return Ok("Contact Informations added to contact successfully");
		}

		[HttpDelete("delete-information/{id}")]
		public IActionResult DeleteInformation(Guid id)
		{
			var contactInfo = _context.ContactInformations.Where(x => x.Id == id).FirstOrDefault()!;
			if (contactInfo == null)
			{
				return NotFound("Contact Information not found");
			}

			_context.ContactInformations.Remove(contactInfo);
			_context.SaveChanges();

			return Ok("Contact Information deleted successfully");
		}

		[HttpGet("create-report/{location}")]
		public async Task<ReportModel> CreateReport(string location)
		{
			var contactList = _context.Contacts
								 .Include(p => p.ContactInformations)
								 .Where(p => p.ContactInformations.Any(c => c.InformationType == ContactInformationType.Location && c.Content == location))
								 .Select(x => x.Id).ToList();

			var phoneList = _context.ContactInformations.Where(x => contactList.Contains(x.ContactId) && x.InformationType == ContactInformationType.Phone)
								 .ToList();

			var reportResult = new ReportModel()
			{
				Location = location,
				ContactCount = contactList.Count(),
				PhoneCount = phoneList.Count()
			};

			return reportResult;
		}

		[HttpGet("contactinformationtype-list")]
		public ActionResult<IEnumerable<string>> GetEnumValues()
		{
			var enumValues = Enum.GetNames(typeof(ContactInformationType));
			return Ok(enumValues);
		}
	}
}
