using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialReceiptViewModel
    {
        public Guid ID { get; set; }
        public Guid SupplierID { get; set; }
        public Guid PurchaseOrderID { get; set; }
        [Display(Name = "Purchase Order Number")]
        public string PurchaseOrderNo { get; set; }
        [Display(Name ="Receipt No")]
        public string ReceiptNo { get; set; }
        [Display(Name = "Receipt Date")]
        public DateTime ReceiptDate { get; set; }
        [Display(Name = "General Notes")]
        public string GeneralNotes { get; set; }
        public CommonViewModel Common { get; set; }

        //Additional Fields
        public string ReceiptDateFormatted { get; set; }
        public MaterialReceiptDetailViewModel MaterialReceiptDetail { get; set; }
        [Display(Name = "Raw Material")]
        public RawMaterialViewModel RawMaterial { get; set; }
        [Display(Name ="Supplier")]
        public SupplierViewModel Supplier { get; set; }

        public List<MaterialReceiptViewModel> MaterialReceiptList { get; set; }
    }

    public class MaterialReceiptDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid HeaderID { get; set; }
        [Display(Name = "Material")]
        public Guid MaterialID { get; set; }
        [Display(Name = "Description")]
        public string MaterialDesc { get; set; }
        [Display(Name = "Unit Code")]
        public string UnitCode { get; set; }
        [Display(Name = "Quantity")]
        public decimal Qty { get; set; }
        public CommonViewModel Common { get; set; }
    }
}