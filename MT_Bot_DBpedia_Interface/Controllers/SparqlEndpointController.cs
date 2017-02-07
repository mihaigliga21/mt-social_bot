using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MT_Bot_DBpedia_Interface.Models;
using MT_Bot_DBpedia_Interface.Helpers;

namespace MT_Bot_DBpedia_Interface.Controllers
{
    [RoutePrefix("api/Endpoint")]
    public class SparqlEndpointController : ApiController
    {
        private readonly Sparql_Helper _sparqlHelper = new Sparql_Helper();
        
        /// <summary>
        /// Get dbpedia response (e.g. queriedItem=Romania and queriedProperty=population
        /// </summary>
        /// <param name="queriedItem"></param>
        /// <param name="queriedProperty"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("queryDb")]
        public HttpResponseMessage QueryDbpedia(string queriedItem, string queriedProperty)
        {
            var queryDbResponse = new List<Sparql_Result_Item_Model>();

            if (!string.IsNullOrEmpty(queriedItem) && !string.IsNullOrEmpty(queriedProperty))
            {

                try
                {
                    queryDbResponse = _sparqlHelper.ProcessSparqlQuery(queriedItem, queriedProperty);
                    if (queryDbResponse.Count > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, queryDbResponse);
                    else
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                catch (Exception exception)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        /// <summary>
        /// Get dbpedia response having only phrase (e.g. phrase=What is the capital of Romania)
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("queryDbByPhrase")]
        public HttpResponseMessage QueryDbpediaByPhrase(string phrase)
        {
            var queryDbResponse = new List<Sparql_Result_Item_Model>();

            if (!string.IsNullOrEmpty(phrase))
            {
                try
                {
                    queryDbResponse = _sparqlHelper.ProcessSparqlQueryByPhrase(phrase);
                    if (queryDbResponse.Count > 0)
                        return Request.CreateResponse(HttpStatusCode.OK, queryDbResponse);
                    else
                        return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                catch (Exception)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError);
                }                
            }
            else
                return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
