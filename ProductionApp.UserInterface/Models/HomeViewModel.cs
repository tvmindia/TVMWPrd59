using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class HomeViewModel
    {
        public AMCSysModuleViewModel SysModuleVMObj { get; set; }
    }


    public class SalesSummaryList {
        public List<SalesSummaryViewModel> SalesSmryList  { get;set;}
    }

    public class PurchaseSummaryList
    {
        public List<PurchaseSummaryViewModel> PurchaseSmryList { get; set; }
    }

    public class IncomeExpenseSummaryListViewModel
    {
        public List<IncomeExpenseSummaryViewModel> IncomeExpenseList { get; set; }
    }

    public class IncomeExpenseSummaryViewModel
    {
        public string Month { get; set; }
        public int MonthCode { get; set; }
        public int Year { get; set; }
        public decimal InAmount { get; set; }
        public decimal ExAmount { get; set; }
    }

    public class ProductionSummaryViewModel
    {
        public string Month { get; set; }
        public int MonthCode { get; set; }
        public int Year { get; set; }
        public decimal Material { get; set; }
        public decimal Product { get; set; }
        public decimal InProduction { get; set; }
        public decimal Damage { get; set; }
    }


    public class PurchaseInvoiceSummaryViewModel
    {

    }

    public class InventorySummaryViewModel
    {
        public List<MaterialSummaryViewModel> MaterialSummaryList { get; set; }
        public decimal TotalValue { get; set; }
        public String TotalValueFormatted { get; set; }
    }


    public class FGInventorySummaryViewModel
    {
        public List<FinishedGoodSummaryViewModel> FinishedGoodsSummaryList { get; set;  }
        public decimal TotalValue { get; set; }
        public String TotalValueFormatted { get; set; }
    }

    public class ProductionSummaryListViewModel
    {
      public  List<ProductionSummaryViewModel> ProductionSummaryList { get; set; }

    }

    public class DispatchSummaryViewModel
    {

    }

}