﻿using ProductionApp.DataAccessObject.DTO;
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
        [Remote(action: "CheckMaterialCodeExist", controller: "Material", AdditionalFields = nameof(IsUpdate))]
        [MaxLength(50)]
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
        public Guid MaterialID { get; set; }
        [Display(Name = "Current Stock")]
        public decimal CurrentStock { get; set; }
        public decimal OpeningStock { get; set; }
        public decimal WeightInKG { get; set; }
        public decimal CostPrice { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public UnitViewModel Unit { get; set; }
        public MaterialTypeViewModel MaterialType { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }

    public class MaterialAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public UnitViewModel Unit { get; set; }
        public MaterialTypeViewModel MaterialType { get; set; }
    }
}