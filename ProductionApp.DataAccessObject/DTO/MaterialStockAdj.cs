﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class MaterialStockAdj
    {
        public Guid ID { get; set; }
        public Guid AdjustedBy { get; set; }
        public Guid? LatestApprovalID { get; set; }
          public DateTime Date { get; set; }
        public string Remarks { get; set; }
        public int LatestApprovalStatus { get; set; }
        public bool IsFinalApproved { get; set; }
        public string ApprovalStatus { get; set; }
        public string AdjustmentNo { get; set; }
        public Common Common { get; set; }

        //Additional Fields
        public string AdjustmentDateFormatted { get; set; }
        public Employee Employee { get; set; }
        public string AdjustedByEmployeeName { get; set; }
        public Material Material { get; set; }
        public List<MaterialStockAdjDetail> MaterialStockAdjDetailList { get; set; }
        public MaterialStockAdjDetail MaterialStockAdjDetail { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public Guid EmployeeID { get; set; }
        public bool IsUpdate { get; set; }
        public string DetailXML { get; set; }
    }

    public class MaterialStockAdjDetail
    {
        public Guid ID { get; set; }
        public Guid AdjustmentID { get; set; }
        public Guid AdjustmentBy { get; set; }
        public Guid MaterialID { get; set; }
        public decimal Qty { get; set; }
        public string Remarks { get; set; }
        public Common Common { get; set; }
        public Material Material { get; set; }
    }

    public class MaterialStockAdjAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Guid AdjustedBy { get; set; }
        public Employee Employee { get; set; }
        public string AdjustedByEmployeeName { get; set; }
        public string ApprovalStatus { get; set; }
    }
}
