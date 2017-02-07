using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using VDS.RDF;
using VDS.RDF.Parsing;
using MT_Bot_DBpedia_Interface.Models;

namespace MT_Bot_DBpedia_Interface.Helpers
{
    public class FredService
    {
        public List<FredGraphList> GetGraphsFredServ(string phraze)
        {
            var response = new List<FredGraphList>();

            if (!String.IsNullOrEmpty(phraze))
            {
                IGraph h = new Graph();
                TurtleParser ttlparser = new TurtleParser();

                var fredUri = "http://wit.istc.cnr.it/stlab-tools/fred/?text=" + phraze;

                var httpWebRequest = (HttpWebRequest) WebRequest.Create(HttpUtility.UrlDecode(fredUri));
                httpWebRequest.Accept = "text/turtle";
                httpWebRequest.Method = "GET";
                
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    ttlparser.Load(h, streamReader);
                }

                var nodeList = new List<INode>();
                var tripleList = new List<Triple>();

                foreach (var node in h.Nodes)
                {                    
                }
            }


            return response;
        }
    }
}