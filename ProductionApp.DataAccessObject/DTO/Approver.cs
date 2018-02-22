using SAMTool.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class Approver
    {
        public Guid ID { get; set; }
        public string DocumentTypeCode { get; set; }
        public int Level { get; set; }
        public Guid UserID { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        //additional fields 
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Common Common { get; set; }
        public User User { get; set; }
        public DocumentType DocumentType { get; set; }
    }

    public class ApproverAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
}
