﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class MaterialStockAdjViewModel
    {
        public Guid ID { get; set; }
        public Guid AdjustedBy { get; set; }
        public Guid LatestApprovalID { get; set; }
        public DateTime Date { get; set; }
        public string DateFormatted { get; set; }
        public string Remarks { get; set; }
        public int LatestApprovalstatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public CommonViewModel Common { get; set; }
    }
}