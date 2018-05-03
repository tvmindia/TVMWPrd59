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
            DetailsXMl(supplierPayment);
            return _supplierPaymentRepository.InsertUpdateSupplierPayment(supplierPayment);
        }
        public void DetailsXMl(SupplierPayment supplierPayment)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in supplierPayment.SupplierPaymentDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            supplierPayment.DetailXML = result;
        }
        public SupplierPayment GetSupplierPayment(string Id)
        {
            return _supplierPaymentRepository.GetSupplierPayment(Id);
        }
        public object DeleteSupplierPayment(Guid id,string userName)
        {
            return _supplierPaymentRepository.DeleteSupplierPayment(id, userName);
        }
        public object ValidateSupplierPayment(Guid id, string paymentrefNo)
        {
            return _supplierPaymentRepository.ValidateSupplierPayment(id, paymentrefNo);
        }



    }
}
