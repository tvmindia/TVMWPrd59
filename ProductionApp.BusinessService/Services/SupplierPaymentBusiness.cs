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
    public class SupplierPaymentBusiness: ISupplierPaymentBusiness
    {
        private ISupplierPaymentRepository _supplierPaymentRepository;
        private ICommonBusiness _commonBusiness;
        public SupplierPaymentBusiness(ISupplierPaymentRepository supplierPaymentRepository, ICommonBusiness commonBusiness)
        {
            _supplierPaymentRepository = supplierPaymentRepository;
            _commonBusiness = commonBusiness;
        }

        public List<SupplierPayment> GetAllSupplierPayment(SupplierPaymentAdvanceSearch supplierPaymentAdvanceSearch)
        {
            return _supplierPaymentRepository.GetAllSupplierPayment(supplierPaymentAdvanceSearch);
        }

        public List<SupplierInvoice> GetOutStandingSupplierInvoices(Guid PaymentID, Guid supplierId)
        {
            return _supplierPaymentRepository.GetOutStandingSupplierInvoices(PaymentID, supplierId);
        }

        public SupplierInvoice GetOutstandingAmount(Guid Id)
        {
            return _supplierPaymentRepository.GetOutstandingAmount(Id);
        }

        public object InsertUpdateSupplierPayment(SupplierPayment supplierPayment)
        {
            throw new NotImplementedException();
        }

        public SupplierPayment GetSupplierPayment(string Id)
        {
            throw new NotImplementedException();
        }

        public object DeleteSupplierPayment(Guid id)
        {
            throw new NotImplementedException();
        }

        public object ValidateSupplierPayment(Guid id, string paymentrefNo)
        {
            throw new NotImplementedException();
        }
    }
}
