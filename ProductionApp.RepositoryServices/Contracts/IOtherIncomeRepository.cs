using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IOtherIncomeRepository
    {
        List<OtherIncome> GetAllOtherIncome(OtherIncomeAdvanceSearch otherIncomeAdvanceSearch);
        object InsertUpdateOtherIncome(OtherIncome otherIncome);
        OtherIncome GetOtherIncome(Guid id);
        List<string> GetAllAccountSubHeadForSelectList();
        object DeleteOtherIncome(Guid id);
    }
}
