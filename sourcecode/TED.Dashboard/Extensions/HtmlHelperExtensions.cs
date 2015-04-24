using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Optimization;
using Forloop.HtmlHelpers;

namespace TED.Dashboard.Extensions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Adds code to include widget view model and initialize it
        /// </summary>
        /// <param name="helper">override helper</param>
        /// <param name="viewModelScript">script file containing view model</param>
        /// <param name="moduleName">view model to initialize</param>
        /// <param name="bindControlName">html control to bind view model to (knockout js)</param>
        /// <param name="dependencies">modules for requirejs to load that the view is dependant on</param>
        /// <returns></returns>
        public static void InitializeWidget(this HtmlHelper helper, string viewModelScript, string moduleName, string bindControlName, string[] dependencies = null)
        {
            // add viewmodel script to viewmodel bundle
            var bundle = BundleTable.Bundles.GetBundleFor("~/bundles/widgetviewmodels");
            bundle.Include(viewModelScript);

            // create script block to initialize viewmodel
            var script = new StringBuilder();
            var depString = new StringBuilder();
            if (dependencies != null)
            {
                foreach (var dependency in dependencies)
                {
                    depString.AppendFormat("\"{0}\",", dependency);
                }
            }

            script.AppendFormat("   require([{0} \"domReady!\"],{1}", depString, Environment.NewLine);
            script.AppendLine("       function(){");
            script.AppendFormat("           require([\"{0}\"]{1},", moduleName, Environment.NewLine);
            script.AppendLine("                 function (Module) {");
            script.AppendFormat("                   var bindCt = document.getElementById(\"{0}\");{1}", bindControlName, Environment.NewLine);
            script.AppendLine("                     var viewModel = new Module();");
            script.AppendLine("                     viewModel.init(bindCt);");
            script.AppendLine("                 }");
            script.AppendLine("             );");
            script.AppendLine("         }");
            script.AppendLine("     );");

            helper.AddScriptBlock(script.ToString());
        }
    }
}