using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialReturnViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Supplier")]
        public Guid SupplierID { get; set; }
        [Display(Name = "Return Slip No#")]
        public string ReturnSlipNo { get; set; }
        [Display(Name = "Return Date")]
        public DateTime ReturnDate { get; set; }
        [Display(Name = "Billing Address")]
        public string BillAddress { get; set; }
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }
        public string GeneralNotes { get; set; }
        //Additional properties
        public CommonViewModel Common { get; set; }
        public string ReturnDateFormatted { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
    }
    public class MaterialReturnDetailViewModel
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