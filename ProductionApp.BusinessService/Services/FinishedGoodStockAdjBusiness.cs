using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;

namespace ProductionApp.BusinessService.Services
{
    public class FinishedGoodStockAdjBusiness:IFinishedGoodStockAdjBusiness
    {
        private IFinishedGoodStockAdjRepository _finishedGoodStockAdjRepository;
        private ICommonBusiness _commonBusiness;

        public FinishedGoodStockAdjBusiness(IFinishedGoodStockAdjRepository finishedGoodStockAdjRepository, ICommonBusiness commonBusiness)
        {
            _finishedGoodStockAdjRepository = finishedGoodStockAdjRepository;
            _commonBusiness = commonBusiness;
        }

        public List<FinishedGoodStockAdj> GetAllFinishedGoodStockAdj(FinishedGoodStockAdjAdvanceSearch finishedGoodStockAdjAdvanceSearch)
        {
            return _finishedGoodStockAdjRepository.GetAllFinishedGoodStockAdj(finishedGoodStockAdjAdvanceSearch);
        }

        public object InsertUpdateFinishedGoodStockAdj(FinishedGoodStockAdj finishedGoodStockAdj)
        {
            DetailsXML(finishedGoodStockAdj);
            return _finishedGoodStockAdjRepository.InsertUpdateFinishedGoodStockAdj(finishedGoodStockAdj);
        }

        public void DetailsXML(FinishedGoodStockAdj finishedGoodStockAdj)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in finishedGoodStockAdj.FinishedGoodStockAdjDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            finishedGoodStockAdj.DetailXML = result;
        }

        public FinishedGoodStockAdj GetFinishedGoodStockAdj(Guid id)
        {
            return _finishedGoodStockAdjRepository.GetFinishedGoodStockAdj(id);
        }

        public List<FinishedGoodStockAdjDetail> GetFinishedGoodStockAdjDetail(Guid id)
        {
            return _finishedGoodStockAdjRepository.GetFinishedGoodStockAdjDetail(id);
        }

        public object DeleteFinishedGoodStockAdj(Guid id)
        {
            return _finishedGoodStockAdjRepository.DeleteFinishedGoodStockAdj(id);
        }

        public object DeleteFinishedGoodStockAdjDetail(Guid id)
        {
            return _finishedGoodStockAdjRepository.DeleteFinishedGoodStockAdjDetail(id);
        }
    }
}
