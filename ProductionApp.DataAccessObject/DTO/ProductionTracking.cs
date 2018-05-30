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
        public DateTime PostedDate { get; set; }
        public string PostedBy { get; set; }
        public Common Common { get; set; }
            
        //Additional
        public bool IsUpdate { get; set; }
        public string EntryDateFormatted { get; set; }
        public string SearchDetail { get; set; }
        public string PostedDateFormatted { get; set; }
        public int PreviousQty { get; set; }
        public int TotalQty { get; set; }
        public bool IsValid { get; set; }
        public int SlNo { get; set; }
        public string ErrorMessage { get; set; }

        public Product Product { get; set; }
        public SubComponent SubComponent { get; set; }
        public Product OutputComponent { get; set; }
        public Stage Stage { get; set; }
        public Product Component { get; set; }
        public Employee Employee { get; set; }
        public BOMComponentLineStageDetail BOMComponentLineStageDetail { get; set; }

        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<ProductionTracking> ProductionTrackingList { get; set; }
        public string BaseURL { get; set; }
    }

    public class ProductionTrackingAdvanceSearch
    {
        public DataTablePaging DataTablePaging { get; set; }
        public string SearchTerm { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Guid ProductID { get; set; }
        public Product Product { get; set; }
        public Guid EmployeeID { get; set; }
        public Employee Employee { get; set; }
        public Guid StageID { get; set; }
        public Stage Stage { get; set; }
        public string PostDate { get; set; }
        public Guid? LineStageDetailID { get; set; }
        public bool? Status { get; set; }

        //public SubComponent SubComponent { get; set; }
        //public Product OutputComponent { get; set; }
        //public Product Component { get; set; }
    }
}
