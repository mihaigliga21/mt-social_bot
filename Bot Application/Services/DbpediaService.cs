using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MT_Bot_DBpedia_Interface.Helpers;

namespace Bot_Application.Services
{
    public class DbpediaService
    {
        public string SearchDbpedia(string item, string property)
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
    }
}