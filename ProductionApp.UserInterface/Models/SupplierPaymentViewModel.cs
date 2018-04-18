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
        public string EntryNo { get; set; }
        public Guid? SupplierID { get; set; }
        public string PaymentMode { get; set; }
        public Guid? DepositWithdrawalID { get; set; }
        public string BankCode { get; set; }
        public string PaymentRef { get; set; }
        public DateTime PaymentDate { get; set; }
        public string GeneralNotes { get; set; }
        public decimal? TotalPaidAmt { get; set; }
        public Guid? CreditID { get; set; }
        public string Type { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string ReferenceBank { get; set; }

        public Guid LatestApprovalID { get; set; }
        public int LatestApprovalStatus { get; set; }
        public string ApprovalStatus { get; set; }

        public bool IsFinalApproved { get; set; }

        //Additional properties
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