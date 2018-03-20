﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    class CustomerInvoice
    {
        public Guid ID { get; set; }
        public string InvoiceNo { get; set; }
        public Guid CustomerID { get; set; }
        public string PaymentTerm { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string BillingAddress { get; set; }
        public decimal Discount { get; set; }
        public string GeneralNotes { get; set; }

        //additional properties
        public bool IsUpdate { get; set; }
        public Guid hdnFileID { get; set; }
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public string InvoiceDateFormatted { get; set; }
        public string PaymentDueDateFormatted { get; set; }
        public Common Common { get; set; }
        public CustomerInvoiceDetail CustomerInvoiceDetail { get; set; }


    }
    public class CustomerInvoiceDetail
    {
        public Guid ID { get; set; }
        public Guid CustomerInvoiceID { get; set; }
        public Guid ProductID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Weight { get; set; }
        public decimal Rate { get; set; }
        public string TaxTypeCode { get; set; }
        public decimal TaxPercApplied { get; set; }
        public decimal TaxAmount { get; set; }
        public bool IsInvoiceInKG { get; set; }
        //additional properties
    }

    public class CustomerInvoiceDetailLink
    {
        public Guid ID { get; set; }
    }

    public class CustomerInvoiceAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }
}
