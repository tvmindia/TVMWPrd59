using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class CustomerPaymentViewModel
    {
        public Guid ID { get; set; }
        public string EntryNo { get; set; }
        public Guid CustomerID { get; set; }
        public string PaymentMode { get; set; }
        public Guid DepositWithdrawalID { get; set; }
        public string BankCode { get; set; }
        public string PaymentRef { get; set; }
        public DateTime PaymentDate { get; set; }
        public string GeneralNotes { get; set; }
        public decimal TotalRecievedAmt { get; set; }
        public Guid CreditID { get; set; }
        public string Type { get; set; }
        public DateTime ChequeDate { get; set; }
        public string ReferenceBank { get; set; }

        //Additional properties
        public string PaymentDateFormatted { get; set; }
        public string ChequeDateFormatted { get; set; }
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
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}