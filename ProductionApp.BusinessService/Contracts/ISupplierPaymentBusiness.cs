using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface ISupplierPaymentBusiness
    {
        List<SupplierPayment> GetAllSupplierPayment(SupplierPaymentAdvanceSearch supplierPaymentAdvanceSearch);
        List<SupplierInvoice> GetOutStandingSupplierInvoices(Guid PaymentID, Guid supplierId);
        SupplierInvoice GetOutstandingAmount(Guid Id);
        object InsertUpdateSupplierPayment(SupplierPayment supplierPayment);
        SupplierPayment GetSupplierPayment(string Id);
        object DeleteSupplierPayment(Guid id);
        object ValidateSupplierPayment(Guid id, string paymentrefNo);
    }
}
