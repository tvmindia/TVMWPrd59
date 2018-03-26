using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class StageBusiness: IStageBusiness
    {
        private IStageRepository _stageRepository;

        public StageBusiness(IStageRepository stageRepository)
        {
            _stageRepository = stageRepository;
        }
        public List<Stage> GetAllStage(StageAdvanceSearch stageAdvanceSearch)
        {
            return _stageRepository.GetAllStage(stageAdvanceSearch);
        }
        public object InsertUpdateStage(Stage stage)
        {
            return _stageRepository.InsertUpdateStage(stage);
        }
        public Stage GetStage(Guid id)
        {
            return _stageRepository.GetStage(id);
        }
        public object DeleteStage(Guid id)
        {
            return _stageRepository.DeleteStage(id);
        }
        public List<Stage> GetStageForSelectList()
        {
            return _stageRepository.GetStageForSelectList();
        }
    }
}
