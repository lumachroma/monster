using System.Web;
using System.Web.Optimization;

namespace Monster
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
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                        "~/Scripts/knockout-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/moment*"));

            bundles.Add(new ScriptBundle("~/bundles/ko-scripts-all").Include(
                        "~/Scripts/KOScripts/KOShared.js",
                        "~/Scripts/KOScripts/KOAll.js"));

            bundles.Add(new ScriptBundle("~/bundles/ko-scripts-details").Include(
                        "~/Scripts/KOScripts/KOShared.js",
                        "~/Scripts/KOScripts/KOSchemas/KO*",
                        "~/Scripts/KOScripts/KODetails.js"));

            bundles.Add(new ScriptBundle("~/bundles/ko-scripts-password").Include(
                        "~/Scripts/KOScripts/KOShared.js",
                        "~/Scripts/KOScripts/KOSchemas/KOUser.js",
                        "~/Scripts/KOScripts/KOPassword.js"));

            bundles.Add(new ScriptBundle("~/bundles/ko-firebase-scripts").Include(
                        "~/Scripts/KOScripts/KOShared.js",
                        "~/Scripts/KOScripts/KOSchemas/KOAuction.js",
                        "~/Scripts/KOFBScripts/KOFBConfig.js",
                        "~/Scripts/KOFBScripts/KOFBAuctionApp.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css"));

            //BundleTable.EnableOptimizations = true;
        }
    }
}
