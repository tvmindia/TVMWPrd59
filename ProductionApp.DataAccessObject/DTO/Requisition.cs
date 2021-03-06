﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class Requisition
    {
        public Guid ID { get; set; }
        public string ReqNo { get; set; }
        public string Title { get; set; }
        public DateTime ReqDate { get; set; }
        public string ReqDateFormatted { get; set; }
        public string ReqStatus { get; set; }
        public string RequisitionBy { get; set; }
        public Guid EmployeeID { get; set; }
        public Guid LatestApprovalID { get; set; }
        public int LatestApprovalStatus { get; set; }
        public string ApprovalStatus { get; set; }

        public bool IsFinalApproved { get; set; }
        public bool IsUpdate { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public DateTime ApprovalDate { get; set; }
        public string ApprovalDateFormatted { get; set; }
        public Guid hdnFileID { get; set; }
        public string DetailXML { get; set; }
        public Common Common { get; set; }
        public RequisitionDetail RequisitionDetail { get; set; }
        public List<RequisitionDetail> RequisitionDetailList { get; set; }
        public List<Requisition> RequisitionList { get; set; }
        public string BaseURL { get; set; }
        public RequisitionAdvanceSearch RequisitionAdvanceSearch { get; set; }
        public string ReqAmount { get; set; }
        public DateTime RequiredDate { get; set; }
        public string RequiredDateFormatted { get; set; }
        public string RequisitionNo { get; set; }
    }


    public class RequisitionDetail
    {
        public Guid ID { get; set; }
        public Guid ReqID { get; set; }
        public Guid MaterialID { get; set; }
        public string Description { get; set; }
        public string RequestedQty { get; set; }
        public string ApproximateRate { get; set; }
        public string POQty { get; set; }
        public string OrderedQty { get; set; }
        public string ReqNo { get; set; }
        public Common Common { get; set; }
        public Material Material { get; set; }   
        public Decimal Discount { get; set; }    
    }

    public class RequisitionAdvanceSearch
    {

        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ReqStatus { get; set; }
        public string RequisitionBy { get; set; }
        public Guid EmployeeID { get; set; }
        public string DateFilter { get; set; }
    }
}
