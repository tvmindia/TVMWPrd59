using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class SupplierViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        [Display(Name = "Email")]
        public string ContactEmail { get; set; }
        [Display(Name = "Title")]
        public string ContactTitle { get; set; }
        public string Product { get; set; }
        public string Website { get; set; }
        public string LandLine { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        [Display(Name = "Other Phone Nos")]
        public string OtherPhoneNos { get; set; }
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Payment Term Code")]
        public string PaymentTermCode { get; set; }
        [Display(Name = "Tax Reg No")]
        public string TaxRegNo { get; set; }
        [Display(Name = "PAN No")]
        public string PANNo { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }
    public class SupplierAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}