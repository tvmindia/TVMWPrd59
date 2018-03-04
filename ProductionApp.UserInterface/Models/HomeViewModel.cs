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

    public class IncomeExpenseSummaryViewModel
    {

    }

   
    public class PurchaseInvoiceSummaryViewModel
    {

    }

    public class InventorySummaryViewModel
    {

    }

    public class ProductionSummaryViewModel
    {

    }

    public class DispatchSummaryViewModel
    {

    }

}