using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface ISubComponentBusiness
    {
        List<SubComponent> GetAllSubComponent(SubComponentAdvanceSearch subComponentAdvanceSearch);
        //bool CheckSubComponentCodeExist(string subComponentCode);
        object InsertUpdateSubComponent(SubComponent subComponent);
        SubComponent GetSubComponent(Guid id);
        object DeleteSubComponent(Guid id, string deletedBy);
        List<SubComponent> GetSubComponentForSelectList();
    }
}
