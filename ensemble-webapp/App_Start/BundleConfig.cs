using System.Web;
using System.Web.Optimization;

namespace ensemble_webapp
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

            bundles.Add(new ScriptBundle("~/bundles/sidebar").Include(
                        "~/Content/assets/js/jquery-migrate-3.0.0.min.js",
                        "~/Content/assets/js/jquery.backstretch.js",
                        "~/Content/assets/js/jquery.backstretch.min.js",
                        "~/Content/assets/js/jquery.mCustomerScrollbar.concat.min.js",
                        "~/Content/assets/js/jquery.waypoints.js",
                        "~/Content/assets/js/jquery.waypoints.min.js",
                        "~/Content/assets/js/scripts.js",
                        "~/Content/assets/js/waypoints.js",
                        "~/Content/assets/js/waypoints.min.js",
                        "~/Content/assets/js/wow.js",
                        "~/Content/assets/js/wow.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/global.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/sidebar-styles").Include(
                      "~/Content/assets/css/animate.css",
                      "~/Content/assets/css/jquery.mCustomerScrollbar.min.css",
                      "~/Content/assets/css/media-queries.css",
                      "~/Content/assets/css/style.css"));
        }
    }
}
