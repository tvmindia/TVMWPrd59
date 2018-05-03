using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IOtherIncomeBusiness
    {
        List<OtherIncome> GetAllOtherIncome(OtherIncomeAdvanceSearch otherIncomeAdvanceSearch);
        object InsertUpdateOtherIncome(OtherIncome otherIncome);
        OtherIncome GetOtherIncome(Guid id);
        List<SelectListItem> GetAllAccountSubHeadForSelectList();
        object DeleteOtherIncome(Guid id);
    }
}
