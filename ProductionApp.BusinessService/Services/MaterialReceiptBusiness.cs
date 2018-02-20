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
        public MaterialReceiptBusiness(IMaterialReceiptRepository materialReceiptRepository)
        {
            _materialReceiptRepository = materialReceiptRepository;
        }
        #endregion Constructor Injection

        public List<MaterialReceipt> GetAllMaterialReceipt()
        {
            return _materialReceiptRepository.GetAllMaterialReceipt();
        }
    }
}
