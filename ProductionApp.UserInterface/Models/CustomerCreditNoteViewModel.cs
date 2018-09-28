using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class CustomerCreditNoteViewModel
    {

        public Guid? ID { get; set; }

        [Display(Name = "Credit Note#")]
        [Required(ErrorMessage = "Please enter Credit Note No")]
        [MaxLength(20)]
        public string CreditNoteNo { get; set; }

        [Display(Name = "Customer Name")]
        [Required(ErrorMessage = "Please select customer")]
        public Guid CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime? CreditNoteDate { get; set; }

        [Display(Name = "Credit Note Date")]
        [Required(ErrorMessage = "Please select credit note date")]
        public string CreditNoteDateFormatted { get; set; }
       
        [Display(Name = "Credit Amount")]
        [Required(ErrorMessage = "Please enter credit amount")]
        [Remote(action: "CheckValue", controller: "CustomerCreditNote", AdditionalFields = "CreditAmount")]
        public decimal CreditAmount { get; set; }
        
        public decimal AvailableCredit { get; set; }

        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        public string creditAmountFormatted { get; set; }
        public string adjustedAmountFormatted { get; set; }
        public decimal adjustedAmount { get; set; }
        public List<SelectListItem> CustomerList { get; set; } 
        public List<SelectListItem> CreditList { get; set; }

        //additional fields
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public CustomerCreditNoteAdvanceSearchViewModel CustomerCreditNoteAdvanceSearch { get; set; }
    }

    public class CustomerCreditNoteAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }

        [Display(Name = "Customer")]
        public Guid CustomerID { get; set; }

        public CustomerViewModel Customer  { get; set; }

    }
}