using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    /// <summary>
    /// SysModules Object
    /// </summary>
    public class AMCSysModule
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string IconClass { get; set; }
    }
}
