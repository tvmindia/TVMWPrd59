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
        List<CustomerInvoiceDetail> GetCustomerInvoiceDetailLinkForEditGroup(string id, string groupID);
        object UpdateCustomerInvoiceDetail(CustomerInvoice customerInvoice);
        object DeleteCustomerInvoice(Guid id);
        object DeleteCustomerInvoiceDetail(Guid id,string isGroupItem,Guid invoiceID);
        List<CustomerInvoice> GetRecentCustomerInvoice(string BaseURL);
        object UpdateCustomerInvoiceMailStatus(CustomerInvoice CustomerInvoice);
        Task<bool> EmailPush(CustomerInvoice CustomerInvoice);
        CustomerInvoice GetMailPreview(Guid ID);
        List<CustomerInvoiceDetail> GetGroupProductListForCustomerInvoiceDetail(string slipNo,Guid groupID);
        List<CustomerInvoiceDetail> GetGroupCustomerInvoiceDetailLink(Guid id, Guid groupID);
    }
}
