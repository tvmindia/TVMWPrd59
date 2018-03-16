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
        public BOMComponentLineDetailViewModel BOMComponentLineDetail { get; set; }
        public List<BOMComponentLineDetailViewModel> BOMComponentLineDetailList { get; set; }
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

        List<BOMComponentLineStageViewModel> BOMComponentLineStageList { get; set; }
        List<BOMComponentLineDetailViewModel> BOMComponentLineDetailList { get; set; }
    }
    public class BOMComponentLineStageViewModel
    {
        public Guid ID { get; set; }
        public Guid ComponentLineID { get; set; }
        public Guid StageID { get; set; }
        public int StageOrder { get; set; }
        public CommonViewModel Common { get; set; }
    }
    public class BOMComponentLineDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid ComponentLineID { get; set; }
        public Guid StageID { get; set; }
        public string PartType { get; set; }
        public Guid PartID { get; set; }
        public string EntryType { get; set; }
        public decimal Qty { get; set; }
        public CommonViewModel Common { get; set; }
    }
}