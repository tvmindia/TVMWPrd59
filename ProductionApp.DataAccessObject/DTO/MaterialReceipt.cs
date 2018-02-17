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
        public DateTime MRNDate { get; set; }
        public string MRNDateFormatted { get; set; }
        public string GeneralNotes { get; set; }
        public Common Common { get; set; }

        public List<MaterialReceipt> MaterialReceiptList { get; set; }

        public MaterialReceiptDetail MaterialReceiptDetail { get; set; }
        public RawMaterial RawMaterial { get; set; }
        public string SupplierName { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
    }

    public class MaterialReceiptDetail
    {
        public Guid ID { get; set; }
        public Guid HeaderID { get; set; }
        public Guid MaterialID { get; set; }
        public string MaterialDesc { get; set; }
        public string UnitCode { get; set; }
        public decimal Qty { get; set; }
        public Common Common { get; set; }
    }
    
}
