using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class RolesViewModel
    {
        public Guid? ID { get; set; }

        [Required(ErrorMessage = "Please Select Application Name")]
        public Guid? AppID { get; set; }

        public string ApplicationName { get; set; }


        [Required(ErrorMessage = "Please enter Role Name")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Please enter Role Description")]
        [Display(Name = "Role Description")]
        public string RoleDescription { get; set; }

        public List<SelectListItem> ApplicationList { get; set; }

        public CommonViewModel commonDetails { get; set; }
        public string PathToAttachment { get; set; }
        public HttpPostedFile FileUpload { get; set; }
        public string Name1 { get; set; }
    }
}