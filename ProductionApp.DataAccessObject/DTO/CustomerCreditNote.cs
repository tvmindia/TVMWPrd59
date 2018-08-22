using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class CustomerCreditNote
    {
        public Guid? ID { get; set; }
        public string CreditNoteNo { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime? CreditNoteDate { get; set; }
        public string CreditNoteDateFormatted { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal AvailableCredit { get; set; }
        public string GeneralNotes { get; set; }
        public string creditAmountFormatted { get; set; }
        public string adjustedAmountFormatted { get; set; }
        public decimal adjustedAmount { get; set; }
      //  public List<SelectListItem> CustomerList { get; set; }
        public Common common { get; set; }
        //  public List<SelectListItem> CreditList { get; set; }

        //additional fields
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Common Common { get; set; }
        public CustomerCreditNoteAdvanceSearch CustomerCreditNoteAdvanceSearch { get; set; }
    }

    public class CustomerCreditNoteAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Guid CustomerID { get; set; }
        public Customer Customer { get; set; }

    }

}
