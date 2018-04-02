﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class CustomerPaymentViewModel
    {
        public Guid ID { get; set; }
        public string EntryNo { get; set; }
        [Display(Name = "Customer")]
        public Guid CustomerID { get; set; }
        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; }
        public Guid DepositWithdrawalID { get; set; }
        [Display(Name = "Deposit To")]
        public string BankCode { get; set; }
        [Display(Name = "Reference No.")]
        public string PaymentRef { get; set; }
        public DateTime PaymentDate { get; set; }
        [DataType(DataType.MultilineText)]
        public string GeneralNotes { get; set; }
        [Display(Name = "Amount Received")]
        public decimal TotalRecievedAmt { get; set; }
        public Guid CreditID { get; set; }
        public string Type { get; set; }
        [Display(Name = "Cheque Date")]
        public DateTime ChequeDate { get; set; }
        [Display(Name = "Reference Bank")]
        public string ReferenceBank { get; set; }

        //Additional properties
        public string PaymentDateFormatted { get; set; }
        public string ChequeDateFormatted { get; set; }
        public CustomerPaymentDetail CustomerPaymentDetail { get; set; }
        public List<CustomerPaymentDetail> CustomerPaymentDetailList { get; set; }
        public string CustomerName { get; set; }
        public decimal AdvanceAmount { get; set; }
        public string CreditNo { get; set; }
        public string DetailXML { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
    public class CustomerPaymentDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid PaymentID { get; set; }
        public Guid InvoiceID { get; set; }
        public decimal PaidAmount { get; set; }
    }
    public class CustomerPaymentAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Customer")]
        public Guid CustomerID { get; set; }
        [Display(Name = "Payment Mode")]
        public string PaymentMode { get; set; }
    }
}