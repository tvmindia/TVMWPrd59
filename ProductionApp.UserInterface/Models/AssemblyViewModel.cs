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
        public DateTime AssemblyDate { get; set; }
        public Guid AssembleBy { get; set; }
        public Guid ProductID { get; set; }
        public decimal Qty { get; set; }
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
    }
}