using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class BankViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Opening { get; set; }
        public decimal ActualODLimit { get; set; }
        public decimal DisplayODLimit { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public BankAdvanceSearchViewModel BankAdvanceSearch { get; set; }
    }
    public class BankAdvanceSearchViewModel
    {
        public string SearchTerm { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDir { get; set; }
    }

}