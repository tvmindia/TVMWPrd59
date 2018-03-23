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

        public Customer GetCustomer(Guid id)
        {
            return _customerRepository.GetCustomer(id);

        }

        public List<Customer> GetCustomerForSelectList()
        {
            return _customerRepository.GetCustomerForSelectList();
        }
        public List<ContactTitle> GetContactTitleForSelectList()
        {
            return _customerRepository.GetContactTitleForSelectList();
        }
        public List<Customer> GetAllCustomer(CustomerAdvanceSearch customerAdvanceSearch)
        {
            return _customerRepository.GetAllCustomer(customerAdvanceSearch);
        }
        public object InsertUpdateCustomer(Customer customer)
        {
            return _customerRepository.InsertUpdateCustomer(customer);
        }
        public object DeleteCustomer(Guid id)
        {
            return _customerRepository.DeleteCustomer(id);
        }

    }
}
