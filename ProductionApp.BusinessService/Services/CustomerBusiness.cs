using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class CustomerBusiness: ICustomerBusiness
    {
        private ICustomerRepository _customerRepository;
        public CustomerBusiness(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Customer GetCustomer(Guid customerId)
        {
            return _customerRepository.GetCustomer(customerId);

        }

        public List<Customer> GetCustomerForSelectList()
        {
            return _customerRepository.GetCustomerForSelectList();
        }
        public List<Customer> GetAllCustomer(CustomerAdvanceSearch customerAdvanceSearch)
        {
            return _customerRepository.GetAllCustomer(customerAdvanceSearch);
        }

    }
}
