using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Bot_Application.Model;
using Newtonsoft.Json.Linq;

namespace Bot_Application.Services
{
    public class BingService
    {
        private static string _imageSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/images/search";
        private string _autoSuggestionEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/suggestions";
        private string _newsSearchEndPoint = "https://api.cognitive.microsoft.com/bing/v5.0/news/search";

        private static HttpClient AutoSuggestionClient { get; set; }
        private static HttpClient SearchClient { get; set; }

        private static string _autoSuggestionApiKey;
        public static string AutoSuggestionApiKey
        {
            get { return _autoSuggestionApiKey; }
            set
            {
                var changed = _autoSuggestionApiKey != value;
                _autoSuggestionApiKey = value;
                if (changed)
                {
                    InitializeBingClients();
                }
            }
        }

        private static string _searchApiKey;
        public static string SearchApiKey
        {
            get { return _searchApiKey; }
            set
            {
                var changed = _searchApiKey != value;
                _searchApiKey = value;
                if (changed)
                {
                    InitializeBingClients();
                }
            }
        }

        private static void InitializeBingClients()
        {
            AutoSuggestionClient = new HttpClient();
            AutoSuggestionClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AutoSuggestionApiKey);

            SearchClient = new HttpClient();
            SearchClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SearchApiKey);
        }

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

        //search image
        public static async Task<IEnumerable<string>> GetImageSearchResults(string query, string imageContent = "Face", int count = 20, int offset = 0)
        {
            List<string> urls = new List<string>();

            var result = await SearchClient.GetAsync(string.Format("{0}?q={1}&safeSearch=Strict&count={2}&offset={3}", _imageSearchEndPoint, WebUtility.UrlEncode(query), count, offset));
            result.EnsureSuccessStatusCode();
            var json = await result.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(json);
            if (data.value != null && data.value.Count > 0)
            {
                for (int i = 0; i < data.value.Count; i++)
                {
                    urls.Add(data.value[i].contentUrl.Value);
                }
            }

            return urls;
        }

    }
}