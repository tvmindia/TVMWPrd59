using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialReceiptViewModel
    {
        public Guid ID { get; set; }
        public Guid SupplierID { get; set; }
        public Guid PurchaseOrderID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string MRNNo { get; set; }
        public string MRNDate { get; set; }
        public string GeneralNotes { get; set; }
        public CommonViewModel Common { get; set; }

        public SupplierViewModel Supplier { get; set; }
        public PurchaseOrderViewModel PurchaseOrder { get; set; }
    }
}