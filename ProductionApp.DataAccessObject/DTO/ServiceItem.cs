using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class ServiceItem
    {
        public Guid ID { get; set; }
        public string ServiceName { get; set; }
        public decimal Rate { get; set; }
        public string SACCode { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Common Common { get; set; }

        public decimal TradeDiscountAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public string TaxTypeCode { get; set; }
        public decimal GrossAmount { get; set; }
    }

    public class ServiceItemAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}
