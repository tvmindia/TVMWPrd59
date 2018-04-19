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
    public class PackingSlipBusiness : IPackingSlipBusiness
    {
        private ICommonBusiness _commonBusiness;
        private IPackingSlipRepository _packingSlipRepository;

        public PackingSlipBusiness(ICommonBusiness commonBusiness, IPackingSlipRepository packingSlipRepository)
        {
            _commonBusiness = commonBusiness;
            _packingSlipRepository = packingSlipRepository;
        }

        public List<PackingSlip> GetAllPackingSlip(PackingSlipAdvanceSearch paySlipAdvanceSearch)
        {
            return _packingSlipRepository.GetAllPackingSlip(paySlipAdvanceSearch);
        }

        public object InsertUpdatePackingSlip(PackingSlip packingSlip)
        {
            DetailsXMl(packingSlip);
            return _packingSlipRepository.InsertUpdatePackingSlip(packingSlip);
        }

        public void DetailsXMl(PackingSlip packingSlip)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in packingSlip.PackingSlipDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            packingSlip.DetailXML = result;
        }

        public PackingSlip GetPackingSlip(Guid id)
        {
            return _packingSlipRepository.GetPackingSlip(id);
        }

        public List<PackingSlipDetail> GetPackingSlipDetail(Guid id)
        {
            return _packingSlipRepository.GetPackingSlipDetail(id);
        }

        public List<PackingSlip> PackingSlipDetailByPackingSlipDetailID(Guid PkgSlipDetailID)
        {
            return _packingSlipRepository.PackingSlipDetailByPackingSlipDetailID(PkgSlipDetailID);
        }

        public object DeletePackingSlipDetail(Guid id)
        {
            return _packingSlipRepository.DeletePackingSlipDetail(id);
        }

        public object DeletePackingSlip(Guid id)
        {
            return _packingSlipRepository.DeletePackingSlip(id);
        }

        public List<PackingSlip> GetPackingSlipForSelectList()
        {
            return _packingSlipRepository.GetPackingSlipForSelectList();
        }

        public List<PackingSlip>GetRecentPackingSlip(string BaseURL)
        {
            return _packingSlipRepository.GetRecentPackingSlip();
        }

    }
}
