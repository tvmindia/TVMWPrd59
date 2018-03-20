using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class PaymentTermViewModel
    {
        [Required(ErrorMessage = "Payment Term is missing")]
        [Display(Name = "Payment Term")]
        public string Code { get; set; }
        public string Description { get; set; }
        public int NoOfDays { get; set; }
        public CommonViewModel Common { get; set; }

    }
}