using System.Web;
using System.Web.Optimization;

namespace EzollutionPro
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.3.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/additional-methods.min.js",
                         "~/Scripts/jquery.validate.unobtrusive.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/bootstrap/js/bootstrap.js",
                      "~/Content/datatables/dataTables.min.js",
                      "~/Content/toastr/toastr.min.js",
                      "~/Content/bootstrap-select/bootstrap-select.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap/css/bootstrap.css",
                      "~/Content/bootstrap/css/font-awesome.min.css",
                      "~/Content/dist/css/AdminLTE.min.css",
                      "~/Content/dist/css/skins/_all-skins.min.css",
                      "~/Content/datatables/bootstrap.dataTables.min.css",
                      "~/Content/toastr/toastr.min.css",
                      "~/Content/bootstrap-select/bootstrap-select.min.css"
                      ));

            //BundleTable.EnableOptimizations = false;
        }
    }
}
