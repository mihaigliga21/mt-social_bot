using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application.Model
{
    public class TextAnalyticsModel
    {
        public KeyPhrase KeyPhrases { get; set; }
        public SentimentModel Sentiment { get; set; }
    }


    public class Document
    {
        public List<string> keyPhrases { get; set; }
        public string id { get; set; }
    }

    public class KeyPhrase
    {
        public List<Document> documents { get; set; }
        public List<object> errors { get; set; }
    }


    public class DocumentLang
    {
        public double score { get; set; }
        public string id { get; set; }
    }

    public class SentimentModel
    {
        public List<DocumentLang> documents { get; set; }
        public List<object> errors { get; set; }
    }


}