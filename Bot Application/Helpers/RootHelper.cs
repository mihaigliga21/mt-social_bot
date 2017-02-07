using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Bot_Application.Model;
using MT_Bot_DBpedia_Interface.Helpers;
using Microsoft.Bot.Builder.Location;
using Microsoft.Bot.Builder.Location.Bing;
using Microsoft.Bot.Connector;

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

        private readonly List<string> _keyWordsAbstract = new List<string>()
        {
            "who", "is", "who is", "what"
        };

        private readonly List<string> _keyWordsPlace = new List<string>()
        {
            "where", "located", "in"
        };

        #endregion

        public string MainRoot(string phrase)
        {
            var response = "";
            try
            {
                //try aiml
                var aimlResponse = CallAimlService(phrase);

                //search info in dbpedia
                if ("<oob><search>".Contains(aimlResponse) || aimlResponse.Contains("<oob><search>"))
                {
                    var getItemAndProp = FindItemAndProperty(phrase, "dbpedia");
                    if (getItemAndProp.Item1 != "" && getItemAndProp.Item2 != "")
                    {
                        var dbpediaResponse = SearchDbpedia(getItemAndProp.Item1, getItemAndProp.Item2);
                        if (dbpediaResponse != null)
                        {
                            response = dbpediaResponse;
                            return response;
                        }
                    }
                    else
                        return response = "I'm sorry something went wrong :( need to talk with my developers.";
                }
                else if ("<oob><map>".Contains(aimlResponse) || aimlResponse.Contains("<oob><map>"))
                {
                    var locationResp = GetLocation(phrase);
                    response = locationResp;
                    return response;
                }
                else
                    if (aimlResponse != "")
                        return response = aimlResponse;

                if (aimlResponse == "")
                    return response = "Nobody knows all answers! I'm not able to answer this quertion.";
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
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
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
                var formatPhrase = "\"" + phrase + "\"";
                var analyserJson = run_cmd(formatPhrase);
                if (analyserJson != "")
                {
                    WordAnalyserModel wordAnalyser = new JavaScriptSerializer().Deserialize<WordAnalyserModel>(analyserJson);
                    if (wordAnalyser != null)
                    {
                        if (calledBy == "dbpedia")
                        {
                            List<Token> itemsKeyWord = wordAnalyser.tokens.Where(x => x.isKeyword == true).ToList();
                            if (itemsKeyWord.Count > 0)
                            {
                                var item = string.Empty;
                                var property = string.Empty;
                                var npList = new List<string>();
                                foreach (Token token in itemsKeyWord)
                                {
                                    if (_keyWordsAbstract.Contains(token.word))
                                        property = "abstract";
                                    if (token.pos == "NNP")
                                        npList.Add(token.word);
                                }
                                if (npList.Count > 0)
                                    item = npList[0] + " " + npList[1];

                                var t = new Tuple<string, string>(item, property);
                                response = t;

                                return response;
                            }
                        }
                        else if (calledBy == "location")
                        {
                            var entity2Ret = wordAnalyser.tokens.FirstOrDefault(x => x.entity.ToLower() == "location").word;
                            if (entity2Ret != "")
                                return response = new Tuple<string, string>(entity2Ret, "");
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return response;
        }

        private string run_cmd(string phrase)
        {

            string fileName = @"C:\mt-bot\WordAnalyser.py " + phrase;

            Process p = new Process();
            p.StartInfo = new ProcessStartInfo(@"C:\Users\UserLB50\Anaconda2\python.exe", fileName)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return output;
        }

        private string GetLocation(string phrase)
        {
            BingSearchWeb bingSearch = new BingSearchWeb();
            Task<string> task = Task.Run<string>(async () => await bingSearch.MakeRequest(phrase));
            return task.Result;
        }
    }
}