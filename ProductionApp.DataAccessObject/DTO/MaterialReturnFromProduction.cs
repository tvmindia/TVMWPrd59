using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class MaterialReturnFromProduction
    {
        public Guid ID { get; set; }
        public string ReturnNo { get; set; }

        public DateTime ReturnDate { get; set; }

        public Guid ReceivedBy { get; set; }
        public Guid ReturnBy { get; set; }
        public string GeneralNotes { get; set; }
        public Common Common { get; set; }


        public string ReturnDateFormatted { get; set; }
    }

    public class MaterialReturnFromProductionDetail
    {
        public Guid ID { get; set; }
        public Guid HeaderID { get; set; }
        public Guid MaterialID { get; set; }
        public string MaterialDesc { get; set; }
        public string UnitCode { get; set; }
        public decimal Qty { get; set; }
        public Common Common { get; set; }
        public Material Material { get; set; }
    }
}
