using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class MaterialReceipt
    {
        public Guid ID { get; set; }
        public Guid SupplierID { get; set; }
        public Guid PurchaseOrderID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string MRNNo { get; set; }
        public string MRNDate { get; set; }
        public string GeneralNotes { get; set; }
        public Common Common { get; set; }

        public Supplier Supplier { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
