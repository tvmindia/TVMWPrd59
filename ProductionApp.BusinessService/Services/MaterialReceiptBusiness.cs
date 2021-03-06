﻿using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class MaterialReceiptBusiness: IMaterialReceiptBusiness
    {
        #region Constructor Injection
        IMaterialReceiptRepository _materialReceiptRepository;
        ICommonBusiness _commonBusiness;
        public MaterialReceiptBusiness(IMaterialReceiptRepository materialReceiptRepository,ICommonBusiness commonBusiness)
        {
            _materialReceiptRepository = materialReceiptRepository;
            _commonBusiness = commonBusiness;
        }
        #endregion Constructor Injection

        public List<MaterialReceipt> GetAllMaterialReceipt(MaterialReceiptAdvanceSearch materialReceiptAdvanceSearch)
        {
            return _materialReceiptRepository.GetAllMaterialReceipt(materialReceiptAdvanceSearch);
        }

        public object InsertUpdateMaterialReceipt(MaterialReceipt materialReceipt)
        {
            DetailsXMl(materialReceipt);
            return _materialReceiptRepository.InsertUpdateMaterialReceipt(materialReceipt);
        }

        public void DetailsXMl(MaterialReceipt materialReceipt)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in materialReceipt.MaterialReceiptDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            materialReceipt.DetailXML = result;
        }

        public object DeleteMaterialReceipt(MaterialReceipt materialReceipt)
        {
            return _materialReceiptRepository.DeleteMaterialReceipt(materialReceipt);
        }

        public object DeleteMaterialReceiptDetail(MaterialReceipt materialReceipt)
        {
            return _materialReceiptRepository.DeleteMaterialReceiptDetail(materialReceipt);
        }

        public MaterialReceipt GetMaterialReceipt(Guid id)
        {
            return _materialReceiptRepository.GetMaterialReceipt(id);
        }

        public List<MaterialReceiptDetail> GetAllMaterialReceiptDetailByHeaderID(Guid id)
        {
            return _materialReceiptRepository.GetAllMaterialReceiptDetailByHeaderID(id);
        }

        public List<MaterialReceipt> GetRecentMaterialReceiptSummary()
        {
            return _materialReceiptRepository.GetRecentMaterialReceiptSummary();
        }

    }
}
