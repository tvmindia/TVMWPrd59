using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class Material
    {
        public Guid ID { get; set; }
        public string MaterialCode { get; set; }
        public decimal Rate { get; set; }
        public string MaterialTypeCode { get; set; }
        public string Description { get; set; }
        public string UnitCode { get; set; }
        public decimal ReorderQty { get; set; }
        public decimal CurrentStock { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal WeightInKG { get; set; }
        public decimal CostPrice { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Common Common { get; set; }
        public Unit Unit { get; set; }
        public MaterialType MaterialType { get; set; }
    }
    public class MaterialAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Unit Unit { get; set; }
        public MaterialType MaterialType { get; set; }
    }


    public class MaterialSummary
    {
        public string MaterialType { get; set; }
        public Decimal Value { get; set; }
        public string Color { get; set; }
        public string ValueFormatted { get; set; }
    }
}
