using ReportService.Models;

namespace ReportService.Clients
{
	public class ContactClient
	{
		private readonly HttpClient httpClient;
		public ContactClient(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public async Task<ReportResultModel> GetReportAsync(string location)
		{
			var items = await httpClient.GetFromJsonAsync<ReportResultModel>("/api/Contacts/create-report/" + location + "");
			return items;
		}
	}
}
