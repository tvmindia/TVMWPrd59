using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class BillOfMaterialViewModel
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage ="Descritpion Required")]
        [Display(Name = "BOM Name")]
        public string Description { get; set; }
        [Required(ErrorMessage ="Please select a Product")]
        public Guid ProductID { get; set; }
        public CommonViewModel Common { get; set; }
        //Additional
        [Display(Name = "Product")]
        public ProductViewModel Product { get; set; }
        public List<BillOfMaterialDetailViewModel> BillOfMaterialDetailList { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public string DetailJSON { get; set; }
        public BOMComponentLineViewModel BOMComponentLine { get; set; }
        public List<BOMComponentLineViewModel> BOMComponentLineList { get; set; }
        public BOMComponentLineStageDetailViewModel BOMComponentLineStageDetail { get; set; }
        public BOMComponentLineStageViewModel BOMComponentLineStage { get; set; }
        public List<BillOfMaterialViewModel> BillOfMaterialList { get; set; }
        public string BaseURL { get; set; }
    }
    public class BillOfMaterialAdvanceSearchViewModel
    {
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name ="Search")]
        public string SearchTerm { get; set; }
    }
    public class BillOfMaterialDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid BillOfMaterialID { get; set; }
        public Guid ComponentID { get; set; }
        public decimal Qty { get; set; }
        public CommonViewModel Common { get; set; }
        //Additional
        public ProductViewModel Product { get; set; }
    }
    public class BOMComponentLineViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Select Component")]
        [Required(ErrorMessage = "Component Required")]
        public Guid ComponentID { get; set; }
        [Display(Name = "Production Line Name")]
        [Required(ErrorMessage = "Line Name Required")]
        public string LineName { get; set; }
        public CommonViewModel Common { get; set; }
        //Additional
        [Display(Name = "Selected Stages")]
        public List<BOMComponentLineStageViewModel> BOMComponentLineStageList { get; set; }
        public BOMComponentLineStageDetailViewModel BOMComponentLineStageDetail { get; set; }
        [Display(Name = "Component")]
        public ProductViewModel Product { get; set; }
        [Display(Name = "Available Stages")]
        public List<StageViewModel> StageList { get; set; }
        public bool IsUpdate { get; set; }
        public string StageJSON { get; set; }
    }
    public class BOMComponentLineStageViewModel
    {
        public Guid ID { get; set; }
        public Guid ComponentLineID { get; set; }
        public Guid StageID { get; set; }
        public int StageOrder { get; set; }
        public CommonViewModel Common { get; set; }
        //Additional
        public StageViewModel Stage { get; set; }
    }
    public class BOMComponentLineStageDetailViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Select Production Line")]
        public Guid ComponentLineID { get; set; }
        [Required(ErrorMessage = "Stage Required")]
        [Display(Name = "Select Stage")]
        public Guid StageID { get; set; }
        [Display(Name = "Item Type")]
        public string PartType { get; set; }
        [Display(Name = "Select Item")]
        [Required(ErrorMessage = "Item Required")]
        public Guid PartID { get; set; }
        [Display(Name = "Entry Type")]
        public string EntryType { get; set; }
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Qty Required")]
        public decimal Qty { get; set; }
        public CommonViewModel Common { get; set; }
        //Additional
        public bool IsUpdate { get; set; }
        public StageViewModel Stage { get; set; }
        public ProductViewModel Product { get; set; }
        public MaterialViewModel Material { get; set; }
        public SubComponentViewModel SubComponent { get; set; }
    }

    public class BOMTreeViewModel
    {
        public Guid ID { get; set; }
        public Guid? ParentID { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Qty { get; set; }

    }
}