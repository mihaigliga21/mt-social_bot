using System;
using VDS.RDF.Query;

namespace MT_Bot_DBpedia_Interface.SPARQL_Interface
{
    public class RemoteSparqlEndpoint
    {
        public static SparqlResultSet QueryRemoteEndpoint(string query, string endpoind, string endpointGraph)
        {
            var result = new SparqlResultSet();
            var endpoint = new SparqlRemoteEndpoint(new Uri(endpoind), endpointGraph);
            try
            {
                result = endpoint.QueryWithResultSet(query);
                if (result is SparqlResultSet)
                {
                    return result;
                }
            }
            catch (Exception exception)
            {
                result = new SparqlResultSet(false);
                return result;
            }
            return null;
        }
    }
}