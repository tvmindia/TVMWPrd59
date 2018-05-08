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
        public Guid? PurchaseOrderID { get; set; }
        public string PurchaseOrderNo { get; set; }
        public string ReceiptNo { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string GeneralNotes { get; set; }
        public Common Common { get; set; }

        //Additional Fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string ReceiptDateFormatted { get; set; }
        public MaterialReceiptDetail MaterialReceiptDetail { get; set; }
        public Supplier Supplier { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public bool IsUpdate { get; set; }
        public string DetailXML { get; set; }

        public List<MaterialReceipt> MaterialReceiptList { get; set; }
        public List<MaterialReceiptDetail> MaterialReceiptDetailList { get; set; }
    }

    public class MaterialReceiptDetail
    {
        public Guid ID { get; set; }
        public Guid MaterialReceiptID { get; set; }
        public Guid MaterialID { get; set; }
        public string MaterialDesc { get; set; }
        public string UnitCode { get; set; }
        public decimal Qty { get; set; }
        public Common Common { get; set; }

        //Additional Fields
        public Material Material { get; set; }
        public Unit Unit { get; set; }
    }

    public class MaterialReceiptAdvanceSearch
    {
        public DataTablePaging DataTablePaging { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Guid SupplierID { get; set; }
        public Supplier Supplier { get; set; }
        public Guid PurchaseOrderID { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
    }
}
