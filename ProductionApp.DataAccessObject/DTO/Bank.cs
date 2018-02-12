using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class Bank
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Opening { get; set; }
        public decimal ActualODLimit { get; set; }
        public decimal DisplayODLimit { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Common Common { get; set; }
        public BankAdvanceSearch BankAdvanceSearch { get; set; }
    }
    public class BankAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string OrderColumn { get; set; }
        public string OrderDir { get; set; }
    }
}
