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

        public object DeleteMaterialReceipt(Guid id)
        {
            return _materialReceiptRepository.DeleteMaterialReceipt(id);
        }

        public object DeleteMaterialReceiptDetail(Guid id)
        {
            return _materialReceiptRepository.DeleteMaterialReceiptDetail(id);
        }

        public MaterialReceipt GetMaterialReceiptByID(Guid id)
        {
            return _materialReceiptRepository.GetMaterialReceiptByID(id);
        }

        public List<MaterialReceiptDetail> GetAllMaterialReceiptDetailByHeaderID(Guid id)
        {
            return _materialReceiptRepository.GetAllMaterialReceiptDetailByHeaderID(id);
        }
    }
}
