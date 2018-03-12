using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IStageBusiness
    {
        List<Stage> GetAllStage(StageAdvanceSearch stageAdvanceSearch);
        object InsertUpdateStage(Stage stage);
        Stage GetStage(Guid id);
        object DeleteStage(Guid id);

    }
}
