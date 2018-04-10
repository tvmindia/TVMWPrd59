using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class DocumentTypeViewModel
    {
        public string Code { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Document Type is missing")]
        public string DocumentTypeCode { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }
}