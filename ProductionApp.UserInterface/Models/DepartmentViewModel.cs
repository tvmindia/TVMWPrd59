using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class DepartmentViewModel
    {
        [Display(Name = "Department")]
        public string Code { get; set; }
        public string Name { get; set; }
        public List<SelectListItem> departmentSelectList { get; set; }
    }
}