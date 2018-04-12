using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class DepartmentViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<SelectListItem> departmentSelectList { get; set; }
    }
}