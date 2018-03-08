using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class BillOfMaterialViewModel
    {
        Guid ID { get; set; }
        string Description { get; set; }
        Guid ProductID { get; set; }
        CommonViewModel Common { get; set; }
    }
    public class BillOfMaterialAdvanceSearchViewModel
    {
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class BillOfMaterialDetailViewModel
    {
        Guid ID { get; set; }
        Guid BillOfMaterialID { get; set; }
        Guid ComponentID { get; set; }
        decimal Qty { get; set; }
        CommonViewModel Common { get; set; }
    }
}