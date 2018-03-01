using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class UserViewModel
    {
        public Guid? ID { get; set; }

        [Required(ErrorMessage = "Please enter user name")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter login name")]
        [Display(Name = "Login Name")]
        public string LoginName { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }

        [Required(ErrorMessage = "Please enter email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{6,}$", ErrorMessage = "should have minimum 6 Char, one alphabet,one numeric and one special character")]
        [StringLength(250, MinimumLength = 6, ErrorMessage = "{0} should be minimum 6 Char")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{6,}$", ErrorMessage = "should have minimum 6 Char, one alphabet,one numeric and one special character")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        [StringLength(250, MinimumLength = 6, ErrorMessage = "{0} should be minimum 6 Char")]
        public string ConfirmPassword { get; set; }
        public string ResetLink { get; set; }
        public DateTime LinkExpiryTime { get; set; }

        public string RoleCSV { get; set; }
        public string RoleIDCSV { get; set; }
        public Guid? AppID { get; set; }
        [Required(ErrorMessage ="Please select user")]
        public Guid? UserID { get; set; }
        [Display(Name = "Select Roles")]
        public List<RolesViewModel> RoleList { get; set; }

        public CommonViewModel commonDetails { get; set; }
        public List<SelectListItem> userList { get; set; }
        public List<SelectListItem> ApplicationList { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter login name")]
        //[Display(Name = "Login Name")]
        [StringLength(250)]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        //[Display(Name = "Password")]
        [StringLength(250)]
        public string Password { get; set; }

        public bool IsFailure { get; set; }
        public string Message { get; set; }
    }
}