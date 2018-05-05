using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class ApprovalStatus
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public Common Common { get; set; }
    }
}
