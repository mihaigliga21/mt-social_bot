using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Bot_Application.Model;

namespace Bot_Application.Helpers
{
    public class TextAnalytics
    {
        /// <summary>
        /// Azure portal URL.
        /// </summary>
        private const string BaseUrl = "https://westus.api.cognitive.microsoft.com/";

        private static readonly string AccountKey = Properties.Resources.TextAnalyticsApiKey;

        /// <summary>
        /// Maximum number of languages to return in language detection API.
        /// </summary>
        private const int NumLanguages = 1;

        static async Task<String> CallEndpoint(HttpClient client, string uri, byte[] byteData)
        {
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(uri, content);
                return await response.Content.ReadAsStringAsync();
            }
        }

        public static async Task<TextAnalyticsModel> MakeRequests(string message)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                // Request headers.
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AccountKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Request body. Insert your text data here in JSON format.
                byte[] byteData = Encoding.UTF8.GetBytes("{\"documents\":[" +
                    "{\"id\":\"1\",\"text\":\""+ message +"\"}]}");

                // Detect key phrases:
                var uri = "text/analytics/v2.0/keyPhrases";
                var responseKey = await CallEndpoint(client, uri, byteData);
                var resdeser = new JavaScriptSerializer().Deserialize<KeyPhrase>(responseKey);

                // Detect language:
                /*var queryString = HttpUtility.ParseQueryString(string.Empty);
                 queryString["numberOfLanguagesToDetect"] = NumLanguages.ToString(CultureInfo.InvariantCulture);
                 uri = "text/analytics/v2.0/languages?" + queryString;
                 response = await CallEndpoint(client, uri, byteData);*/

                // Detect sentiment:
                uri = "text/analytics/v2.0/sentiment";
                var responselang = await CallEndpoint(client, uri, byteData);
                var langdeser = new JavaScriptSerializer().Deserialize<SentimentModel>(responselang);

                TextAnalyticsModel textAnalytics = new TextAnalyticsModel()
                {
                    KeyPhrases = resdeser,
                    Sentiment = langdeser
                };

                return textAnalytics;
            }
        }
    }
}