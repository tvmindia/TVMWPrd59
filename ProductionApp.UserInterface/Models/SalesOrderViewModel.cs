using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class SalesOrderViewModel
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
        public CommonViewModel Common { get; set; }
        public List<SalesOrderDetailViewModel> SalesOrderDetailList { get; set; }


    }
    public class SalesOrderDetailViewModel
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
        public CommonViewModel Common { get; set; }

    }
}