using ReportService.Data;
using ReportService.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportService.Enums;
using ReportService.Models;

namespace ReportService.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReportsController : ControllerBase
	{
		private readonly ReportDbContext _context;
		private readonly ISendEndpointProvider _sendEndpointProvider;

		public ReportsController(ReportDbContext context, ISendEndpointProvider sendEndpointProvider)
		{
			_context = context;
			_sendEndpointProvider = sendEndpointProvider;
		}

		public ReportsController(ReportDbContext context)
		{
			_context = context;
		}

		[HttpPost("create-report")]
		public async Task<IActionResult> CreateReport([FromBody] string location)
		{
			var report = new Report()
			{
				Id = Guid.NewGuid(),
				ReportDate = DateTime.Now,
				Status = ReportStatus.Preparing.ToString()
			};

			_context.Reports.Add(report);
			_context.SaveChanges();

			var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:report-queue"));
			await endpoint.Send<ReportRequestModel>(new
			{
				Location = location,
				Id = report.Id
			});

			return Ok("Report request sent successfully");
		}

		[HttpGet("report-list")]
		public IEnumerable<Report> ReportList()
		{
			return _context.Reports.Include(x => x.ReportDetails).ToList();
		}

		[HttpGet("{id}")]
		public Report Get(Guid id)
		{
			return _context.Reports.Where(x => x.Id == id).Include(x => x.ReportDetails).FirstOrDefault()!;
		}

		[HttpGet("reportstatus-list")]
		public ActionResult<IEnumerable<string>> GetEnumValues()
		{
			var enumValues = Enum.GetNames(typeof(ReportStatus));
			return Ok(enumValues);
		}
	}
}
