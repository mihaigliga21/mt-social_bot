using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Bot_Application.Model;
using System.Web;

namespace Bot_Application.Analyser
{
    public class TextAnalytics
    {
        /// <summary>
        /// Azure portal URL.
        /// </summary>
        private static string BaseUrl = Properties.Resources.CognitiveServices; 

        public static async Task<HttpResponseMessage> CallEndpoint(HttpClient client, string uri, byte[] byteData)
        {
            try
            {
                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = await client.PostAsync(uri, content);                   
                    return response;
                }
            }
            catch (Exception e)
            {                
                throw e;
            }            
        }

        public static async Task<TextAnalyticsModel> GetKeyPhrasesAndSent(string message)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                // Request headers.
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Properties.Resources.TextAnalyticsApiKey);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Request body. Insert your text data here in JSON format.
                byte[] byteData = Encoding.UTF8.GetBytes("{\"documents\":[" +
                    "{\"id\":\"1\",\"text\":\""+ message +"\"}]}");

                // Detect key phrases:
                var uri = "text/analytics/v2.0/keyPhrases";
                var response = await CallEndpoint(client, uri, byteData);


                var json = await response.Content.ReadAsStringAsync();

                var resdeser = new JavaScriptSerializer().Deserialize<KeyPhrase>(json);

                // Detect language:
                /*var queryString = HttpUtility.ParseQueryString(string.Empty);
                 queryString["numberOfLanguagesToDetect"] = NumLanguages.ToString(CultureInfo.InvariantCulture);
                 uri = "text/analytics/v2.0/languages?" + queryString;
                 response = await CallEndpoint(client, uri, byteData);*/

                // Detect sentiment:
                uri = "text/analytics/v2.0/sentiment";
                response = await CallEndpoint(client, uri, byteData);
                json = await response.Content.ReadAsStringAsync();
                var sentimentser = new JavaScriptSerializer().Deserialize<SentimentModel>(json);

                TextAnalyticsModel textAnalytics = new TextAnalyticsModel()
                {
                    KeyPhrases = resdeser,
                    Sentiment = sentimentser
                };

                return textAnalytics;
            }
        }

        public static async Task<LinguisticAnalitycModel> GetLinguisticAnalytic(string message)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(String.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "9f3d41718e8842fa80e533a204f6fde6");

            var uri = BaseUrl + "/linguistics/v1.0/analyze?" + queryString;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("{\"language\": \"en\", " +
                                                     "\"analyzerIds\" : [\"4fa79af1-f22c-408d-98bb-b7d7aeef7f04\", \"22a6b758-420f-4745-8a3c-46835a67c0d2\"]," +
                                                     "\"text\": \""+ message +"\"}");

            var response = await CallEndpoint(client, uri, byteData);
            var json = await response.Content.ReadAsStringAsync();
            var analytic = new JavaScriptSerializer().Deserialize<LinguisticAnalitycModel>(json);

            return analytic;
        }
    }
}