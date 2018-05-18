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

        #region CheckBillOfMaterialExist
        public bool CheckBillOfMaterialExist(Guid productID)
        {
            return _billOfMaterialRepository.CheckBillOfMaterialExist(productID);
        }
        #endregion CheckBillOfMaterialExist

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

        #region CheckLineNameExist
        public bool CheckLineNameExist(string lineName)
        {
            return _billOfMaterialRepository.CheckLineNameExist(lineName);
        }
        #endregion

        #region InsertUpdateBOMComponentLine
        public object InsertUpdateBOMComponentLine(BOMComponentLine bOMComponentLine)
        {
            StageXML(bOMComponentLine);
            return _billOfMaterialRepository.InsertUpdateBOMComponentLine(bOMComponentLine);
        }
        #endregion InsertUpdateBOMComponentLine

        #region StageXML
        public void StageXML(BOMComponentLine bOMComponentLine)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in bOMComponentLine.BOMComponentLineStageList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            bOMComponentLine.StageXML = result;
        }
        #endregion StageXML

        #region DeleteBOMComponentLine
        public object DeleteBOMComponentLine(Guid id)
        {
            return _billOfMaterialRepository.DeleteBOMComponentLine(id);
        }
        #endregion DeleteBOMComponentLine

        #region GetBOMComponentLineByComponentID
        public List<BOMComponentLine> GetBOMComponentLineByComponentID(Guid componentID)
        {
            return _billOfMaterialRepository.GetBOMComponentLineByComponentID(componentID);
        }
        #endregion GetBOMComponentLineByComponentID

        #region GetBOMComponentLineStage
        public List<BOMComponentLineStage> GetBOMComponentLineStage(Guid id)
        {
            return _billOfMaterialRepository.GetBOMComponentLineStage(id);
        }
        #endregion GetBOMComponentLineStage

        #region InsertUpdateBOMComponentLineStageDetail
        public object InsertUpdateBOMComponentLineStageDetail(BOMComponentLineStageDetail bOMComponentLineStageDetail)
        {
            return _billOfMaterialRepository.InsertUpdateBOMComponentLineStageDetail(bOMComponentLineStageDetail);
        }
        #endregion InsertUpdateBOMComponentLineStageDetail

        #region DeleteBOMComponentLineStageDetail
        public object DeleteBOMComponentLineStageDetail(Guid id)
        {
            return _billOfMaterialRepository.DeleteBOMComponentLineStageDetail(id);
        }
        #endregion DeleteBOMComponentLineStageDetail

        #region GetBOMComponentLineStageDetail
        public List<BOMComponentLineStageDetail> GetBOMComponentLineStageDetail(Guid id)
        {
            return _billOfMaterialRepository.GetBOMComponentLineStageDetail(id);
        }
        #endregion GetBOMComponentLineStageDetail


        #region GetRecentBillOfMaterial
        public List<BillOfMaterial> GetRecentBillOfMaterial(string BaseURL)
        {
            return _billOfMaterialRepository.GetRecentBillOfMaterial();
        }
        #endregion GetRecentBillOfMaterial

        public List<BOMTree> GetBOMTree(Guid ProductID)
        {
            List<BOMTree> result = null;
            result= _billOfMaterialRepository.GetBOMTree(ProductID);
            if (result != null) {

                foreach (BOMTree F in result) {
                    if (F.Type == "BOM") F.Icon = "../Content/images/bom.png";
                    if (F.Type == "Component") { F.Icon = "../Content/images/component.png"; }
                    if (F.Type == "Line") F.Icon = "../Content/images/line.png";
                    if (F.Type == "Stage") F.Icon = "../Content/images/stage.png";
                    if (F.Type.Contains("| Input")) F.Icon = "../Content/images/input.png";
                    if (F.Type.Contains("| Output")) F.Icon = "../Content/images/output.png";

                    F.Name = "<b>["+F.Type+"]</b> " + F.Name;
                }


            }

            return result;

        }
        }
}
