using ProductionApp.BusinessService.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;

namespace ProductionApp.BusinessService.Services
{
    public class PaymentTermBusiness : IPaymentTermBusiness
    {

        private IPaymentTermRepository _paymentTermRepository;

        public PaymentTermBusiness(IPaymentTermRepository paymentTermRepository)
        {
            _paymentTermRepository = paymentTermRepository;
        }
        public List<PaymentTerm> GetAllPaymentTerm()
        {
            return _paymentTermRepository.GetAllPaymentTerm();
        }

        public PaymentTerm GetPaymentTermDetails(string Code)
        {
            return _paymentTermRepository.GetPaymentTermDetails(Code);
        }
    }
}
