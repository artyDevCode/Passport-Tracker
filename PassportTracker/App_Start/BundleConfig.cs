﻿using System.Web;
using System.Web.Optimization;

namespace PassportTracker
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                   "~/Scripts/jquery.unobtrusive*",
                   "~/Scripts/jquery.validate*",
                   "~/Scripts/jquery-{version}.js",
                   "~/Scripts/jquery-ui.js",
                   "~/Scripts/jquery.timepicker.js",
                   "~/Scripts/DataTables-1.9.4/media/js/*.js",
                   "~/Scripts/DataTables-1.9.4/extras/TableTools/media/js/*.js",
                   //"~/Scripts/tinymce/tinymce.min.js",
                   "~/Scripts/custom.js"
                   ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                   "~/Content/bootstrap.css",
                   "~/Content/Custom.css",
                   "~/Content/jquery-ui.css",
                   "~/Content/jquery.timepicker.css",
                   "~/Content/DataTables-1.9.4/media/css/*.css",
                   "~/Content/DataTables-1.9.4/extras/TableTools/media/css/*.css",
                   "~/Media/layout.css",
                   "~/Media/elements.css",
                   "~/Content/site.css"
                  
                   ));

        }
    }
}
