using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class UnitViewModel
    {
        [Required(ErrorMessage = "Please Select Unit")]
        public string Code { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Select Unit")]
        public string UnitCode { get; set; }
        public List<SelectListItem> UnitSelectList { get; set; }

    }
}