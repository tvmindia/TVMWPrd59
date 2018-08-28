using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class SupplierCreditNote
    {
        public Guid? ID { get; set; }
        public string CreditNoteNo { get; set; }
        public Guid SupplierID { get; set; }
        public string SupplierName { get; set; }
        public DateTime? CreditNoteDate { get; set; }
        public string CreditNoteDateFormatted { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal AvailableCredit { get; set; }
        public string GeneralNotes { get; set; }
        public string creditAmountFormatted { get; set; }
        public string adjustedAmountFormatted { get; set; }
        public decimal adjustedAmount { get; set; }
        //  public List<SelectListItem> SupplierList { get; set; }
        public Common commonObj { get; set; }
        //  public List<SelectListItem> CreditList { get; set; }

        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Common Common { get; set; }
        public SupplierCreditNoteAdvanceSearch SupplierCreditNoteAdvanceSearch { get; set; }
    }

    public class SupplierCreditNoteAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Guid SupplierID { get; set; }
        public Supplier Supplier { get; set; }
    }
}
