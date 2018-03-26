﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IBillOfMaterialRepository
    {
        List<BillOfMaterial> GetAllBillOfMaterial(BillOfMaterialAdvanceSearch billOfMaterialAdvanceSearch);
        object InsertUpdateBillOfMaterial(BillOfMaterial billOfMaterial);
        BillOfMaterial GetBillOfMaterial(Guid id);
        List<BillOfMaterialDetail> GetBillOfMaterialDetail(Guid id);
        object DeleteBillOfMaterial(Guid id);
        object DeleteBillOfMaterialDetail(Guid id);
        object InsertUpdateBOMComponentLine(BOMComponentLine bOMComponentLine);
        object DeleteBOMComponentLine(Guid id);
        List<BOMComponentLine> GetBOMComponentLineByComponentID(Guid componentID);
        List<BOMComponentLineStage> GetBOMComponentLineStage(Guid id);
    }
}
