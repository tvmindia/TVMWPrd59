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
        public Guid LatestApprovalID { get; set; }
        public string Date { get; set; }
        public string Remarks { get; set; }
        public int LatestApprovalstatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public CommonViewModel common { get; set; }
    }
}