using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface ICustomerBusiness
    {
        List<Customer> GetCustomerForSelectList();
        List<ContactTitle> GetContactTitleForSelectList();
        Customer GetCustomer(Guid id);
        List<Customer> GetAllCustomer(CustomerAdvanceSearch customerAdvanceSearch);
        object InsertUpdateCustomer(Customer customer);
        object DeleteCustomer(Guid id);
    }
}
