using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MapPlotter.UI.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static bool _isSqlTypesLoaded;

        protected void Application_Start()
        {

            if (!_isSqlTypesLoaded)
            {
                _isSqlTypesLoaded = true;
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
