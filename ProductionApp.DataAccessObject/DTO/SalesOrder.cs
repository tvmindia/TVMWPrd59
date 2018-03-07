using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    class SalesOrder
    {
        public Guid ID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid CustomerID { get; set; }
        public Guid SalesPerson { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string Remarks { get; set; }
        
        //additional properties
        public string OrderDateFormatted { get; set; }
        public string ExpectedDeliveryDateFormatted { get; set; }
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

        //additional properties
        public string ExpectedDeliveryDateFormatted { get; set; }
        public Common Common { get; set; }

    }
}
