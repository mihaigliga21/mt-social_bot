using System;
using System.Collections.Generic;

namespace MT_Bot_DBpedia_Interface.Models
{
    // ReSharper disable once InconsistentNaming
    public class Sparql_Result_Model
    {
        private string _queriedItem = String.Empty;
        private List<Sparql_Result_Model_Item_Property> _queriedPropertyList = new List<Sparql_Result_Model_Item_Property>();

        public string QueredItem
        {
            get { return _queriedItem; }
            set { _queriedItem = value; }
        }

        public List<Sparql_Result_Model_Item_Property> QueredPropertyList
        {
            get { return _queriedPropertyList; }
            set { _queriedPropertyList = value; }
        }
    }

    // ReSharper disable once InconsistentNaming
    public class Sparql_Result_Model_Item_Property
    {        
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }

    // ReSharper disable once InconsistentNaming
    public class Sparql_Result_Item_Model
    {
        private string _queriedItem = String.Empty;
        private string _queriedProperty = String.Empty;
        private string _status = String.Empty;

        public string QueredItem
        {
            get { return _queriedItem; }
            set { _queriedItem = value; }
        }

        public string QueredProperty
        {
            get { return _queriedProperty; }
            set { _queriedProperty = value; }
        }

        public string Status
        {
            get { return _status; } 
            set { _status = value; }
        }
    }
}