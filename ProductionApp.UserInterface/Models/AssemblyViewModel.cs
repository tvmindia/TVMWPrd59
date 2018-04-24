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
        [Required(ErrorMessage = "Assembling Date is missing")]
        public DateTime AssemblyDate { get; set; }
        [Display(Name = "Done By")]
        public Guid AssembleBy { get; set; }
        [Display(Name = "Select Product")]
        [Required(ErrorMessage = "Product is missing")]
        public Guid ProductID { get; set; }
        [Display(Name = "Quantity To Assemble")]
        [Required(ErrorMessage = "Quantity is missing")]
        public decimal? Qty { get; set; }
        //Additional properties
        public CommonViewModel Common { get; set; }
        public string AssemblyDateFormatted { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public ProductViewModel Product { get; set; }
        public decimal BOMQty { get; set; }
        public decimal ReaquiredQty { get; set; }
        public decimal Stock { get; set; }
        public decimal Balance { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<AssemblyViewModel> AssemblyList { get; set; }
        public string BaseURL { get; set; }
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