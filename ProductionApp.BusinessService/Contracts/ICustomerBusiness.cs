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
        Customer GetCustomer(Guid customerId);

    }
}
