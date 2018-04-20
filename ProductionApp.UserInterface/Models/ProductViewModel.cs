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
        [Remote(action: "CheckProductCodeExist", controller: "Product", AdditionalFields = "IsUpdate,ID")]
        [Required(ErrorMessage = "Product Code is missing")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Product Name is missing")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Display(Name = "Specification")]
        [Required(ErrorMessage = "Description is missing")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Unit is missing")]
        [Display(Name = "Unit")]
        public string UnitCode { get; set; }
        [Display(Name = "Product Category")]
        [Required(ErrorMessage = "Category is missing")]
        public string ProductCategoryCode { get; set; }
        [Display(Name = "Reorder Quantity")]
        public decimal? ReorderQty { get; set; }
        //[Required(ErrorMessage = "Please Enter Rate")]
        //public decimal Rate { get; set; }
        [Display(Name = "Opening Stock")]
        public decimal? OpeningStock { get; set; }
        [Display(Name = "Current Stock")]
        public decimal? CurrentStock { get; set; }
        [Display(Name = "HSN No.")]
        public string HSNNo { get; set; }
        [Display(Name = "Product Weight In KG")]
        public decimal? WeightInKG { get; set; }
        [Display(Name = "Cost Price Per KG")]
        public decimal? CostPrice { get; set; }
        [Display(Name = "Cost Price Per Piece")]
        public decimal? CostPricePerPiece { get; set; }
        [Display(Name = "Selling Price Per KG")]
        public decimal? SellingPriceInKG { get; set; }
        [Display(Name = "Selling Price Per Piece")]
        public decimal? SellingPricePerPiece { get; set; }
        [Display(Name = "Is Invoice In KG")]
        public bool IsInvoiceInKG { get; set; }
        [Required(ErrorMessage = "Type is missing")]
        public string Type { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public UnitViewModel Unit { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }
        public List<SelectListItem> ProductSelectList { get; set; }
    }

    public class ProductAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        public UnitViewModel Unit { get; set; }
        public ProductCategoryViewModel ProductCategory { get; set; }
    }


    public class FinishedGoodSummaryViewModel
    {
        public string Category { get; set; }
        public Decimal Value { get; set; }
        public string Color { get; set; }
        public string ValueFormatted { get; set; }

    }
}