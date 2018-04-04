using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class ProductionTracking
    {
        public Guid ID { get; set; }
        public DateTime EntryDate { get; set; }
        public string ProductionRefNo { get; set; }
        public Guid ProductID { get; set; }
        public Guid LineDetailID { get; set; }
        public int AcceptedQty { get; set; }
        public decimal AcceptedWt { get; set; }
        public int DamagedQty { get; set; }
        public decimal DamagedWt { get; set; }
        public Guid ForemanID { get; set; }
        public string Remarks { get; set; }
        public Common Common { get; set; }
        //Additional
        public bool IsUpdate { get; set; }
        public string EntryDateFormatted { get; set; }
        public Product Product { get; set; }
        public BOMComponentLine BOMComponentLine { get; set; }
        public Stage Stage { get; set; }
        public SubComponent SubComponent { get; set; }
        public string SearchValue { get; set; }
        public string SearchText { get; set; }
        public BOMComponentLineStageDetail BOMComponentLineStageDetail { get; set; }
        public ProductionTrackingSearch ProductionTrackingSearch { get; set; }
    }

    public class ProductionTrackingAdvanceSearch
    {

    }

    public class ProductionTrackingSearch
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
