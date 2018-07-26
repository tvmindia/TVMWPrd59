using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.DataAccessObject.DTO
{
    public class Customer
    {
        public Guid ID { get; set; }
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string ContactEmail { get; set; }
        public string ContactTitle { get; set; }
        public string Website { get; set; }
        public string LandLine { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string OtherPhoneNumbers { get; set; }
        public string BillingAddress { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentTermCode { get; set; }
        public string TaxRegNo { get; set; }
        public string PANNo { get; set; }
        public string GeneralNotes { get; set; }

        //additional fields
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public bool IsUpdate { get; set; }
        public Common Common { get; set; }
        public CustomerAdvanceSearch CustomerAdvanceSearch { get; set; }
        public decimal? OutStanding { get; set; } 

    }
    public class CustomerAdvanceSearch
    {
        public string SearchTerm { get; set; }
        public DataTablePaging DataTablePaging { get; set; }
    }
    public class ContactTitle
    {
        public string Title { get; set; }
    }
}
