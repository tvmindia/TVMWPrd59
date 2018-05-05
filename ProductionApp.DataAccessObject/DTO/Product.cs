using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class Product
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string UnitCode { get; set; }
        public string ProductCategoryCode { get; set; }
        public decimal? ReorderQty { get; set; }
        //public string Category { get; set; }
        public decimal Rate { get; set; }
        public decimal? OpeningStock { get; set; }
        public decimal CurrentStock { get; set; }
        public string HSNNo { get; set; }
        public decimal? WeightInKG { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? CostPricePerPiece { get; set; }
        public decimal? SellingPriceInKG { get; set; }
        public decimal? SellingPricePerPiece { get; set; }
        public bool IsInvoiceInKG { get; set; }
        public string Type { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Common Common { get; set; }
        public Unit Unit { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public Customer Customer { get; set; }
    }
    public class ProductAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public Unit Unit { get; set; }
        public ProductCategory ProductCategory { get; set; }
    }


    public class FinishedGoodSummary
    {
        public string Category { get; set; }
        public Decimal Value { get; set; }
        public string Color { get; set; }
        public string ValueFormatted { get; set; }
    }
}
