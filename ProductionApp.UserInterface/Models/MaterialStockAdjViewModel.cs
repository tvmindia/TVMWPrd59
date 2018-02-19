using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialStockAdjViewModel
    {
        public Guid ID { get; set; }
        public Guid AdjustedBy { get; set; }
        public Guid? LatestApprovalID { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public string Remarks { get; set; }
        public int LatestApprovalstatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public CommonViewModel Common { get; set; }

        //Additional Fields Adjustment
        public string AdjustmentDateFormatted { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public RawMaterialViewModel RawMaterial { get; set; }
        public MaterialStockAdjDetailViewModel MaterialStockAdjDetail { get; set; }

        public List<MaterialStockAdjViewModel> MaterialStockAdjList { get; set; }
    }

    public class MaterialStockAdjDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid AdjustmentBy { get; set; }
        public Guid MaterialID { get; set; }
        public decimal Qty { get; set; }
        public string Remarks { get; set; }
        public CommonViewModel Common { get; set; }
    }
}