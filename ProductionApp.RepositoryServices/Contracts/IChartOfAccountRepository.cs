using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IChartOfAccountRepository
    {
        List<ChartOfAccount> GetChartOfAccountForSelectList(string type);
        List<ChartOfAccount> GetAllChartOfAccount(ChartOfAccountAdvanceSearch chartOfAccountAdvanceSearch);
        //bool CheckChartOfAccountTypeExist(ChartOfAccount chartOfAccount);
        object InsertUpdateChartOfAccount(ChartOfAccount chartOfAccount);
        ChartOfAccount GetChartOfAccount(string code);
        object DeleteChartOfAccount(string code);
    }
}
