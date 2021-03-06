﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IMaterialReceiptRepository
    {
        List<MaterialReceipt> GetAllMaterialReceipt(MaterialReceiptAdvanceSearch materialReceiptAdvanceSearch);
        object InsertUpdateMaterialReceipt(MaterialReceipt materialReceipt);
        object DeleteMaterialReceipt(MaterialReceipt materialReceipt);
        object DeleteMaterialReceiptDetail(MaterialReceipt materialReceipt);
        MaterialReceipt GetMaterialReceipt(Guid id);
        List<MaterialReceiptDetail> GetAllMaterialReceiptDetailByHeaderID(Guid id);
        List<MaterialReceipt> GetRecentMaterialReceiptSummary();
    }
}
