using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class SupplierPaymentViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Entry No")]
        public string EntryNo { get; set; }
        [Display(Name = "Supplier")]
        [Required(ErrorMessage = "Invoice No is missing")]
        public Guid? SupplierID { get; set; }
        [Display(Name = "Payment Mode")]
        [Required(ErrorMessage = "Payment Mode is missing")]
        public string PaymentMode { get; set; }
        public Guid? DepositWithdrawalID { get; set; }
        [Display(Name = "Bank Code")]
        public string BankCode { get; set; }
        [Display(Name = "Payment Reference")]
        public string PaymentRef { get; set; }
        public DateTime PaymentDate { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        [Display(Name = "Total Paid Amount")]
        public decimal? TotalPaidAmt { get; set; }
        public Guid? CreditID { get; set; }
        public string Type { get; set; }
        [Display(Name = "Cheque Date")]
        public DateTime? ChequeDate { get; set; }
        [Display(Name = "Reference Bank")]
        public string ReferenceBank { get; set; }

        public Guid LatestApprovalID { get; set; }
        public int LatestApprovalStatus { get; set; }
        public string ApprovalStatus { get; set; }

        public bool IsFinalApproved { get; set; }

        //Additional properties
        [Display(Name = "Payment Date")]
        [Required(ErrorMessage = "Payment Date is missing")]
        public string PaymentDateFormatted { get; set; }
        public string ChequeDateFormatted { get; set; }
        public string SupplierName { get; set; }
        public decimal? AdvanceAmount { get; set; }
        public string CreditNo { get; set; }
        public bool IsUpdate { get; set; }
        public string DetailJSON { get; set; }
        public Guid hdnFileID { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public SupplierViewModel Supplier { get; set; }
        public SupplierPaymentDetailViewModel SupplierPaymentDetail { get; set; }
        public List<SupplierPaymentDetailViewModel> SupplierPaymentDetailList { get; set; }
    }
    public class SupplierPaymentDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid PaymentID { get; set; }
        public Guid InvoiceID { get; set; }
        public decimal PaidAmount { get; set; }

    }
    public class SupplierPaymentAdvanceSearchViewModel
    {
       
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }

        [Display(Name = "Supplier")]
        public Guid SupplierID { get; set; }
        public SupplierViewModel Supplier { get; set; }

    }
}