using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.BusinessService.Services
{
    public class SupplierInvoiceBusiness: ISupplierInvoiceBusiness
    {
        private ISupplierInvoiceRepository _supplierInvoiceRepository;
        private ICommonBusiness _commonBusiness;
        public SupplierInvoiceBusiness(ISupplierInvoiceRepository supplierInvoiceRepository, ICommonBusiness commonBusiness)
        {
            _supplierInvoiceRepository = supplierInvoiceRepository;
            _commonBusiness = commonBusiness;
        }

        public List<SupplierInvoice> GetAllSupplierInvoice(SupplierInvoiceAdvanceSearch supplierInvoiceAdvanceSearch)
        {
            return _supplierInvoiceRepository.GetAllSupplierInvoice(supplierInvoiceAdvanceSearch);
        }

        public SupplierInvoice GetSupplierInvoice(Guid id)
        {
            return _supplierInvoiceRepository.GetSupplierInvoice(id);
        }

        public object InsertUpdateSupplierInvoice(SupplierInvoice supplierInvoice)
        {
            DetailsXMl(supplierInvoice);
            return _supplierInvoiceRepository.InsertUpdateSupplierInvoice(supplierInvoice);
        }
        public void DetailsXMl(SupplierInvoice salesOrder)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in salesOrder.SupplierInvoiceDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            salesOrder.DetailXML = result;
        }
        public object DeleteSupplierInvoice(Guid id,string userName)
        {
            return _supplierInvoiceRepository.DeleteSupplierInvoice(id,userName);
        }

        public object DeleteSupplierInvoiceDetail(Guid id, string userName)
        {
            return _supplierInvoiceRepository.DeleteSupplierInvoiceDetail(id, userName);
        }

        public List<SupplierInvoiceDetail> GetAllSupplierInvoiceDetail(Guid id)
        {
            return _supplierInvoiceRepository.GetAllSupplierInvoiceDetail(id);
        }
        public SupplierInvoiceDetail GetSupplierInvoiceDetail(Guid id)
        {
            return _supplierInvoiceRepository.GetSupplierInvoiceDetail(id);
        }

        public decimal GetOutstandingSupplierInvoice()
        {
            return _supplierInvoiceRepository.GetOutstandingSupplierInvoice();
        }
    }
}
