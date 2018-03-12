using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class SubComponentViewModel
    {
        public Guid ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal OpeningQty { get; set; }
        public decimal CurrentQty { get; set; }
        public string UnitCode { get; set; }
        public decimal WeightInKG { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public CommonViewModel Common { get; set; }
        public UnitViewModel Unit { get; set; }
    }

    public class SubComponentAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}