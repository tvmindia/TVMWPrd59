using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class OtherExpenseViewModel
    {
        public Guid ID { get; set; }
        public string EntryNo { get; set; }
        public DateTime ExpenseDate { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountSubHead { get; set; }
        public string PaymentMode { get; set; }
        public Guid? DepositWithdrawalID { get; set; }
        public string BankCode { get; set; }
        public string ExpneseRef { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string ReversalRef { get; set; }
        public DateTime? ChequeClearDate { get; set; }
        public decimal RequiredApprovalLimit { get; set; }
        public Guid? LatestApprovalID { get; set; }
        public int LatestApprovalStatus { get; set; }
        public Boolean isFinalApproved { get; set; }
        //additional fields
        public CommonViewModel Common { get; set; }
        public string ExpenseDateFormatted { get; set; }
        public string ChequeDateFormatted { get; set; }
        public string ChequeClearDateFormatted { get; set; }
    }
    public class OtherExpenseAdvanceSearchViewModel
    {
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public ChartOfAccountViewModel ChartOfAccount { get; set; }
    }
}