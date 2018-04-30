using Newtonsoft.Json;
using System.Net.Http;

namespace VisualDPMB
{
	class JsonDownload<T>
	{
		private string url;
		public JsonDownload(string url)
		{
			this.url=url;
		}
		public T Download()
		{
			var client = new HttpClient();
			var str = client.GetStringAsync(url).Result;
			return JsonConvert.DeserializeObject<T>(str);
		}
	}
}
