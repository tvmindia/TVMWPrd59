using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class MyApprovalsViewModel
    {
        public string URL { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public List<MaterialStockAdjViewModel> MaterialStockAdjList { get; set; }
    }
}