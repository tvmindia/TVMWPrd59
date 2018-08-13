using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductionApp.UserInterface.Models
{
    public class ServiceItemsViewModel
    {
        public Guid ID { get; set; }
        [Display(Name = "Service Name")]
        public string ServiceName { get; set; }
        public decimal Rate { get; set; }

        [Display(Name = "Discount Amount")]
        public decimal TradeDiscountAmount { get; set; }
        [Display(Name = "Discount %")]
        public decimal DiscountPercent { get; set; }
        [Display(Name = "Quantity")]
        public decimal Quantity { get; set; }

        [Display(Name = "Taxable Amount")]
        public decimal TaxableAmount { get; set; }
        [Display(Name = "Tax Amount")]
        public decimal TaxAmount { get; set; }

        [Display(Name = "Tax %")]
        public string TaxTypeCode { get; set; }
       
        [Display(Name = "Total")]
        public decimal GrossAmount { get; set; }
        [Display(Name = "Net Amount")]
        public decimal NetAmount { get; set; }

        public TaxTypeViewModel TaxType { get; set; }
        public List<SelectListItem> SelectList { get; set; }

    }
}