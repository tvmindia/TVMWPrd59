using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class ChartOfAccountViewModel
    {
        //[Remote(action: "CheckChartOfAccountTypeExist", controller: "ChartOfAccount", AdditionalFields =nameof(IsUpdate))]
        [Required(ErrorMessage = "Code is missing")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Type is missing")]
        public string Type { get; set; }
        [Display(Name = "Type Description")]
        public string TypeDesc { get; set; }
        [Display(Name = "Is Sub Head")]
        [Required(ErrorMessage = "Is SubHead Applicable is missing")]
        public bool IsSubHeadApplicable { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public List<SelectListItem> ChartOfAccountSelectList { get; set; }
    }
    public class ChartOfAccountAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}