using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class OtherIncomeViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Entry No")]
        public string EntryNo { get; set; }
        [Display(Name = "Income Date")]       
        public DateTime IncomeDate { get; set; }
        [Display(Name = "Cheque Date")]
        public DateTime? ChequeDate { get; set; }
        [Display(Name = "Account Code")]
        [Required(ErrorMessage = "Account Code Required")]
        public string AccountCode { get; set; }
        [Display(Name = "Account Sub Head")]
        public string AccountSubHead { get; set; }
        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; }
        //[Display(Name = "IncomeDate")]
        public Guid DepositWithdrawalID { get; set; }      
        [Display(Name = "Bank Code")]        
        public string BankCode { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Amount Required and must be greater than zero")]        
        public decimal Amount { get; set; }
        [Display(Name = "Reference Bank")]
        public string ReferenceBank { get; set; }
        [Display(Name = "Reference No.")]
        public string PaymentRef { get; set; }
        public CommonViewModel Common { get; set; }
        //ADDITIONAL FIELDS
        [Required(ErrorMessage = "Income Date Required")]
        public string IncomeDateFormatted { get; set; }
        public string ChequeDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }

    public class OtherIncomeAdvanceSearchViewModel
    {
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Select Account")]
        public ChartOfAccountViewModel ChartOfAccount { get; set; }
        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; }
    }
}