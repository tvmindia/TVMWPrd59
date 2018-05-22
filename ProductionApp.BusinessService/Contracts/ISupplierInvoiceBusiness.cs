using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface ISupplierInvoiceBusiness
    {
        List<SupplierInvoice> GetAllSupplierInvoice(SupplierInvoiceAdvanceSearch supplierInvoiceAdvanceSearch);
        SupplierInvoice GetSupplierInvoice(Guid id);
        object InsertUpdateSupplierInvoice(SupplierInvoice SupplierInvoice);
        object DeleteSupplierInvoice(Guid id, string userName);
        object DeleteSupplierInvoiceDetail(Guid id, string userName);
        List<SupplierInvoiceDetail> GetAllSupplierInvoiceDetail(Guid id);
        SupplierInvoiceDetail GetSupplierInvoiceDetail(Guid id);
    }
}
