using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialReturnFromProductionViewModel
    {
        public Guid ID{ get; set; }
        public string ReturnNo{get; set;}

        public DateTime ReturnDate { get; set; }      

        public Guid ReceivedBy { get; set; }
        public Guid ReturnBy { get; set; }

        [DataType(DataType.MultilineText)]
        public string GeneralNotes { get; set; }
        public CommonViewModel Common { get; set; }

        //Additional Fields
        public string ReturnDateFormatted { get; set; }
    }

    public class MaterialReturnFromProductionDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid HeaderID { get; set; }
        public Guid MaterialID { get; set; }
        public string MaterialDesc { get; set; }
        public string UnitCode { get; set; }
        public decimal Qty { get; set; }
        public CommonViewModel Common { get; set; }
    }
}