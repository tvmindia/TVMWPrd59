using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class BillOfMaterial
    {
        Guid ID { get; set; }
        string Description { get; set; }
        Guid ProductID { get; set; }
        Common Common { get; set; }
    }
    public class BillOfMaterialAdvanceSearch
    {
        public DataTablePaging DataTablePaging { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class BillOfMaterialDetail
    {
        Guid ID { get; set; }
        Guid BillOfMaterialID { get; set; }
        Guid ComponentID { get; set; }
        decimal Qty { get; set; }
        Common Common { get; set; }
    }
}
