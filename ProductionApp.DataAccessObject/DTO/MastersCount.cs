using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProductionApp.DataAccessObject.DTO
{
   public  class MastersCount
    {
        public int Approver { get; set; }
        public int Bank { get; set; }
        public int ChartOfAccounts { get; set; }
        public int Customer { get; set; }
        public int Employee { get; set; }
        public int Material { get; set; }
        public int Product { get; set; }
        public int ProductCategory { get; set; }
        public int ProductStage { get; set; }
        public int SubComponents { get; set; }
        public int Supplier { get; set; }
        public List<MastersCount> MasterCountList { get; set; }
    }
}
