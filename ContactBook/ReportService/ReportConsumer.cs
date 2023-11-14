using MassTransit;
using Microsoft.EntityFrameworkCore;
using ReportService.Clients;
using ReportService.Data;
using ReportService.Entities;
using ReportService.Enums;
using ReportService.Models;

namespace ReportService
{
	public class ReportConsumer : IConsumer<ReportRequestModel>
	{
		private readonly ReportDbContext _context;
		private readonly ISendEndpointProvider _sendEndpointProvider;
		private readonly ContactClient _contactClient;

		public ReportConsumer(ReportDbContext context, ISendEndpointProvider sendEndpointProvider, ContactClient contactClient)
		{
			_context = context;
			_sendEndpointProvider = sendEndpointProvider;
			_contactClient = contactClient;
		}
		public async Task Consume(ConsumeContext<ReportRequestModel> requestModel)
		{
			var reportResult = await _contactClient.GetReportAsync(requestModel.Message.Location);

			var report = _context.Reports.Where(x => x.Id == requestModel.Message.Id).Include(x => x.ReportDetails).FirstOrDefault()!;
			
			try
			{
				var reportDetail = new ReportDetail()
				{
					Id = Guid.NewGuid(),
					ReportId = requestModel.Message.Id,
					Location = reportResult.Location,
					PhoneCount = reportResult.PhoneCount,
					ContactCount = reportResult.ContactCount
				};

				report.Status = ReportStatus.Completed.ToString();

				_context.ReportDetails.Add(reportDetail);
				_context.SaveChanges();

				_context.Reports.Update(report);
				report.ReportDetails.Add(reportDetail);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
			}
		}
	}
}
