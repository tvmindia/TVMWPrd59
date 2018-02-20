using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialTypeViewModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        [Required]
        public string MaterialTypeCode { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }
}