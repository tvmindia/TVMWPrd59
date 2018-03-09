using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class ProductViewModel
    {
        public Guid ID { get; set; }
        [Remote(action: "CheckProductCodeExist", controller: "Product", AdditionalFields = nameof(IsUpdate))]
        [Required(ErrorMessage = "Please Enter Product Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Enter Unit Code")]
        [Display(Name = "Unit Code")]
        public string UnitCode { get; set; }
        [Required(ErrorMessage = "Please Enter Category")]
        public string Category { get; set; }
        [Required(ErrorMessage = "Please Enter Rate")]
        public decimal Rate { get; set; }
        public decimal? OpeningStock { get; set; }
        [Display(Name = "Current Stock")]
        public decimal? CurrentStock { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid ProductID { get; set; }
        public string HSNNo { get; set; }
        public CommonViewModel Common { get; set; }
        public UnitViewModel Unit { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }

    public class ProductAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public UnitViewModel Unit { get; set; }
    }


    public class FinishedGoodSummaryViewModel
    {
        public string Category { get; set; }
        public Decimal Value { get; set; }
        public string Color { get; set; }
        public string ValueFormatted { get; set; }

    }
}