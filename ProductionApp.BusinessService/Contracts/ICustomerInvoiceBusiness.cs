using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface ICustomerInvoiceBusiness
    {
        List<CustomerInvoiceDetail> GetPackingSlipListDetail(string packingSlipIDs,string id);
        List<PackingSlip> GetPackingSlipList(Guid customerID);
        object InsertUpdateCustomerInvoice(CustomerInvoice customerInvoice);
        CustomerInvoice GetCustomerInvoice(Guid id);
        List<CustomerInvoiceDetail> GetCustomerInvoiceDetail(Guid id);
        List<CustomerInvoice> GetAllCustomerInvoice(CustomerInvoiceAdvanceSearch customerInvoiceAdvanceSearch);
        List<CustomerInvoiceDetail> GetCustomerInvoiceDetailLinkForEdit(string id);
        object UpdateCustomerInvoiceDetail(CustomerInvoice customerInvoice);
        object DeleteCustomerInvoice(Guid id, string userName);
        object DeleteCustomerInvoiceDetail(Guid id, string userName);
        List<CustomerInvoice> GetRecentCustomerInvoice(string BaseURL);
        object UpdateCustomerInvoiceMailStatus(CustomerInvoice CustomerInvoice);
        Task<bool> EmailPush(CustomerInvoice CustomerInvoice);
        CustomerInvoice GetMailPreview(Guid ID);
        decimal GetOutstandingCustomerInvoice();
    }
}
