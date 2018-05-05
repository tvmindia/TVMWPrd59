using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class OtherExpenseViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Entry No")]
        public string EntryNo { get; set; }
        [Display(Name = "Expense Date")]
        public DateTime ExpenseDate { get; set; }
        [Display(Name = "Cheque Date")]
        public DateTime? ChequeDate { get; set; }
        [Display(Name = "Account Head")]
        [Required(ErrorMessage = "Account Head required")]
        public string AccountCode { get; set; }
        [Display(Name = "Subtype (Employee,Other,etc..)")]
        public string AccountSubHead { get; set; }
        [Display(Name = "Payment Mode")]
        [Required(ErrorMessage = "Payment Mode required")]
        public string PaymentMode { get; set; }
        public Guid? DepositWithdrawalID { get; set; }
        [Display(Name = "Bank")]
        public string BankCode { get; set; }
        [Display(Name = "Payment Reference")]
        public string ExpneseRef { get; set; }
        [Display(Name = "General Notes")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Amount required")]
        public decimal Amount { get; set; }
        [Display(Name = "Reversal Of")]
        public string ReversalRef { get; set; }
        [Display(Name = "Cheque Cleared Date")]
        public DateTime? ChequeClearDate { get; set; }
        public decimal RequiredApprovalLimit { get; set; }
        public Guid? LatestApprovalID { get; set; }
        public int LatestApprovalStatus { get; set; }
        public bool isFinalApproved { get; set; }
        //additional fields
        public CommonViewModel Common { get; set; }
        public ChartOfAccountViewModel ChartOfAccount { get; set; }
        public List<SelectListItem> SelectList { get; set; }
        [Required(ErrorMessage = "Expense Date required")]
        public string ExpenseDateFormatted { get; set; }
        public string ChequeDateFormatted { get; set; }
        public string ChequeClearDateFormatted { get; set; }
        public string ApprovalStatus { get; set; }
        public decimal ReversableAmount { get; set; }
        [Display(Name = "Regular/Reversal")]
        public bool IsReverse { get; set; }
        public bool IsUpdate { get; set; }
        public string Account { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }

    }
    public class OtherExpenseAdvanceSearchViewModel
    {
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Account")]
        public ChartOfAccountViewModel ChartOfAccount { get; set; }
        [Display(Name = "Approval Status")]
        public ApprovalStatusViewModel ApprovalStatus { get; set; }
    }
}