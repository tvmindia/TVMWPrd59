using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class MastersCountViewModel
    {
        public int Approver { get; set; }
        public int Bank { get; set; }
        public int ChartOfAccounts{ get; set; }
        public int Customer { get; set; }
        public int Employee { get; set; }
        public int Material { get; set; }
        public int Product { get; set; }
        public int ProductCategory { get; set; }
        public int ProductStage { get; set; }
        public int SubComponents { get; set; }
        public int Supplier { get; set; }
        public int ServiceItem { get; set; }
        public List<MastersCountViewModel> MasterCountList { get; set; }
    }
}