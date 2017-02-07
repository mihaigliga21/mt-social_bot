using System.Web;
using System.Web.Mvc;

namespace MT_Bot_DBpedia_Interface
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
