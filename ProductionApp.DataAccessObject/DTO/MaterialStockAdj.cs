using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class MaterialStockAdj
    {
        public Guid ID { get; set; }
        public Guid AdjustedBy { get; set; }
        public Guid? LatestApprovalID { get; set; }
        public DateTime AdjustmentDate { get; set; }
        public string Remarks { get; set; }
        public int LatestApprovalstatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public Common Common { get; set; }

        //Additional Fields
        public string AdjustmentDateFormatted { get; set; }
        public Employee Employee { get; set; }
        public RawMaterial RawMaterial { get; set; }
        public MaterialStockAdjDetail MaterialStockAdjDetail { get; set; }

        public List<MaterialStockAdj> MaterialStockAdjList { get; set; }
    }

    public class MaterialStockAdjDetail
    {
        public Guid ID { get; set; }
        public Guid AdjustmentBy { get; set; }
        public Guid MaterialID { get; set; }
        public decimal Qty { get; set; }
        public string Remarks { get; set; }
        public Common Common { get; set; }
    }
}
