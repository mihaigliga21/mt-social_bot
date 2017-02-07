using System;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Bot_Application.Model;
using Newtonsoft.Json.Linq;

namespace Bot_Application.Helpers
{
    public class BingSearchWeb
    {
        public async Task<string> MakeRequest(string phrase)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(String.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Properties.Resources.BingApiKey);

            // Request parameters
            queryString["q"] = phrase;
            queryString["count"] = "10";
            queryString["offset"] = "0";
            queryString["mkt"] = "en-us";
            queryString["safesearch"] = "Moderate";
            var uri = "https://api.cognitive.microsoft.com/bing/v5.0/search?" + queryString;

            var response = await client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var data = new JavaScriptSerializer().Deserialize<BingSearchModel>(json);

            var response2Return = string.Empty;
            if (data != null)
            {
                response2Return = "Well! Check this - " + data.webPages.value[0].snippet + " and you can read more here " + data.webPages.value[0].displayUrl;
            }

            return response2Return;
        }
    }
}