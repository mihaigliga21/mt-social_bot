using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace Bot_Application.Helpers
{
    [Serializable]
    public class FaqDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageRecievedAsync);
        }

        private async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (IsQuestion(message.Text))
                await context.PostAsync("This looks like a question");
            else
                await context.PostAsync("This dosen't look like a question");

            context.Wait(MessageRecievedAsync);
        }

        private bool IsQuestion(string message)
        {
            //List of common question words
            List<string> questionWords = new List<string>() { "who", "what", "why", "how", "when" };

            //Question word present in the message
            Regex questionPattern = new Regex(@"\b(" + string.Join("|", questionWords.Select(Regex.Escape).ToArray()) + @"\b)", RegexOptions.IgnoreCase);

            //Return true if a question word present, or the message ends with "?"
            if (questionPattern.IsMatch(message) || message.EndsWith("?"))
                return true;
            else
                return false;
        }
    }
}