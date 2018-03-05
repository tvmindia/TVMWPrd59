using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class SalesInvoiceViewModel
    {
    }

    public class SalesInvoiceSummaryViewModel
    {
        
    }

    public class SalesSummaryViewModel
    {
        public string Month { get; set; }
        public int MonthCode { get; set; }
        public int Year { get; set; }
        public decimal Sales { get; set; }
    }
}