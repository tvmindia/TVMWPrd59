﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IPurchaseOrderBusiness
    {
        List<PurchaseOrder> GetAllPurchaseOrder(PurchaseOrderAdvanceSearch purchaseOrderAdvanceSearch);
        List<SelectListItem> PurchaseOrderDropdownList(Guid supplierID);
        object InsertPurchaseOrder(PurchaseOrder purchaseOrder);
        object UpdatePurchaseOrder(PurchaseOrder purchaseOrder);
        object UpdatePurchaseOrderDetailLink(PurchaseOrder purchaseOrder);
        PurchaseOrder GetPurchaseOrder(Guid ID);
        PurchaseOrder GetMailPreview(Guid ID);
        List<PurchaseOrderDetail> GetPurchaseOrderDetail(Guid ID);
        List<PurchaseOrderDetail> GetPurchaseOrderDetailByPODetailID(Guid ID);
        object UpdatePOMailDetails(PurchaseOrder purchaseOrder);
        object DeletePurchaseOrder(Guid ID);
        object DeletePurchaseOrderDetail(Guid ID);
        object UpdatePurchaseOrderMailStatus(PurchaseOrder purchaseOrder);
        Task<bool> QuoteEmailPush(PurchaseOrder purchaseOrder);
        List<PurchaseOrder> RecentPurchaseOrder(string BaseURL);
    }
}
