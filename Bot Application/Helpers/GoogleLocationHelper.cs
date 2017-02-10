using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bot_Application.Services;
using Microsoft.Bot.Connector;

namespace Bot_Application.Helpers
{
    public class GoogleLocationHelper
    {
        //todo
        private string FindLocation(string message)
        {
            var location = FindItemAndProperty(message, "location");
            if (location.Item1 == "")
                return "I couldn't figure out which place are you looking for.";
            else
            {
                var googleLocation = GoogleLocationService.GetCoordinates(location.Item1);
                object geo = googleLocation.lat + ',' + googleLocation.lng;
                Place place = new Place(null, geo, null, googleLocation.type, googleLocation.formatted_address);

                //if(googleLocation)
            }

            return "I couldn't find this place " + location.Item1 != "" ? location.Item1 : "";
        }

        //todo
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
    }
}