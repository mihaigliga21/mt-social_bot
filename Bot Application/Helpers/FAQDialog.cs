using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot_Application.Analyser;
using Bot_Application.Model;
using Bot_Application.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;


namespace Bot_Application.Helpers
{
    [Serializable]
    public class FaqDialog : IDialog<object>
    {
        #region global       

        private TextAnalyticsModel _textAnalyticsModel = null;
        private List<LinguisticAnalitycModel> _linguisticAnalitycModel = null;
        #endregion

#pragma warning disable 1998
        public async Task StartAsync(IDialogContext context)
#pragma warning restore 1998
        {
            context.Wait(MessageRecievedAsync);
        }

        private async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            var botResponse = string.Empty;
            var aimlResponse = new DbAimlService().CallAimlService(message.Text);

            var messageType = GetQuestionType(aimlResponse, message.Text);

            await messageType;
            if (messageType.Result == null)
                await context.PostAsync("I can't figure out what do you mean. Try again. - message type == null");
            else
            {
                await messageType;
                switch (messageType.Result)
                {
                    case "dbpedia-search":
                        botResponse = new DbpediaHelper().GetDbpediaResult(_textAnalyticsModel, _linguisticAnalitycModel, message.Text, aimlResponse);
                        break;
                    case "bing-search":
                        break;
                    case "valid-aiml":
                        botResponse = aimlResponse;
                        break;
                    case "error":
                        botResponse = "I'm sorry, there was a problem. Try again or hate me )). - case error";
                        break;
                    case "don't know":
                        botResponse = "I can't figure out what do you mean. Try again. - case don't know";
                        break;
                    case "google-location":
                        break;
                }

                await context.PostAsync(botResponse);
            }

            context.Wait(MessageRecievedAsync);
        }

        //set question type for future procesing
        private async Task<string> GetQuestionType(string messageAiml, string messageOriginal)
        {
            //analyse aiml response first
            if (!messageAiml.Contains("<oob><search>") && !messageAiml.Contains("<oob><map>") &&
                !messageAiml.Contains("<oob><url>"))
                return "valid-aiml";

            //analyse original text with cognitive-services text analytics
            GetTextAnalyticsModel(messageOriginal);
            GetLinguisticAnalytic(messageOriginal);

            //anayser returns nothing
            if (_linguisticAnalitycModel == null && _textAnalyticsModel == null)
                return "error";
            else
            {
                //when aiml response contains <oob><search> we are going to search dbpedia => here we need to find queriedItem and his queriedProperty
                if (messageAiml.Contains("<oob><search>"))
                    return "dbpedia-search";

                //when aiml response contains <oob><map> we are going to serch google location => here we need to identify locationItemQueried 
                else if (messageAiml.Contains("<oob><map>"))
                    return "google-location";
                //when aiml response containt <oob><url> + image/video/song we are going to search it with bing services => here we need to identify which type of media and it's content
                else if (messageAiml.Contains("<oob><url>"))
                    return "bing-search";
                else
                    return "don't know";
            }
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

        #region call cognitive service

        private async void GetTextAnalyticsModel(string message)
        {
            var analyser = await TextAnalytics.GetKeyPhrasesAndSent(message);

            _textAnalyticsModel = analyser;
        }

        private async void GetLinguisticAnalytic(string message)
        {
            var textLinguisticAnalytic = await TextAnalytics.GetLinguisticAnalytic(message);
            _linguisticAnalitycModel = textLinguisticAnalytic;
        }

        #endregion
    }
}