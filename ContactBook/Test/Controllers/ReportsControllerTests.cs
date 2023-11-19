using ContactService.Controllers;
using ContactService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportService.Controllers;
using ReportService.Data;
using ReportService.Entities;
using ReportService.Enums;
using Xunit;

namespace Test.Controllers
{
	public class ReportsControllerTests
	{
		ReportDbContext dbContext;
		ReportsController reportController;
		Guid reportId = Guid.NewGuid();

		public ReportsControllerTests()
		{
			var optionsBuilder = new DbContextOptionsBuilder<ReportDbContext>()
				 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

			dbContext = new ReportDbContext(optionsBuilder.Options);
			dbContext.Reports.Add(new Report
			{
				Id = reportId,
				ReportDate = DateTime.Now,
				Status = ReportStatus.Preparing.ToString(),
			});
			dbContext.SaveChanges();

			reportController = new ReportsController(dbContext);
		}

		[Fact]
		public void getlist_result_test()
		{
			var result = reportController.ReportList();

			Assert.NotNull(result);
			Assert.IsAssignableFrom<IEnumerable<Report>>(result);
		}

	}
}
