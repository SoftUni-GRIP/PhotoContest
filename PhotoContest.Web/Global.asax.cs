﻿namespace PhotoContest.Web
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Infrastructure.Mappings;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            var autoMapperConfig = new AutoMapperConfig(new List<Assembly> {Assembly.GetExecutingAssembly()});
            autoMapperConfig.Execute();

            // TODO: Select only Razor View Engine
        }
    }
}