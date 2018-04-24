using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface ISupplierInvoiceRepository
    {
        List<SupplierInvoice> GetAllSupplierInvoice(SupplierInvoiceAdvanceSearch supplierInvoiceAdvanceSearch);
        SupplierInvoice GetSupplierInvoice(Guid id);
        object InsertUpdateSupplierInvoice(SupplierInvoice supplierInvoice);
        object DeleteSupplierInvoice(Guid id);
        object DeleteSupplierInvoiceDetail(Guid id);
        List<SupplierInvoiceDetail> GetSupplierInvoiceDetail(Guid id);
    }
}
