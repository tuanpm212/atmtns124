using System.Web;
using System.Web.Optimization;

namespace LFLCorp
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/player").Include(
                        "~/scripts/build/mediaelement-and-player.js"));

            bundles.Add(new StyleBundle("~/default/main/css").Include(
                        "~/content/templates/default/css/screen-min.css"));
        }
    }
}