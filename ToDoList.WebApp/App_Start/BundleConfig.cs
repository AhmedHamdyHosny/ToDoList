﻿using System.Web.Optimization;

namespace ToDoList.WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                     "~/Scripts/DataTables/js/jquery.dataTables.min.js"));

            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                      "~/Content/DataTables/css/jquery.dataTables.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/sweetAlert").Include(
                      "~/Scripts/sweetalert.min.js"));

            bundles.Add(new StyleBundle("~/Content/sweetAlert").Include(
                     "~/Content/sweetalert.css"));

            bundles.Add(new ScriptBundle("~/bundles/fontawesome").Include(
                      "~/Scripts/all.min.js"));

            bundles.Add(new StyleBundle("~/Content/fontawesome").Include(
                     "~/Content/css/all.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                      "~/Scripts/custom/utility.js"));
        }
    }
}
