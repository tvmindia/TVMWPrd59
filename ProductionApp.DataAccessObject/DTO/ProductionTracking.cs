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
        public Guid LineStageDetailID { get; set; }
        public int? AcceptedQty { get; set; }
        public decimal? AcceptedWt { get; set; }
        public int? DamagedQty { get; set; }
        public decimal? DamagedWt { get; set; }
        public Guid ForemanID { get; set; }
        public string Remarks { get; set; }
        public Common Common { get; set; }
        //Additional
        public bool IsUpdate { get; set; }
        public string EntryDateFormatted { get; set; }
        public Product Product { get; set; }
        public SubComponent SubComponent { get; set; }
        public string SearchDetail { get; set; }
        public BOMComponentLineStageDetail BOMComponentLineStageDetail { get; set; }
    }

    public class ProductionTrackingAdvanceSearch
    {

    }
}
