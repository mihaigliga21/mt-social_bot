using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application.Model
{
    public class About
    {
        public string name { get; set; }
    }

    public class DeepLink
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Value
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public List<About> about { get; set; }
        public string displayUrl { get; set; }
        public string snippet { get; set; }
        public string dateLastCrawled { get; set; }
        public List<DeepLink> deepLinks { get; set; }
    }

    public class WebPages
    {
        public string webSearchUrl { get; set; }
        public int totalEstimatedMatches { get; set; }
        public List<Value> value { get; set; }
    }

    public class Thumbnail
    {
        public int width { get; set; }
        public int height { get; set; }
    }

    public class InsightsSourcesSummary
    {
        public int shoppingSourcesCount { get; set; }
        public int recipeSourcesCount { get; set; }
    }

    public class Value2
    {
        public string name { get; set; }
        public string webSearchUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string datePublished { get; set; }
        public string contentUrl { get; set; }
        public string hostPageUrl { get; set; }
        public string contentSize { get; set; }
        public string encodingFormat { get; set; }
        public string hostPageDisplayUrl { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public Thumbnail thumbnail { get; set; }
        public InsightsSourcesSummary insightsSourcesSummary { get; set; }
    }

    public class Images
    {
        public string id { get; set; }
        public string readLink { get; set; }
        public string webSearchUrl { get; set; }
        public bool isFamilyFriendly { get; set; }
        public List<Value2> value { get; set; }
        public bool displayShoppingSourcesBadges { get; set; }
        public bool displayRecipeSourcesBadges { get; set; }
    }

    public class Value3
    {
        public string text { get; set; }
        public string displayText { get; set; }
        public string webSearchUrl { get; set; }
    }

    public class RelatedSearches
    {
        public string id { get; set; }
        public List<Value3> value { get; set; }
    }

    public class Value4
    {
        public string id { get; set; }
    }

    public class Item
    {
        public string answerType { get; set; }
        public Value4 value { get; set; }
        public int? resultIndex { get; set; }
    }

    public class Mainline
    {
        public List<Item> items { get; set; }
    }

    public class RankingResponse
    {
        public Mainline mainline { get; set; }
    }

    public class BingSearchModel
    {
        public string _type { get; set; }
        public WebPages webPages { get; set; }
        public Images images { get; set; }
        public RelatedSearches relatedSearches { get; set; }
        public RankingResponse rankingResponse { get; set; }
    }

}