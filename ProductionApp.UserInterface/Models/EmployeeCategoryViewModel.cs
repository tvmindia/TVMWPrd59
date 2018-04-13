using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class EmployeeCategoryViewModel
    {
        [Display(Name = "Category")]
        public string Code { get; set; }
        public string Name { get; set; }
        //Additional Fields
        public CommonViewModel Common { get; set; }
        public List<SelectListItem> employeeCategorySelectList { get; set; }
    }
}