using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class RawMaterialViewModel
    {        

        public Guid ID { get; set; }
        [Remote(action: "CheckMaterialCodeExist", controller: "RawMaterial", AdditionalFields = nameof(IsUpdate))]
        [Required(ErrorMessage = "Please Enter Material Code")]
        [Display(Name = "Material Code")]
        public string MaterialCode { get; set; }
        [Required(ErrorMessage = "Please Enter Rate")]
        public decimal Rate { get; set; }
        [Required(ErrorMessage = "Please Enter Type")]
        [Display(Name = "Material Type")]
        public string MaterialTypeCode { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Enter UnitCode")]
        [Display(Name = "Unit Code")]
        public string UnitCode { get; set; }
        [Display(Name = "Reorder Qty")]
        public decimal? ReorderQty { get; set; }
        //additional fields 
        [Display(Name = "Current Stock")]
        public string CurrentStock { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public UnitViewModel Unit { get; set; }
        public MaterialTypeViewModel MaterialType { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }

    public class RawMaterialAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}