using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class EmployeeCategoryViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        //Additional Fields
        public CommonViewModel Common { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }
}