using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialReturnViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Supplier")]
        public Guid SupplierID { get; set; }
        [Display(Name = "Return Slip No#")]
        public string ReturnSlipNo { get; set; }
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }
        [Display(Name = "Billing Address")]
        [DataType(DataType.MultilineText)]
        public string BillAddress { get; set; }
        [Display(Name = "Shipping Address")]
        [DataType(DataType.MultilineText)]
        public string ShippingAddress { get; set; }
        [Display(Name = "General Notes")]
        [DataType(DataType.MultilineText)]
        public string GeneralNotes { get; set; }
        [Display(Name = "Return By")]
        public Guid ReturnBy { get; set; }
        //Additional properties
        public CommonViewModel Common { get; set; }
        [Display(Name = "Return Date")]
        [Required(ErrorMessage = "Return Date is required")]
        public string ReturnDateFormatted { get; set; }
        public string ReturnByEmployeeName { get; set; }
        public string SupplierName { get; set; }
        public bool IsUpdate { get; set; }
        public string DetailJSON { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public MaterialReturnDetailViewModel MaterialReturnDetail { get; set; }
        public List<MaterialReturnDetailViewModel> MaterialReturnDetailList { get; set; }
        public string DetailXML { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
    public class MaterialReturnDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid MaterialReturnID { get; set; }
        [Display(Name = "Material")]
        public Guid MaterialID { get; set; }
        [Display(Name = "Material Description")]
        public string MaterialDesc { get; set; }
        [Display(Name = "Unit")]
        public string UnitCode { get; set; }
        [Display(Name = "Quantity")]
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        [Display(Name = "CGST %")]
        public decimal CGSTPerc { get; set; }
        [Display(Name = "SGST %")]
        public decimal SGSTPerc { get; set; }
        [Display(Name = "IGST %")]
        public decimal IGSTPerc { get; set; }
        [Display(Name = "Tax Type")]
        public string TaxTypeCode { get; set; }
        //additional properties
        public MaterialViewModel Material { get; set; }
        public string TaxTypeDescription { get; set; }
    }
    public class MaterialReturnAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Return By")]
        public Guid ReturnBy { get; set; }
        public string ReturnedByEmployeeName { get; set; }
        public EmployeeViewModel Employee { get; set; }
        [Display(Name = "Supplier")]
        public Guid SupplierID { get; set; }
        public SupplierViewModel Supplier { get; set; }
    }
}