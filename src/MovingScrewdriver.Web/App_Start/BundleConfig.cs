using System.Web.Optimization;
using MovingScrewdriver.Web.Infrastructure;

namespace MovingScrewdriver.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true; 
            var jqueryCdn = "http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js";

            var jq = new ScriptBundle("~/bundles/jquery",
                                      jqueryCdn).Include(
                                          "~/Content/js/lib/jquery-{version}.js");
            jq.Transforms.Add(new CacheBundleTransform());
            bundles.Add(jq);

            var libs = new ScriptBundle("~/bundles/libs").Include(
                "~/Content/js/lib/b*",
                "~/Content/js/lib/m*",
                "~/Content/js/lib/p*",
                //"~/Content/js/lib/rainbow.js",
                "~/Content/js/lib/jquery.pnotify.js",
                "~/Content/js/lib/prettify/prettify.js",
                "~/Content/js/ie10wp8.js",
                "~/Content/js/app.js"
                );
            libs.Transforms.Add(new CacheBundleTransform());
            bundles.Add(libs);
            

            bundles.Add(new ScriptBundle("~/bundles/social").Include(
                "~/content/js/social.js"
                ));

            var post = new ScriptBundle("~/bundles/post-view").Include(
                "~/content/js/views/post-view.js"
                );
            post.Transforms.Add(new CacheBundleTransform());
            bundles.Add(post);

            bundles.Add(new ScriptBundle("~/bundles/screw-on").Include(
                "~/content/js/views/screw-on.js"
                ));

            var css = new StyleBundle("~/bundles/css").Include(
                "~/content/css/site.css"
                );
            css.Transforms.Add(new CacheBundleTransform());
            bundles.Add(css);

            //BundleTable.EnableOptimizations = true;
        }
    }
}