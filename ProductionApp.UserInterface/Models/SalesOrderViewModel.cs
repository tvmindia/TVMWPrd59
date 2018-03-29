using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class SalesOrderViewModel
    {
        public Guid ID { get; set; }
        public string OrderNo { get; set; }
      
        public DateTime OrderDate { get; set; }
        [Display(Name = "Customer")]
        [Required(ErrorMessage = "Customer required")]
        public Guid CustomerID { get; set; }
        [Display(Name = "Reference Customer")]
        public Guid ReferenceCustomer { get; set; }
        [Display(Name = "Sales Person")]
      //  [Required(ErrorMessage = "Sales Person required")]
        public Guid SalesPerson { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        //public string OrderStatus { get; set; }
        public string Remarks { get; set; }

        //additional properties
        public Guid EmployeeID { get; set; }
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public int TotalCount { get; set; }
        public string DetailJSON { get; set; }
        [Display(Name = "Sales Person")]
        public string CustomerName { get; set; }
        public string ReferenceCustomerName { get; set; }
        public string SalesPersonName { get; set; }

        public int FilteredCount { get; set; }
        [Display(Name = "Order Date")]
        [Required(ErrorMessage = "Order Date required")]
        public string OrderDateFormatted { get; set; }
        [Display(Name = "Expected Delivery Date")]
        [Required(ErrorMessage = "Expected Delivery Date required")]
        public string ExpectedDeliveryDateFormatted { get; set; }
        public CommonViewModel Common { get; set; }
        public List<SalesOrderDetailViewModel> SalesOrderDetailList { get; set; }
        public SalesOrderDetailViewModel SalesOrderDetail { get; set; }
        public List<SelectListItem> SelectList { get; set; }


    }
    public class SalesOrderDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid SalesOrderID { get; set; }
        public Guid ProductID { get; set; }
        [Display(Name = "Tax %")]

        public string TaxTypeCode { get; set; }
        public decimal Quantity { get; set; }
        public string UnitCode { get; set; }
        public decimal Rate { get; set; }
        [Display(Name = "Discount Amount")]
        public decimal TradeDiscountAmount { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        [Display(Name = "Discount %")]
        public decimal? DiscountPercent { get; set; }
        
        //additional properties
        public decimal PrevPkgQty { get; set; }
        public decimal PkgWt { get; set; }
        public decimal CurrentPkgQty { get; set; }
        [Display(Name = "Tax Amount")]
        public decimal TaxAmount { get; set; }
        [Display(Name = "Taxable Amount")]
        public decimal TaxableAmount { get; set; }
        [Display(Name = "Net Amount")]
        public decimal NetAmount { get; set; }
        [Display(Name = "Gross Amount")]
        public decimal GrossAmount { get; set; }
        public string TaxTypeDescription { get; set; }

        [Display(Name = "Expected Delivery Date")]
        public string ExpectedDeliveryDateFormatted { get; set; }
        public CommonViewModel Common { get; set; }
        public ProductViewModel Product { get; set; }

    }
    public class SalesOrderAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Sales Person")]
        public Guid EmployeeID { get; set; }
        [Display(Name = "Customer")]
        public Guid CustomerID { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }

    }
}