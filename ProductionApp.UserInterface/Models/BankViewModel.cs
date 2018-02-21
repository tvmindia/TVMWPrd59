using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class BankViewModel
    {
        [Remote(action: "CheckCodeExist", controller: "Bank", AdditionalFields =nameof(IsUpdate))]
        [Required]
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Opening { get; set; }
        public decimal ActualODLimit { get; set; }
        public decimal DisplayODLimit { get; set; }
        //additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public string BankCode { get; set; }
        public CommonViewModel Common { get; set; }
        public List<SelectListItem> SelectList { get; set; }
    }
    public class BankAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }

}