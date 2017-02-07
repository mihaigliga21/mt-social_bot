using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application.Model
{
    public class WordAnalyserModel
    {
        public string initialInput { get; set; }
        public int numberOfSentences { get; set; }
        public List<Token> tokens { get; set; }
    }
    public class Token
    {
        public string id { get; set; }
        public string word { get; set; }
        public string lemma { get; set; }
        public string pos { get; set; }
        public string entity { get; set; }
        public bool isKeyword { get; set; }
        public int sentenceNumber { get; set; }
        public int wordNumber { get; set; }
        public List<object> synonyms { get; set; }
    }
}