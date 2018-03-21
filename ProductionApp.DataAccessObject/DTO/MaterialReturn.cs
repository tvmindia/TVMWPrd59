using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class MaterialReturn
    {
        public Guid ID { get; set; }
        public Guid SupplierID { get; set; }
        public string ReturnSlipNo { get; set; }
        public DateTime ReturnDate { get; set; }
        public string BillAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string GeneralNotes { get; set; }
        //Additional properties
        public Common Common { get; set; }
        public string ReturnDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
    public class MaterialReturnDetail
    {
        public Guid ID { get; set; }
        public Guid HeaderID { get; set; }
        public Guid MaterialID { get; set; }
        public string MaterialDesc { get; set; }
        public string UnitCode { get; set; }
        public decimal Qty { get; set; }
        public decimal Rate { get; set; }
        public decimal CGSTPerc { get; set; }
        public decimal SGSTPerc { get; set; }
        public decimal IGSTPerc { get; set; }
    }
}
