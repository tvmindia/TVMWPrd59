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
    public class MaterialBusiness : IMaterialBusiness
    {
        private IMaterialRepository _materialRepository;
        private ICommonBusiness _commonBusiness;

        public MaterialBusiness(IMaterialRepository materialRepository, ICommonBusiness commonBusiness)
        {
            _materialRepository = materialRepository;
            _commonBusiness = commonBusiness;
        }
        public List<Material> GetMaterialForSelectList()
        {          
            return _materialRepository.GetMaterialForSelectList();
        }
        public List<Material> GetAllMaterial(MaterialAdvanceSearch materialAdvanceSearch)
        {
            return _materialRepository.GetAllMaterial(materialAdvanceSearch);
        }
        public bool CheckMaterialCodeExist(Material material)
        {
            return _materialRepository.CheckMaterialCodeExist(material);
        }
        public object InsertUpdateMaterial(Material material)
        {
            return _materialRepository.InsertUpdateMaterial(material);
        }
        public Material GetMaterial(Guid id)
        {
            return _materialRepository.GetMaterial(id);
        }
        public object DeleteMaterial(Guid id,string deletedBy)
        {
            return _materialRepository.DeleteMaterial(id, deletedBy);
        }

        public List<MaterialSummary> GetMaterialSummary()
        {
            List<MaterialSummary> result = _materialRepository.GetMaterialSummary();
            if (result != null)
            {
                int r = 50;
                int g = 50;
                int b = 50;
                string color = "rgba($r$,$g$,$b$,0.6)";
                foreach (MaterialSummary s in result)
                {
                    Random rnd = new Random();

                    s.Color = color.Replace("$r$", r.ToString()).Replace("$g$", g.ToString()).Replace("$b$", b.ToString());
                    b = b + 50;
                    g = g + 30;
                    r = r + g;
                    if (b > 250)
                    {
                        b = 0;
                    }
                    if (g > 250)
                    {
                        g = 0;

                    }
                    if (r > 250)
                    {
                        r = 0;
                    }


                    s.ValueFormatted = _commonBusiness.ConvertCurrency(s.Value, 2);

                }
            }
            return result;
        }

        public List<Material> GetMaterialListForReorderAlert()
        {
            return _materialRepository.GetMaterialListForReorderAlert();
        }

        public List<Material> GetMaterialListForBillOfMaterial(string materialIDs)
        {
            return _materialRepository.GetMaterialListForBillOfMaterial(materialIDs);
        }
    }
}
