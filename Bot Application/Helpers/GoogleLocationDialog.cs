using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using Bot_Application.Model;

namespace Bot_Application.Helpers
{
    public class GoogleLocationDialog
    {
        public static Coordinate GetCoordinates(string region)
        {
            using (var client = new WebClient())
            {
                string uri = String.Format("https://maps.googleapis.com/maps/api/place/textsearch/json?query={0}&key={1}", region, Properties.Resources.GoogleLocationApiKey);

                if (uri != "")
                {
                    var data = new JavaScriptSerializer().Deserialize<RootObject>(uri);

                    var item2Response = new Coordinate
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

        #region class
                
        public class Location
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Northeast
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Southwest
        {
            public double lat { get; set; }
            public double lng { get; set; }
        }

        public class Viewport
        {
            public Northeast northeast { get; set; }
            public Southwest southwest { get; set; }
        }

        public class Geometry
        {
            public Location location { get; set; }
            public Viewport viewport { get; set; }
        }

        public class Photo
        {
            public int height { get; set; }
            public List<string> html_attributions { get; set; }
            public string photo_reference { get; set; }
            public int width { get; set; }
        }

        public class Result
        {
            public string formatted_address { get; set; }
            public Geometry geometry { get; set; }
            public string icon { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public List<Photo> photos { get; set; }
            public string place_id { get; set; }
            public string reference { get; set; }
            public List<string> types { get; set; }
        }

        public class RootObject
        {
            public List<object> html_attributions { get; set; }
            public List<Result> results { get; set; }
            public string status { get; set; }
        }

        public class Coordinate
        {
            public string formatted_address { get; set; }
            public string lat { get; set; }
            public string lng { get; set; }
            public string type { get; set; }
        }

        #endregion
    }
}