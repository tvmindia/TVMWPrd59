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
        [Display(Name = "Product")]
        public Guid ID { get; set; }
        [Remote(action: "CheckProductCodeExist", controller: "Product", AdditionalFields = nameof(IsUpdate))]
        [Required(ErrorMessage = "Please Enter Product Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please Enter Product Name")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Display(Name = "Specification")]
        [Required(ErrorMessage = "Please Enter Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please Enter Unit Code")]
        [Display(Name = "Unit Code")]
        public string UnitCode { get; set; }
        [Display(Name = "Product Category")]
        [Required(ErrorMessage = "Please Enter Category")]
        public string ProductCategoryCode { get; set; }
        [Display(Name = "Reorder Quantity")]
        public decimal? ReorderQty { get; set; }
        //[Required(ErrorMessage = "Please Enter Rate")]
        //public decimal Rate { get; set; }
        [Display(Name = "Opening Stock")]
        public decimal? OpeningStock { get; set; }
        [Display(Name = "Current Stock")]
        public decimal? CurrentStock { get; set; }
        public string HSNNo { get; set; }
        [Display(Name = "Weight In KG")]
        public decimal? WeightInKG { get; set; }
        [Display(Name = "Cost Price")]
        public decimal? CostPrice { get; set; }
        //[Display(Name = "Selling Price")]
        //public decimal? SellingPrice { get; set; }
        [Display(Name = "Selling Price In KG")]
        public decimal? SellingPriceInKG { get; set; }
        [Display(Name = "Selling Price Per Piece")]
        public decimal? SellingPricePerPiece { get; set; }
        [Display(Name = "Is Invoice In KG")]
        public bool IsInvoiceInKG { get; set; }
        [Required(ErrorMessage = "Please Select Type")]
        public string Type { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid ProductID { get; set; }
        public CommonViewModel Common { get; set; }
        public UnitViewModel Unit { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }
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