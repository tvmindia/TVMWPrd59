using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class User
    {
        public Guid ID { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string Password { get; set; }
        public string ResetLink { get; set; }
        public DateTime LinkExpiryTime { get; set; }
    }
}
