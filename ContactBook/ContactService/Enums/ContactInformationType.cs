
using System.ComponentModel;

namespace ContactService.Enums
{
	public enum ContactInformationType
	{
		[Description("Phone")]
		Phone = 1,
		[Description("Email")]
		Email = 2,
		[Description("Location")]
		Location = 3,
	}
}
