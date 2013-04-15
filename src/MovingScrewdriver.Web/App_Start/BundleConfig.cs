using System.Web.Optimization;

namespace MovingScrewdriver.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true; 
            var jqueryCdn = "http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js";

            bundles.Add(new ScriptBundle("~/bundles/jquery",
                jqueryCdn).Include(
                "~/Content/js/lib/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                        "~/Content/js/lib/b*",
                        "~/Content/js/lib/m*",
                        "~/Content/js/lib/p*",
                        //"~/Content/js/lib/rainbow.js",
                        "~/Content/js/lib/jquery.pnotify.js",
                        "~/Content/js/lib/prettify/prettify.js",
                        "~/Content/js/*.js"
                        ));
            
            bundles.Add(new ScriptBundle("~/bundles/post-view").Include(
                "~/content/js/views/post-view.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/screw-on").Include(
                "~/content/js/views/screw-on.js"
                ));

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/content/css/site.css"
                ));
        }
    }
}