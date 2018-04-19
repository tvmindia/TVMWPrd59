using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;

namespace ProductionApp.BusinessService.Services
{
    public class RequisitionBusiness: IRequisitionBusiness
    {
        private IRequisitionRepository _requisitionRepository;
        private ICommonBusiness _commonBusiness;
        public RequisitionBusiness(IRequisitionRepository requisitionRepository, ICommonBusiness commonBusiness)
        {
            _requisitionRepository = requisitionRepository;
            _commonBusiness = commonBusiness;
        }

        public List<Requisition> GetAllRequisition(RequisitionAdvanceSearch requisitionAdvanceSearch)
        {
            return _requisitionRepository.GetAllRequisition(requisitionAdvanceSearch);
        }

        public object InsertUpdateRequisition(Requisition requisition)
        {
            DetailsXMl(requisition);
            return _requisitionRepository.InsertUpdateRequisition(requisition);
        }

        public void DetailsXMl(Requisition requisition)
        {
            string result = "<Details>";
            int totalRows = 0;
            foreach (object some_object in requisition.RequisitionDetailList)
            {
                _commonBusiness.XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            requisition.DetailXML = result;
        }

        public List<Requisition> GetAllRequisitionForPurchaseOrder()
        {
            return _requisitionRepository.GetAllRequisitionForPurchaseOrder();
        }
        public List<RequisitionDetail> GetRequisitionDetailsByIDs(string IDs, string POID)
        {
            return _requisitionRepository.GetRequisitionDetailsByIDs(IDs, POID);
        }
        public Requisition GetRequisition(Guid ID)
        {
            return _requisitionRepository.GetRequisition(ID);
        }

        public List<RequisitionDetail> GetRequisitionDetail(Guid ID)
        {
            return _requisitionRepository.GetRequisitionDetail(ID);
        }

        public object DeleteRequisitionDetail(Guid ID)
        {
            return _requisitionRepository.DeleteRequisitionDetail(ID);
        }
        public object DeleteRequisition(Guid ID)
        {
            return _requisitionRepository.DeleteRequisition(ID);
        }

        public List<Requisition> GetRecentRequisition(string BaseURL)
        {           
            return _requisitionRepository.GetRecentRequisition();          
        }
    }
}
