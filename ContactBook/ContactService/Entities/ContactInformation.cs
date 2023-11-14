using ContactService.Enums;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ContactService.Entities
{
	public class ContactInformation
	{
		[Key]
		public Guid Id { get; set; }

		[ForeignKey(nameof(Contact))]
		public Guid ContactId { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public ContactInformationType InformationType { get; set; }
		public string Content { get; set; }		
	}
}
