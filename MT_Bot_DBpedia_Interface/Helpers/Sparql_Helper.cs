using System;
using System.Collections.Generic;
using System.Linq;

using MT_Bot_DBpedia_Interface.Models;
using MT_Bot_DBpedia_Interface.SPARQL_Interface;

namespace MT_Bot_DBpedia_Interface.Helpers
{
    // ReSharper disable once InconsistentNaming
    public class Sparql_Helper
    {
        
        public List<Sparql_Result_Item_Model> ProcessSparqlQuery(string queriedItem, string queriedProperty)
        {
            var queryDbResponse = new List<Sparql_Result_Item_Model>();

            var endpoint = "http://dbpedia.org/sparql";
            var endpointGraph = "http://dbpedia.org";

            var prefix = "prefix dbpedia: <http://dbpedia.org/resource/> " + Environment.NewLine +
                         "prefix dbprop: <http://dbpedia.org/property/> " + Environment.NewLine +
                         "prefix dbo: <http://dbpedia.org/ontology/> " + Environment.NewLine +
                         "prefix foaf: <http://xmlns.com/foaf/0.1/>";

            var qTestSpace = queriedItem.Split(' ');
            if (qTestSpace.Count() > 1)
                queriedItem = queriedItem.Replace(' ', '_');

            string query = "select ?p ?o where " +
                           "{ dbpedia:" + queriedItem + " ?p ?o " +
                           "filter (strstarts(str(?p),str(dbprop:)) || strstarts(str(?p),str(dbo:)) || strstarts(str(?p),str(foaf:)))" + Environment.NewLine +
                           "filter(langMatches(lang(?o), \"en\")) " + Environment.NewLine +
                           "}";

            var createFullQuery = prefix + query;
            var dbpediaResponse = RemoteSparqlEndpoint.QueryRemoteEndpoint(createFullQuery, endpoint, endpointGraph);

            if (dbpediaResponse.Count > 0)
            {
                var dbpediaResultList = new List<Sparql_Result_Model_Item_Property>();
                foreach (var responseResult in dbpediaResponse.Results)
                {
                    var responseItem = new Sparql_Result_Model_Item_Property()
                    {
                        PropertyName = responseResult[0].ToString().Split('/').Last(),
                        PropertyValue = responseResult[1].ToString()
                    };

                    dbpediaResultList.Add(responseItem);
                }

                var sparqlResultModelList = new Sparql_Result_Model()
                {
                    QueredItem = queriedItem,
                    QueredPropertyList = dbpediaResultList
                };

                foreach (var itemProperty in sparqlResultModelList.QueredPropertyList)
                {
                    if (queriedProperty.Contains(itemProperty.PropertyName) || itemProperty.PropertyName.Contains(queriedProperty))
                    {
                        Sparql_Result_Item_Model itemCheck = null;

                        if (queryDbResponse.Count > 0)
                            itemCheck = queryDbResponse.FirstOrDefault(x=>x.QueredProperty.Trim() == itemProperty.PropertyValue.Trim());

                        if (itemCheck == null)
                        {
                            var foundedItem = new Sparql_Result_Item_Model()
                            {
                                QueredItem = queriedItem,
                                QueredProperty = itemProperty.PropertyValue,
                                Status = "I have found this information."
                            };
                            queryDbResponse.Add(foundedItem);
                        }
                        else if (itemCheck.QueredProperty != itemProperty.PropertyValue)
                        {
                            var foundedItem = new Sparql_Result_Item_Model()
                            {
                                QueredItem = queriedItem,
                                QueredProperty = itemProperty.PropertyValue,
                                Status = "I have found this information."
                            };
                            queryDbResponse.Add(foundedItem);
                        }
                    }
                }
            }
            else
            {
                var newItem = new Sparql_Result_Item_Model();
                newItem.Status = "Your query didn't return anything. I'm sorry!";
                queryDbResponse.Add(newItem);
            }

            return queryDbResponse;
        }

        public List<Sparql_Result_Item_Model> ProcessSparqlQueryByPhrase(string phrase)
        {
            if (!String.IsNullOrEmpty(phrase))
            {
                var responseList = new List<Sparql_Result_Item_Model>();

               
            }           

            return null;
        }
    }
}