using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Bot_Application.Services;

namespace Bot_Application.Helpers
{
    public class BingHelper
    {
        private string _imageContent;

        //todo
        private IEnumerable<string> CallBingService(string message, string type)
        {
            BingService.SearchApiKey = "0312087c68bd4082bfa33f8fa3df632a";

            switch (type)
            {
                case "image":
                    Task<IEnumerable<string>> urls = null;
                    urls = BingService.GetImageSearchResults(message, _imageContent, 1);
                    return urls.Result;
                case "news":
                    break;
                case "video":
                    break;
            }

            return null;
        }

        //todo
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
                        typefind = word.Replace('?', ' ');
                    else if (prev.Contains("some"))
                        typefind = word.Replace('?', ' '); ;
                }
                prev = word;
            }
            _imageContent = typefind;
        }
    }
}