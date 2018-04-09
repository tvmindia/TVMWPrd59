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
        [Required(ErrorMessage ="Material Type code is required")]
        [MaxLength(10,ErrorMessage ="Length exceeds")]
        public string Code { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public bool IsUpdate { get; set; }
        [Required(ErrorMessage = "Please Select Material Type ")]
        public string MaterialTypeCode { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }
}