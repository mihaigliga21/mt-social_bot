using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot_Application.Helpers
{
    [Serializable]
    public class BasicMakerDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {            
            return null;
        }

        private string CallAimlService(string message)
        {

            return null;
        }
    }
}