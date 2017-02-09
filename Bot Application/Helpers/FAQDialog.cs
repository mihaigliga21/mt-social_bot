using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Bot_Application.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MT_Bot_DBpedia_Interface.Helpers;
using Microsoft.Bot.Builder.Location;

namespace Bot_Application.Helpers
{
    [Serializable]
    public class FaqDialog : IDialog<object>
    {
        #region global

        private readonly List<string> _keyWordsAbstract = new List<string>()
        {
            "who", "is", "who is", "what"
        };

        private string _imageContent = String.Empty;
        private string _dbpediaResponse = String.Empty;

        #endregion

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageRecievedAsync);
        }

        private async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            var botResponse = string.Empty;
            var aimlResponse = CallAimlService(message.Text);
           
            var messageType = GetQuestionType(aimlResponse, message.Text);

            if (messageType == null)
                await context.PostAsync("I can't figure out what do you mean. Try again.");
            else
            {                
                switch (messageType)
                {
                    case "dbpedia-response":
                        botResponse = _dbpediaResponse;
                        break;
                    case "search-bing":
                        break;
                    case "valid-aiml":
                        botResponse = aimlResponse;
                        break;
                    case "error":
                        botResponse = "I'm sorry, there was a problem. Try again or hate me )).";
                        break;
                    case "don't know":
                        botResponse = "I can't figure out what do you mean. Try again.";
                        break;
                    case "search-bing-image":
                        SetImageContent(message.Text);
                        var item = CallBingService(message.Text, "image");
                        botResponse = item.FirstOrDefault();
                        break;
                    case "location":
                        botResponse = FindLocation(message.Text);
                        break;
                }

                await context.PostAsync(botResponse);
            }

            context.Wait(MessageRecievedAsync);
        }   
        
        //Callback, after the QnAMaker Dialog returns a result.
        public async Task AfterQnA(IDialogContext context, IAwaitable<object> argument)
        {
            context.Wait(MessageRecievedAsync);
        }

        private string CallAimlService(string message)
        {
            try
            {
                var path = "https://mt-aiml.herokuapp.com/getResponse/" + message;

                // Create a request for the URL.         
                WebRequest request = WebRequest.Create(path);
                //Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                return responseFromServer; 
            }
            catch (Exception)
            {
                return "error";
            }
            return null;
        }

        private string GetQuestionType(string messageAiml, string messageOriginal)
        {
            //analyse aiml response first
            if (!messageAiml.Contains("<oob><search>") && !messageAiml.Contains("<oob><map>") &&
                !messageAiml.Contains("<oob><url>"))
                return "valid-aiml";


            //analyse original text with cognitive-services text analytics
            var analyser = TextAnalytics.MakeRequests(messageOriginal);
            Task.WaitAll();
            //anayser returns nothing
            if (analyser.Result == null)
                return "don't know";



            return null;
        }

        private string SearchDbpedia(string item, string property)
        {
            var response = "";

            Sparql_Helper sparqlHelper = new Sparql_Helper();
            var sparqlResp = sparqlHelper.ProcessSparqlQuery(item, property);
            if (sparqlResp.Count > 0)
            {
                var createResponse =
                    String.Format("I have searched the web and found this {0}", sparqlResp[0].QueredProperty.Split('.')[0]);

                response = createResponse;
            }

            return response;
        }

        private Tuple<string, string> FindItemAndProperty(string phrase, string calledBy)
        {
            var response = new Tuple<string, string>("", "");
            try
            {                
                
            }
            catch (Exception exception)
            {
                throw;
            }

            return response;
        }

        #region unused method                
        //private string run_cmd(string phrase)
        //{

        //    string param = @"-jar C:\mt-bot\WordAnalyser.jar " + phrase;

        //    Process p = new Process();
        //    p.StartInfo = new ProcessStartInfo(@"C:\Program Files\Java\jre1.8.0_121\bin\java.exe", param)
        //    {
        //        RedirectStandardOutput = true,
        //        UseShellExecute = false,
        //        CreateNoWindow = true
        //    };
        //    p.Start();

        //    string output = p.StandardOutput.ReadToEnd();
        //    p.WaitForExit();

        //    return output;
        //}
        #endregion

        //todo
        private IEnumerable<string> CallBingService(string message, string type)
        {
            BingSearchWeb.SearchApiKey = "0312087c68bd4082bfa33f8fa3df632a";

            switch (type)
            {
                case "image":
                    Task<IEnumerable<string>> urls = null;
                    urls = BingSearchWeb.GetImageSearchResults(message, _imageContent, 1);
                    return urls.Result;                    
                case "news":
                    break;
                case "video":
                    break;
            }

            return null;
        }

        private void SetImageContent(string message)
        {
            var messagelist = message.Split(' ');

            var prev = "";
            var typefind = string.Empty;

            foreach (string word in messagelist)
            {
                if (prev != "")
                {
                    if (prev.Contains("with"))
                        typefind = word.Replace('?',' ');
                    else if (prev.Contains("some"))
                        typefind = word.Replace('?', ' '); ;
                }
                prev = word;
            }
            _imageContent = typefind;
        }

        //todo
        private string FindLocation(string message)
        {
            var location = FindItemAndProperty(message, "location");
            if (location.Item1 == "")
                return "I couldn't figure out which place are you looking for.";
            else
            {
                var googleLocation = GoogleLocationDialog.GetCoordinates(location.Item1);
                object geo = googleLocation.lat + ','+ googleLocation.lng;
                Place place = new Place(null, geo, null, googleLocation.type, googleLocation.formatted_address);

                //if(googleLocation)
            }

            return "I couldn't find this place " + location.Item1 != "" ? location.Item1 : "";
        }
    }
}