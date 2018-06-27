using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface ISubComponentRepository
    {
        List<SubComponent> GetAllSubComponent(SubComponentAdvanceSearch subComponentAdvanceSearch);
        //bool CheckSubComponentCodeExist(string materialCode);
        object InsertUpdateSubComponent(SubComponent subComponent);
        SubComponent GetSubComponent(Guid id);
        object DeleteSubComponent(Guid id, string deletedBy);
        List<SubComponent> GetSubComponentForSelectList();
    }
}
