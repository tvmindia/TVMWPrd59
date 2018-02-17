using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class RawMaterial
    {
        public Guid ID { get; set; }
        public string MaterialCode { get; set; }
        public decimal Rate { get; set; }
        public string MaterialType { get; set; }
        public string Description { get; set; }
        public string UnitCode { get; set; }
        public decimal ReorderQty { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public string CurrentStock { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Common Common { get; set; }

    }
    public class RawMaterialAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}
