﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class OtherExpense
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
        public bool isFinalApproved { get; set; }
        public decimal OELimit { get; set; }
        //additional fields
        public Common Common { get; set; }
        public ChartOfAccount ChartOfAccount { get; set; }
        public string ExpenseDateFormatted { get; set; }
        public string ChequeDateFormatted { get; set; }
        public string ChequeClearDateFormatted { get; set; }
        public string ApprovalStatus { get; set; }
        public decimal ReversableAmount { get; set; }
        public bool IsReverse { get; set; }
        public bool IsUpdate { get; set; }
        public string Account { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string LogoURL { get; set; }
        public string InvoiceAmountWords { get; set; }
    }
    public class OtherExpenseAdvanceSearch
    {
        public DataTablePaging DataTablePaging { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string AccountCode { get; set; }
        public ChartOfAccount ChartOfAccount { get; set; }
        public int? Status { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
    }
}
