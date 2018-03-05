using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IBankBusiness
    {
        List<Bank> GetBankForSelectList();
        List<Bank> GetAllBank(BankAdvanceSearch bankAdvanceSearch);
        Bank GetBank(string code);
        bool CheckCodeExist(string code);
        object InsertUpdateBank(Bank bank);
        object DeleteBank(string code);
    }
}
