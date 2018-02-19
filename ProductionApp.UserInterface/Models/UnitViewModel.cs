using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class UnitViewModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string UnitCode { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }
}