using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class PurchaseInvoiceViewModel
    {
    }
    public class PurchaseSummaryViewModel
    {
        public string Month { get; set; }
        public int MonthCode { get; set; }
        public int Year { get; set; }
        public decimal Purchase { get; set; }
    }
}