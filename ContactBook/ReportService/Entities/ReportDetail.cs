using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportService.Entities
{
	public class ReportDetail
	{
		[Key]
		public Guid Id { get; set; }

		[ForeignKey(nameof(Report))]
		public Guid ReportId { get; set; }
		public string Location { get; set; }
		public int ContactCount { get; set; }
		public int PhoneCount { get; set; }
	}
}
