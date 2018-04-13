using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IEmployeeCategoryBusiness
    {
        List<SelectListItem> GetEmployeeCategoryForSelectList();
    }
}
