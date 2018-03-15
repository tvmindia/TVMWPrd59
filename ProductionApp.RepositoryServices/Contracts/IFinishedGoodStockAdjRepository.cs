using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IFinishedGoodStockAdjRepository
    {
        List<FinishedGoodStockAdj> GetAllFinishedGoodStockAdj(FinishedGoodStockAdjAdvanceSearch finishedGoodStockAdjAdvanceSearch);
        object InsertUpdateFinishedGoodStockAdj(FinishedGoodStockAdj finishedGoodStockAdj);
        FinishedGoodStockAdj GetFinishedGoodStockAdj(Guid id);
        List<FinishedGoodStockAdjDetail> GetFinishedGoodStockAdjDetail(Guid id);
        object DeleteFinishedGoodStockAdj(Guid id);
        object DeleteFinishedGoodStockAdjDetail(Guid id);
    }
}
