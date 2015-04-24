using System.Web.Optimization;
using BundleTransformer.Core.Builders;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;

namespace TED.Dashboard
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            var nullBuilder = new NullBuilder();
            var styleTransformer = new StyleTransformer();
            var nullOrderer = new NullOrderer();
                        
            /* scripts */
            bundles.Add(new Bundle("~/bundles/lib").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/require.js",
                "~/Scripts/colResizable-1.5.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/domReady").Include(
                "~/Scripts/domReady.js"));

            bundles.Add(new Bundle("~/bundles/ko").Include(
                "~/Scripts/knockout-{version}.js"));

            bundles.Add(new Bundle("~/bundles/modal").Include(
                "~/Scripts/bootstrapko-modal.js"));

            bundles.Add(new Bundle("~/bundles/charts").Include(
                "~/Scripts/raphael.js",
                "~/Scripts/morris.js",
                "~/Scripts/knockout-morris.js"));

            bundles.Add(new Bundle("~/bundles/forms").Include(
                "~/Scripts/select2.js",
                "~/Scripts/knockout-select2.js"));

            // use regular bundle class in debug mode so the scripts are 
            // easy to step through.  ScriptBundle will minify
            #if DEBUG
                // empty bundle for view model scripts.  Each partial view
                // will add it's viewmodel to the bundle when needed
                bundles.Add(new Bundle("~/bundles/widgetviewmodels"));

                bundles.Add(new Bundle("~/bundles/common").Include(
                    "~/Scripts/App/services/*.js",
                    "~/Scripts/App/bindings/*.js"));
            #else
                // empty bundle for view model scripts.  Each partial view
                // will add it's viewmodel to the bundle when needed
                bundles.Add(new ScriptBundle("~/bundles/widgetviewmodels"));

                bundles.Add(new ScriptBundle("~/bundles/common").Include(
                    "~/Scripts/App/services/*.js",
                    "~/Scripts/App/bindings/*.js"));
#endif

                /*  styles */
            // handle bootstrap styles
            bundles.Add(new Bundle("~/Content/css/libraries").Include(
                "~/Content/css/font-awesome.css",
                "~/Content/css/morris.css",
                "~/Content/css/select2.css"));

            bundles.Add(new StyleBundle("~/Content/css/app").Include(
               "~/Content/css/navmenu.css",
               "~/Content/css/dashboard.css", 
               "~/Content/css/inbasket.css"));

            //bundles.Add(new StyleBundle("~/Content/CommonStyles").Include(
            //   "~/Content/css/custom-bootstrap.less.css"));

            /* this is causing an error in production setup;  investigate 
             http://stackoverflow.com/questions/23820111/bootstrap-less-out-of-stack-space-error-v3/23820230#23820230
             */
            //include less files converted to css
            var commonStylesBundle = new Bundle("~/Content/CommonStyles");
            commonStylesBundle.Include(
                "~/Content/less/custom-bootstrap.less"
               );
            commonStylesBundle.Builder = nullBuilder;
            commonStylesBundle.Transforms.Add(styleTransformer);
            commonStylesBundle.Orderer = nullOrderer;
            bundles.Add(commonStylesBundle);


            #if DEBUG
                BundleTable.EnableOptimizations = false;
            #else
                BundleTable.EnableOptimizations = true;
            #endif
        }
    }
}