using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    /// <summary>
    /// SysModules Object
    /// </summary>
    public class SysModuleViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<SysModuleViewModel> SysModuleList { get; set; }
    }
}