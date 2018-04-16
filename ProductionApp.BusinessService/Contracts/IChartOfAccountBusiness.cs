using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IChartOfAccountBusiness
    {
        List<SelectListItem> GetChartOfAccountForSelectList();
        List<ChartOfAccount> GetAllChartOfAccount(ChartOfAccountAdvanceSearch chartOfAccountAdvanceSearch);
        //bool CheckChartOfAccountTypeExist(ChartOfAccount chartOfAccount);
        object InsertUpdateChartOfAccount(ChartOfAccount chartOfAccount);
        ChartOfAccount GetChartOfAccount(string code);
        object DeleteChartOfAccount(string code);
    }
}
