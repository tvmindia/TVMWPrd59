using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class ApprovalStatusViewModel
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public CommonViewModel Common { get; set; }
    }
}