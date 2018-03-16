using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class BillOfMaterial
    {
        public Guid ID { get; set; }
        public string Description { get; set; }
        public Guid ProductID { get; set; }
        public Common Common { get; set; }
        //Additional
        public Product Product { get; set; }
        public List<BillOfMaterialDetail> BillOfMaterialDetailList { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public string DetailXML { get; set; }
        public BOMComponentLine BOMComponentLine { get; set; }
        public List<BOMComponentLine> BOMComponentLineList { get; set; }
        public BOMComponentLineDetail BOMComponentLineDetail { get; set; }
        public List<BOMComponentLineDetail> BOMComponentLineDetailList { get; set; }
    }
    public class BillOfMaterialAdvanceSearch
    {
        public DataTablePaging DataTablePaging { get; set; }
        public string SearchTerm { get; set; }
    }
    public class BillOfMaterialDetail
    {
        public Guid ID { get; set; }
        public Guid BillOfMaterialID { get; set; }
        public Guid ComponentID { get; set; }
        public decimal Qty { get; set; }
        public Common Common { get; set; }
        //Additional
        public Product Product { get; set; }
    }
    public class BOMComponentLine
    {
        public Guid ID { get; set; }
        public Guid ComponentID { get; set; }
        public string LineName { get; set; }
        public Common Common { get; set; }
        //Additional
        public List<BOMComponentLineStage> BOMComponentLineStage { get; set; }
        public List<BOMComponentLineDetail> BOMComponentLineDetail { get; set; }
    }
    public class BOMComponentLineStage
    {
        public Guid ID { get; set; }
        public Guid ComponentLineID { get; set; }
        public Guid StageID { get; set; }
        public int StageOrder { get; set; }
        public Common Common { get; set; }
    }
    public class BOMComponentLineDetail
    {
        public Guid ID { get; set; }
        public Guid ComponentLineID { get; set; }
        public Guid StageID { get; set; }
        public string PartType { get; set; }
        public Guid PartID { get; set; }
        public string EntryType { get; set; }
        public decimal Qty { get; set; }
        public Common Common { get; set; }
    }
}
