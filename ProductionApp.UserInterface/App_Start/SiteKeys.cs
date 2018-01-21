using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace ProductionApp.UserInterface.App_Start
{
    public class SiteKeys
    {
        public static string StyleVersion
        {
            get
            {
                return "<link href=\"{0}?version=" + ConfigurationManager.AppSettings["version"] + "\" rel=\"stylesheet\"/>";
            }
        }
        public static string ScriptVersion
        {
            get
            {
                return "<script src=\"{0}?version=" + ConfigurationManager.AppSettings["version"] + "\"></script>";
            }
        }
    }
}