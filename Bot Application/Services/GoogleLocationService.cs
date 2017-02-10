using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Web.Script.Serialization;
using Bot_Application.Model;
using Microsoft.Bot.Connector;

namespace Bot_Application.Services
{
    public class GoogleLocationService
    {       
        public static GoogleLocationModel.Coordinate GetCoordinates(string region)
        {
            using (var client = new WebClient())
            {
                string uri = String.Format("https://maps.googleapis.com/maps/api/place/textsearch/json?query={0}&key={1}", region, Properties.Resources.GoogleLocationApiKey);

                if (uri != "")
                {
                    var data = new JavaScriptSerializer().Deserialize<GoogleLocationModel.RootObject>(uri);

                    var item2Response = new GoogleLocationModel.Coordinate
                    {
                        formatted_address = data.results[0].formatted_address != null
                            ? data.results[0].formatted_address
                            : region,
                        lat = data.results[0].geometry.location.lat != null
                            ? data.results[0].geometry.location.lat.ToString(CultureInfo.InvariantCulture)
                            : null,
                        lng = data.results[0].geometry.location.lng != null
                            ? data.results[0].geometry.location.lng.ToString(CultureInfo.InvariantCulture)
                            : null,
                        type = data.results[0].types[0] != null ? data.results[0].types[0] : null
                    };

                    return item2Response;
                }
                return null;
            }
        }
    }
}