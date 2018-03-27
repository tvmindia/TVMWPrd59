using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class CustomerPaymentBusiness : ICustomerPaymentBusiness
    {
        private ICommonBusiness _commonBusiness;
        private ICustomerPaymentRepository _customerPaymentRepository;
        public CustomerPaymentBusiness(ICommonBusiness commonBusiness, ICustomerPaymentRepository customerPaymentRepository)
        {
            _commonBusiness = commonBusiness;
            _customerPaymentRepository = customerPaymentRepository;
        }
    }
}
