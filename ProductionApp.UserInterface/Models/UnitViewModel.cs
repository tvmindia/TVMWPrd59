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
        [Display(Name = "Unit")]
        [Required(ErrorMessage = "Unit is missing")]
        public string Code { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Unit is missing")]
        public string UnitCode { get; set; }
        public List<SelectListItem> UnitSelectList { get; set; }

    }
}