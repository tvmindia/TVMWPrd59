using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class AppObjectViewModel
    {
        public Guid ID { get; set; }
        public Guid AppID { get; set; }
        public string AppName { get; set; }
        [Required(ErrorMessage = "Please Enter Object name")]
        [Display(Name = "Object Name")]
        public string ObjectName { get; set; }
        public CommonViewModel commonDetails { get; set; }
        public List<SelectListItem> ApplicationList { get; set; }
    }

    public class AppSubobjectViewmodel
    {
        public Guid ID { get; set; }
        public Guid AppID { get; set; }

        [Required(ErrorMessage = "Please Enter Sub-object name")]
        public Guid ObjectID { get; set; }

        [Display(Name = "Sub Object Name")]
        [Required(ErrorMessage = "Please Enter Sub-object name")]
        public string SubObjName { get; set; }
        public string AppName { get; set; }
        public string ObjectName { get; set; }

        public CommonViewModel commonDetails { get; set; }
        public List<SelectListItem> ApplicationList { get; set; }
        public List<SelectListItem> ObjectList { get; set; }
    }
}