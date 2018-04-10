using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialTypeViewModel
    {
        [Required(ErrorMessage ="Material Type code is required")]
        [MaxLength(10,ErrorMessage ="Length exceeds")]
        public string Code { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        //additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public CommonViewModel Common { get; set; }
        public List<SelectListItem> MaterialTypeSelectList { get; set; }
    }
    public class MaterialTypeAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}