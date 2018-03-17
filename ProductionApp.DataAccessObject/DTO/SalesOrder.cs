using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class SalesOrder
    {
        public Guid ID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CustomerID { get; set; }
        public Guid SalesPerson { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
       // public string OrderStatus { get; set; }
        public string Remarks { get; set; }

        //additional properties
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string OrderDateFormatted { get; set; }
        public string ExpectedDeliveryDateFormatted { get; set; }
        public string DetailXML { get; set; }
        public string CustomerName { get; set; }
        public Common Common { get; set; }
        public List<SalesOrderDetail> SalesOrderDetailList { get; set; }


    }
    public class SalesOrderDetail
    {
        public Guid ID { get; set; }
        public Guid SalesOrderID { get; set; }
        public Guid ProductID { get; set; }
        public string TaxTypeCode { get; set; }
        public decimal Quantity { get; set; }
        public string UnitCode { get; set; }
        public decimal Rate { get; set; }
        public decimal TradeDiscountAmount { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public decimal? DiscountPercent { get; set; }

        //additional properties
        public decimal TaxAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string TaxTypeDescription { get; set; }

        public decimal PrevPkgQty { get; set; }
        public decimal PkgWt { get; set; }
        public decimal CurrentPkgQty { get; set; }
        public string ExpectedDeliveryDateFormatted { get; set; }
        public Common Common { get; set; }
        public Product Product { get; set; }

    }

    public class SalesOrderAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Guid CustomerID { get; set; }
        public Guid EmployeeID { get; set; }

        public DataTablePaging DataTablePaging { get; set; }

    }
}
