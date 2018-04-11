using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class ProductCategoryViewModel
    {
        [Required(ErrorMessage = "Product Category Code is missing")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Product Description is missing")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Product Category is missing")]
        public string ProductCategoryCode { get; set; }
        //additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public CommonViewModel Common { get; set; }
        public List<SelectListItem> ProductCategorySelectList { get; set; }
    }
    public class ProductCategoryAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}