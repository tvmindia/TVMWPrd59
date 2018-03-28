using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class CustomerViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Company Name required")]
        public string CompanyName { get; set; }
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        [Display(Name = "Email")]
        [RegularExpression(@"^((\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)\s*[;,.]{0,1}\s*)+$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string ContactEmail { get; set; }
        [Display(Name = "Title")]
        public string ContactTitle { get; set; }
        public string Website { get; set; }
        public string LandLine { get; set; }
        [RegularExpression(@"^((\+91-?)|0)?[0-9]{10}$", ErrorMessage = "Entered phone format is not valid.")]
        public string Mobile { get; set; }
        public string Fax { get; set; }
        [Display(Name = "Other Phone Nos")]
        public string OtherPhoneNumbers { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Billing Address")]
        public string BillingAddress { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        [Display(Name = "Payment Term Code")]
        public string PaymentTermCode { get; set; }
        [Display(Name = "Tax Reg No")]
        public string TaxRegNo { get; set; }
        [Display(Name = "PAN No")]
        public string PANNo { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        //additional fields
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public List<SelectListItem> SelectList { get; set; }
        public CustomerAdvanceSearchViewModel CustomerAdvanceSearch { get; set; }
        public List<SelectListItem> ContactTitleList { get; set; }
    }
    public class CustomerAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
    public class ContactTitleViewModel
    {
        public string Title { get; set; }
    }
}