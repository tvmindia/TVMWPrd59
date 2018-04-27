using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class OtherIncomeViewModel
    {
        public Guid ID { get; set; }
        public string EntryNo { get; set; }
        public DateTime IncomeDate { get; set; }
        public DateTime? ChequeDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountSubHead { get; set; }
        public string PaymentMode { get; set; }
        public Guid DepositWithdrawalID { get; set; }
        public string BankCode { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string ReferenceBank { get; set; }
        public CommonViewModel Common { get; set; }
        //ADDITIONAL FIELDS
        public string IncomeDateFormatted { get; set; }
        public string ChequeDateFormatted { get; set; }
    }

    public class OtherIncomeAdvanceSearchViewModel
    {
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public ChartOfAccountViewModel ChartOfAccount { get; set; }
        public string PaymentMode { get; set; }
    }
}