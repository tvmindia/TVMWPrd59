using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IPaymentTermBusiness
    {
        List<PaymentTerm> GetAllPaymentTerm();
        PaymentTerm GetPaymentTermDetails(string Code);
    }
}
