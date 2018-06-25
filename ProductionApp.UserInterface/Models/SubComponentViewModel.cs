using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class SubComponentViewModel
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Sub Component Code is missing")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Sub Component Description is missing")]
        public string Description { get; set; }
        [Display(Name = "Opening Quantity")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Opening Quantity must be numeric")]
        public decimal? OpeningQty { get; set; }
        [Display(Name = "Current Quantity")]
        public decimal CurrentQty { get; set; }
        [Required(ErrorMessage = "Unit is missing")]
        [Display(Name = "Unit")]
        public string UnitCode { get; set; }
        [Display(Name = "Weight In KG")]
        public decimal WeightInKG { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public UnitViewModel Unit { get; set; }
        public Guid SubComponentID { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }

    public class SubComponentAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}