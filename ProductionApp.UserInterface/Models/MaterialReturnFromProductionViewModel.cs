﻿using System;
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
        [Display(Name = "Return No#")]
        public string ReturnNo{get; set;}
        
        public DateTime ReturnDate { get; set; }
        [Display(Name = "Return To")]
        [Required(ErrorMessage = "Return To is required")]
        public Guid ReceivedBy { get; set; }
        [Display(Name = "Return By")]
        [Required(ErrorMessage = "Return By is required")]
        public Guid ReturnBy { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Remarks")]
        public string GeneralNotes { get; set; }
        public CommonViewModel Common { get; set; }

        //Additional Fields.
        [Display(Name = "Return Date")]
        [Required(ErrorMessage = "Return Date is required")]
        public string ReturnDateFormatted { get; set; }
        public string ReturnToEmployeeName { get; set; }
        public string RecievedByEmployeeName { get; set; }
        public bool IsUpdate { get; set; }
        public string DetailJSON { get; set; }
        public MaterialReturnFromProductionDetailViewModel MaterialReturnFromProductionDetail { get; set; }
        public List<MaterialReturnFromProductionDetailViewModel> MaterialReturnFromProductionDetailList { get; set; }
        public EmployeeViewModel Employee { get; set; }
        public string DetailXML { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }

    }

    public class MaterialReturnFromProductionDetailViewModel
    {
        public Guid ID { get; set; }
        public Guid HeaderID { get; set; }
        [Display(Name = "Material")]
        public Guid MaterialID { get; set; }
        [Display(Name = "Material Description")]
        public string MaterialDesc { get; set; }
        [Display(Name = "Unit Code")]
        public string UnitCode { get; set; }
        [Display(Name = "Quantity")]
        public decimal Qty { get; set; }
        public CommonViewModel Common { get; set; }
        public MaterialViewModel Material { get; set; }
    }

    public class MaterialReturnFromProductionAdvanceSearchViewModel
    {

        [Display(Name = "Search")]
        public string SearchTerm { get; set; }
        public DataTablePagingViewModel DataTablePaging { get; set; }

        [Display(Name = "From Date")]
        public string FromDate { get; set; }
        [Display(Name = "To Date")]
        public string ToDate { get; set; }
        [Display(Name = "Return By")]
        public Guid ReceivedBy { get; set; }
        [Display(Name = "Return To")]
        public Guid ReturnBy { get; set; }
        public string ReturnedByEmployeeName { get; set; }
        public string RecievedByEmployeeName { get; set; }
        public EmployeeViewModel Employee { get; set; }
    }
}