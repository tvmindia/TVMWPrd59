﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IMaterialReceiptBusiness
    {
        List<MaterialReceipt> GetAllMaterialReceipt(MaterialReceiptAdvanceSearch materialReceiptAdvanceSearch);
        object InsertUpdateMaterialReceipt(MaterialReceipt materialReceipt);
        object DeleteMaterialReceipt(Guid id);
        object DeleteMaterialReceiptDetail(Guid id);
        MaterialReceipt GetMaterialReceiptByID(Guid id);
        List<MaterialReceiptDetail> GetAllMaterialReceiptDetailByHeaderID(Guid id);
    }
}
