using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class ProductCategory
    {
        //additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Common Common { get; set; }
    }
    public class ProductCategoryAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}
