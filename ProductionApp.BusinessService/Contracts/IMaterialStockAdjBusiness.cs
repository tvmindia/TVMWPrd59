using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IMaterialStockAdjBusiness
    {
        List<MaterialStockAdj> GetAllMaterialStockAdjustment(MaterialStockAdjAdvanceSearch materialStockAdjAdvanceSearch);
        object InsertUpdateStockAdjustment(MaterialStockAdj materialStockAdj);
        MaterialStockAdj GetMaterialStockAdjustment(Guid id);
        List<MaterialStockAdjDetail> GetMaterialStockAdjustmentDetail(Guid id);
        object DeleteMaterialStockAdjustment(Guid id);
        object DeleteMaterialStockAdjustmentDetail(Guid id);       
    }
}
