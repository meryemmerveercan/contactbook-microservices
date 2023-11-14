
using System.ComponentModel;

namespace ReportService.Enums
{
	public enum ReportStatus
	{
		[Description("Preparing")]
		Preparing = 1,
		[Description("Completed")]
		Completed = 2,
	}
}
