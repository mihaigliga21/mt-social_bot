using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Bot_Application.Services
{
    public class DbAimlService
    {
        public string CallAimlService(string message)
        {
            try
            {
                var path = Properties.Resources.AimlServiceUrl + message;

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
    }
}