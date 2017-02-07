using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Bot_Application.Helpers
{
    public class RootHelper
    {
        #region keywords

        private readonly List<string> _searchDbpediaListKeywords = new List<string>()
        {
            "I don't know the answer.",
            "I used my lifeline to ask another robot, but he didn't know.",
            "I asked another robot, but he didn't know.",
            "Sorry, nothing found in web services.",
            "I'm unable to find the answer from web services.",
            "None of the other robots can tell me the answer.",
            "Let's try Google.",
            "I'm going to try a search",
            "Perhaps we should try a web search.",
            "I'll try asking Google."
        };

        #endregion

        public string MainRoot(string phrase)
        {
            var response = "";
            try
            {
                //try aiml
                var aimlResponse = CallAimlService(phrase);

                if (_searchDbpediaListKeywords.IndexOf(aimlResponse) == -1)
                {
                    var getItemAndProp = FindItemAndProperty(phrase);
                    if (getItemAndProp.Item1 != "" && getItemAndProp.Item2 != "")
                    {
                        var dbpediaResponse = SearchDbpedia(getItemAndProp.Item1, getItemAndProp.Item2);
                    }
                }

                if (aimlResponse != "")
                    return response = aimlResponse;


            }
            catch (Exception)
            {
                response = "I'm sorry, I have a problem :( need to talk with my developers ))";
            }


            return response;
        }

        private string CallAimlService(string phrase)
        {
            try
            {
                var path = "https://mt-aiml.herokuapp.com/getResponse/" + phrase;

                // Create a request for the URL.         
                WebRequest request = WebRequest.Create(path);
                //Get the response.
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                return responseFromServer;
                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception)
            {
            }

            return null;
        }

        //todo
        private string SearchDbpedia(string item, string property)
        {
            var response = "";

            return response;
        }

        private Tuple<string, string> FindItemAndProperty(string phrase)
        {
            var response = new Tuple<string, string>("", "");
            try
            {              
               /* WordAnalyser wordAnalyser = new WordAnalyser();
                wordAnalyser.setInputSentence(phrase);
                var i = wordAnalyser.getWordsAsJSON();*/
            }
            catch (Exception)
            {                
                throw;
            }            

            return response;
        }      
    }
}