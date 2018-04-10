using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.BusinessService.Services
{
    public class MaterialTypeBusiness: IMaterialTypeBusiness
    {
        private IMaterialTypeRepository _materialTypeRepository;

        public MaterialTypeBusiness(IMaterialTypeRepository materialRepository)
        {
            _materialTypeRepository = materialRepository;
        }
        public List<SelectListItem> GetMaterialTypeForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<MaterialType> materialTypeList = _materialTypeRepository.GetMaterialTypeForSelectList();
            if (materialTypeList != null)
                foreach (MaterialType materialType in materialTypeList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = materialType.Description,
                        Value = materialType.Code,
                        Selected = false
                    });
                }
            return selectListItem;
        }
        public object InsertUpdateMaterialType(MaterialType materialType)
        {
            return _materialTypeRepository.InsertUpdateMaterialType(materialType);
        }
        public MaterialType GetMaterialType(string code)
        {
            return _materialTypeRepository.GetMaterialType(code);
        }
    }
}
