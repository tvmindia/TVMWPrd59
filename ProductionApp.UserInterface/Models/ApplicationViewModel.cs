using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProductionApp.UserInterface.Models
{
    public class ApplicationViewModel
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Please Enter Application name")]
        [Display(Name = "Application Name")]
        public string Name { get; set; }

        public CommonViewModel commonDetails { get; set; }
    }
}