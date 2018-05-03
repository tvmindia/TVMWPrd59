using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class OtherIncome
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
        public string PaymentRef { get; set; }
        public Common Common { get; set; }
        //ADDITIONAL FIELDS
        public string IncomeDateFormatted { get; set; }
        public string ChequeDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
    }

    public class OtherIncomeAdvanceSearch
    {
        public DataTablePaging DataTablePaging { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public ChartOfAccount ChartOfAccount { get; set; }
        public string PaymentMode { get; set; }
    }
}
