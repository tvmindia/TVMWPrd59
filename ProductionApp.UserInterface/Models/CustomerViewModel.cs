using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class CustomerViewModel
    {
        public Guid ID { get; set; }
        public string CompanyName { get; set; }

        public CommonViewModel Common { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }
}