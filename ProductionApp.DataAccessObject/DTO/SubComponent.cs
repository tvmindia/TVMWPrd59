using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class SubComponent
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal OpeningQty { get; set; }
        public decimal CurrentQty { get; set; }
        public string UnitCode { get; set; }
        public decimal WeightInKG { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Common Common { get; set; }
        public Unit Unit { get; set; }
    }
    public class SubComponentAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}
