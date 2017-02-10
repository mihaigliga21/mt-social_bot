using System;
using System.Threading.Tasks;
using Bot_Application.Analyser;
using Bot_Application.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;


namespace Bot_Application.Helpers
{
    [Serializable]
    public class FaqDialog : IDialog<object>
    {
        #region global
        private readonly DbAimlService _aimlService = new DbAimlService();

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
            var aimlResponse = _aimlService.CallAimlService(message.Text);

            var messageType = GetQuestionType(aimlResponse, message.Text);

            if (messageType.Result == null)
                await context.PostAsync("I can't figure out what do you mean. Try again.");
            else
            {
                await messageType;
                switch (messageType.Result)
                {
                    case "dbpedia-response":
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
                        break;
                    case "location":
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
            var analyser = await TextAnalytics.GetKeyPhrasesAndSent(messageOriginal);
            var textLinguisticAnalytic = await TextAnalytics.GetLinguisticAnalytic(messageOriginal);

            //anayser returns nothing
            if (analyser == null && textLinguisticAnalytic == null)
                return "don't know";
            else
            {

            }



            return null;
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
    }
}