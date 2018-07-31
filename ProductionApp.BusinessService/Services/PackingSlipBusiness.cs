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
            try
            {
                return _packingSlipRepository.GetAllPackingSlip(paySlipAdvanceSearch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object InsertUpdatePackingSlip(PackingSlip packingSlip)
        {
            try
            {
                DetailsXMl(packingSlip);
                return _packingSlipRepository.InsertUpdatePackingSlip(packingSlip);
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        public void DetailsXMl(PackingSlip packingSlip)
        {
            //string result = "<Details>";
            //int totalRows = 0;
            //foreach (object some_object in packingSlip.PackingSlipDetailList)
            //{
            //    _commonBusiness.XML(some_object, ref result, ref totalRows);
            //}
            //result = result + "</Details>";

            //packingSlip.DetailXML = result;

            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in packingSlip.PackingSlipDetailList_Group)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            packingSlip.GroupDetailXML = result;

            result = "<Details>";
            totalRows = 0;
            foreach (object some_object in packingSlip.PackingSlipDetailList_Product)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            packingSlip.ProductDetailXML = result;

        }

        public PackingSlip GetPackingSlip(Guid id)
        {
            try
            {
                return _packingSlipRepository.GetPackingSlip(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PackingSlipDetail> GetPackingSlipDetail(Guid id)
        {
            try
            {
                return _packingSlipRepository.GetPackingSlipDetail(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PackingSlip> PackingSlipDetailByPackingSlipDetailID(Guid PkgSlipDetailID)
        {
            try
            {
                return _packingSlipRepository.PackingSlipDetailByPackingSlipDetailID(PkgSlipDetailID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object DeletePackingSlipDetail(Guid id, string isGroupItem)
        {
            try
            {
                return _packingSlipRepository.DeletePackingSlipDetail(id, isGroupItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object DeletePackingSlip(Guid id)
        {
            try
            {
                return _packingSlipRepository.DeletePackingSlip(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PackingSlip> GetPackingSlipForSelectList()
        {
            try
            {
                return _packingSlipRepository.GetPackingSlipForSelectList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PackingSlip>GetRecentPackingSlip(string BaseURL)
        {
            try
            {
                return _packingSlipRepository.GetRecentPackingSlip();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SalesOrderDetail> GetPackingSlipDetailGroupEdit(Guid groupID, Guid packingSlipID,Guid saleOrderID)
        {
            try
            {
                return _packingSlipRepository.GetPackingSlipDetailGroupEdit(groupID, packingSlipID, saleOrderID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
