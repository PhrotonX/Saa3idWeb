using Newtonsoft.Json;
using System.Text;

namespace Saa3idWeb.Util
{
	public class EmergencyNotifier
	{
		private static readonly HttpClient client = new HttpClient();
		public const String FIREBASE_DB_URL = "https://saa3id-a80b1-default-rtdb.asia-southeast1.firebasedatabase.app/";
		public const String JSON = "emergencies.json";
		private IConfiguration configuration;

		public EmergencyNotifier(IConfiguration configuration)
		{
			this.configuration = configuration;
		}
		
		public async Task Notify(Object payload)
		{
			var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

			await client.PostAsync(FIREBASE_DB_URL + JSON + "?auth=" + this.configuration["Firebase:token"], content);
		}
	}
}
