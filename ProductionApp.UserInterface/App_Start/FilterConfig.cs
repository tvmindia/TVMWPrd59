using System.Web;
using System.Web.Mvc;


namespace ProductionApp.UserInterface.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}