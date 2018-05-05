using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class ApprovalStatusViewModel
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public CommonViewModel Common { get; set; }
        public List<SelectListItem> StatusSelectList { get; set; }
    }
}