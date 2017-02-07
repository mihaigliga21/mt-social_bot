using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDS.RDF;

namespace MT_Bot_DBpedia_Interface.Models
{
    public class FredGraphList
    {
        public IEnumerable<INode> Nodes { get; set; }
        public IEnumerable<Triple> Triples { get; set; }
    }
}