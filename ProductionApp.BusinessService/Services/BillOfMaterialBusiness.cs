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
    public class BillOfMaterialBusiness: IBillOfMaterialBusiness
    {
        #region Constructor Injection
        private IBillOfMaterialRepository _billOfMaterialRepository;
        ICommonBusiness _commonBusiness;
        public BillOfMaterialBusiness(IBillOfMaterialRepository billOfMaterialRepository, ICommonBusiness commonBusiness)
        {
            _billOfMaterialRepository = billOfMaterialRepository;
            _commonBusiness = commonBusiness;
        }
        #endregion Constructor Injection

        #region GetAllBillOfMaterial
        public List<BillOfMaterial> GetAllBillOfMaterial(BillOfMaterialAdvanceSearch billOfMaterialAdvanceSearch)
        {
            return _billOfMaterialRepository.GetAllBillOfMaterial(billOfMaterialAdvanceSearch);
        }
        #endregion GetAllBillOfMaterial

        #region InsertUpdateBillOfMaterial
        public object InsertUpdateBillOfMaterial(BillOfMaterial billOfMaterial)
        {
            DetailsXMl(billOfMaterial);
            return _billOfMaterialRepository.InsertUpdateBillOfMaterial(billOfMaterial);
        }
        #endregion InsertUpdateBillOfMaterial

        #region DetailsXMl
        public void DetailsXMl(BillOfMaterial billOfMaterial)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in billOfMaterial.BillOfMaterialDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            billOfMaterial.DetailXML = result;
        }
        #endregion DetailsXMl

        #region GetBillOfMaterial
        public BillOfMaterial GetBillOfMaterial(Guid id)
        {
            return _billOfMaterialRepository.GetBillOfMaterial(id);
        }
        #endregion GetBillOfMaterial

        #region GetBillOfMaterialDetail
        public List<BillOfMaterialDetail> GetBillOfMaterialDetail(Guid id)
        {
            return _billOfMaterialRepository.GetBillOfMaterialDetail(id);
        }
        #endregion GetBillOfMaterialDetail

        #region DeleteBillOfMaterial
        public object DeleteBillOfMaterial(Guid id)
        {
            return _billOfMaterialRepository.DeleteBillOfMaterial(id);
        }
        #endregion DeleteBillOfMaterial

        #region DeleteBillOfMaterialDetail
        public object DeleteBillOfMaterialDetail(Guid id)
        {
            return _billOfMaterialRepository.DeleteBillOfMaterialDetail(id);
        }
        #endregion DeleteBillOfMaterialDetail

        #region
        #endregion

    }
}
