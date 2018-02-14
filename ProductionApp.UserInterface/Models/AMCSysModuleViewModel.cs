using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    /// <summary>
    /// SysModules Object
    /// </summary>
    public class AMCSysModuleViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string IconClass { get; set; }
        public List<AMCSysModuleViewModel> SysModuleList { get; set; }
    }
}