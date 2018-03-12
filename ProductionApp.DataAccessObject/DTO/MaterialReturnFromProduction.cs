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
        //Additional Properties.
        public string ReturnDateFormatted { get; set; }
        public string ReturnToEmployeeName { get; set; }
        public string RecievedByEmployeeName { get; set; }
        public bool IsUpdate { get; set; }
        public string DetailJSON { get; set; }
        public MaterialReturnFromProductionDetail MaterialReturnFromProductionDetail { get; set; }
        public List<MaterialReturnFromProductionDetail> MaterialReturnFromProductionDetailList { get; set; }
        public Employee Employee { get; set; }
        public string DetailXML { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }

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
    public class MaterialReturnFromProductionAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Guid ReturnBy { get; set; }
        public Guid ReceivedBy { get; set; }
        public string ReturnedByEmployeeName { get; set; }
        public string RecievedByEmployeeName { get; set; }
        public Employee Employee { get; set; }
    }
}
