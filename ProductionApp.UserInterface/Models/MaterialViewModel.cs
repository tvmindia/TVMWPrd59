using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialViewModel
    {        

        public Guid ID { get; set; }
        [Remote(action: "CheckMaterialCodeExist", controller: "Material", AdditionalFields = "IsUpdate,ID")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Material Code is missing")]
        [Display(Name = "Material Code")]
        public string MaterialCode { get; set; }
        [Required(ErrorMessage = "Rate is missing")]
        public decimal Rate { get; set; }
        [Required(ErrorMessage = "Type is missing")]
        [Display(Name = "Material Type")]
        public string MaterialTypeCode { get; set; }
        [Required(ErrorMessage = "Description is missing")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "HSN No.")]
        public string HSNNo { get; set; }
        [Required(ErrorMessage = "Unit is missing")]
        [Display(Name = "Unit")]
        public string UnitCode { get; set; }
        [Display(Name = "Reorder Quantity")]
        public decimal? ReorderQty { get; set; }
        //additional fields 
        public Guid MaterialID { get; set; }
        [Display(Name = "Current Stock (Nos.)")]
        public decimal? CurrentStock { get; set; }
        [Display(Name = "Opening Stock")]
        public decimal? OpeningStock { get; set; }
        [Display(Name = "Weight In KG")]
        public decimal? WeightInKG { get; set; }
        [Display(Name = "Cost Price")]
        public decimal? CostPrice { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public UnitViewModel Unit { get; set; }
        public MaterialTypeViewModel MaterialType { get; set; }
        public List<SelectListItem> SelectList { get; set; }
        public List<MaterialViewModel> MaterialList { get; set; }
    }

    public class MaterialAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "Unit")]
        public UnitViewModel Unit { get; set; }
        public MaterialTypeViewModel MaterialType { get; set; }
        //[Display(Name = "Material Type ")]
        //public string MaterialType { get; set; }
        public string ID { get; set; } //For ReorderAlert url view in StoreDashboard 

    }


    public class MaterialSummaryViewModel
    {
        public string MaterialType { get; set; }
        public Decimal Value { get; set; }
        public string Color { get; set; }
        public string ValueFormatted { get; set; }
    }
}