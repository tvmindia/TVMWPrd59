using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class AssemblyViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Entry No")]
        public string EntryNo { get; set; }
        [Display(Name = "Assembling Date")]
        public DateTime AssemblyDate { get; set; }
        [Display(Name = "Done By")]
        public Guid AssembleBy { get; set; }
        [Display(Name = "Select Product")]
        public Guid ProductID { get; set; }
        [Display(Name = "Quantity To Assemble")]
        public decimal Qty { get; set; }
        //Additional properties
        public CommonViewModel Common { get; set; }
        public string AssemblyDateFormatted { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public ProductViewModel Product { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
    public class AssemblyAdvanceSearchViewModel
    {
        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }
        [Display(Name = "FromDate")]
        public string FromDate { get; set; }
        [Display(Name = "ToDate")]
        public string ToDate { get; set; }
        [Display(Name = "Product")]
        public Guid ProductID { get; set; }
        [Display(Name = "Assemble By")]
        public Guid AssembleBy { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public ProductViewModel Product { get; set; }
    }
}