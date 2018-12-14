using System.Web;
using System.Web.Mvc;

namespace TODTool
{
    public class FilterConfig
    {
        private static readonly log4net.ILog log =
        log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            log.Info("Bundling GlobalFilter libraries");
            filters.Add(new HandleErrorAttribute());
        }
    }
}
