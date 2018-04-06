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
        //additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public CommonViewModel Common { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }
    public class ProductCategoryAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}