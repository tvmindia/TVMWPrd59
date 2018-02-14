using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class RawMaterialViewModel
    {
        public Guid ID { get; set; }
        public string MaterialCode { get; set; }
        public decimal Rate { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string UnitCode { get; set; }
        public decimal ReorderQty { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
    public class RawMaterialAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
    }
}