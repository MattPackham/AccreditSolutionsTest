using Service.Abstract;
using System.Net;
using System.Net.Http;

namespace Service.Concrete
{
    public class WebClientFacade : IWebClient
    {
        public string DownloadString(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");

                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    response.EnsureSuccessStatusCode();
                    var responseBody = response.Content.ReadAsStringAsync().Result;

                    return responseBody;
                }
            }
        }
    }
}
