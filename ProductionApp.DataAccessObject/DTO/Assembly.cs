using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class Assembly
    {
        public Guid ID { get; set; }
        public string EntryNo { get; set; }
        public DateTime AssemblyDate { get; set; }
        public Guid AssembleBy { get; set; }
        public Guid ProductID { get; set; }
        public decimal Qty { get; set; }
        //Additional properties
        public Common Common { get; set; }
        public string AssemblyDateFormatted { get; set; }
        public Employee Employee { get; set; }
        public Product Product { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
    public class AssemblyAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Guid ProductID { get; set; }
        public Guid AssembleBy { get; set; }
        public Employee Employee { get; set; }
        public Product Product { get; set; }
    }
}
