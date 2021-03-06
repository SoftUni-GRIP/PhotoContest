﻿namespace PhotoContest.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js")
                .Include("~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                "~/Scripts/signalRConnection.js"));

            bundles.Add(new ScriptBundle("~/bundles/unobtrusive")
                  .Include("~/Scripts/jquery.unobtrusive-ajax.min.js")
                  );

            bundles.Add(new ScriptBundle("~/bundles/modals")
            .Include("~/Scripts/modalFormInvoker.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/moment.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo-console").Include(
                "~/Scripts/kendo/kendo.console.js"
              ));


            bundles.Add(new ScriptBundle("~/bundles/bootstrap-datetimepicker")
            .Include("~/Scripts/bootstrap-datetimepicker.min.js")
            .Include("~/Scripts/bootstrap-calendar-picker.js")
            );

            bundles.Add(new StyleBundle("~/Content/bootstrap-datetimepicker")
                .Include("~/Content/bootstrap-datetimepicker.min.css")
                );

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"
                ));

            bundles.Add(new StyleBundle("~/content/toastr", "http://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css")
                .Include("~/Content/toastr.css"));

            bundles.Add(new ScriptBundle("~/bundles/toastr", "http://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js")
                .Include("~/Scripts/toastr.js"));
        }
    }
}