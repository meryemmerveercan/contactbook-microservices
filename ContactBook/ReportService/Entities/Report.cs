using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ReportService.Entities
{
    public class Report
	{
		[Key]
        public Guid Id { get; set; }
		public DateTime ReportDate  { get; set; }
		public string Status { get; set; }
		public ICollection<ReportDetail> ReportDetails { get; set; }
	}
}
