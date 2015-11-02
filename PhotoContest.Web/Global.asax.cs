﻿namespace PhotoContest.Web
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Infrastructure.Mappings;
    using ModelBinders;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            var autoMapperConfig = new AutoMapperConfig(new List<Assembly> {Assembly.GetExecutingAssembly()});
            autoMapperConfig.Execute();

            ModelBinderProviders.BinderProviders.Add(new EntityModelBinderProviderById());
        }
    }
}