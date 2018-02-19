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
        public RequisitionBusiness(IRequisitionRepository requisitionRepository)
        {
            _requisitionRepository = requisitionRepository;
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
                XML(some_object, ref result, ref totalRows);
            }
            result = result + "</Details>";

            requisition.DetailXML = result;
        }

        private void XML(object some_object, ref string result, ref int totalRows)
        {
            var properties = GetProperties(some_object);
            result = result + "<item ";
            foreach (var p in properties)
            {
                string name = p.Name;
                var value = p.GetValue(some_object, null);
                result = result + " " + name + @"=""" + value + @""" ";
            }
            result = result + "></item>";
            totalRows = totalRows + 1;
        }
        /// <summary>
        /// using System.Reflection;
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static PropertyInfo[] GetProperties(object obj)
        {
            return obj.GetType().GetProperties();
        }
       public List<Requisition> GetAllRequisitionForPurchaseOrder(RequisitionAdvanceSearch requisitionAdvanceSearch)
        {
            return _requisitionRepository.GetAllRequisitionForPurchaseOrder(requisitionAdvanceSearch);
        }
        public List<RequisitionDetail> GetRequisitionDetailsByIDs(string IDs, string POID)
        {
            return _requisitionRepository.GetRequisitionDetailsByIDs(IDs, POID);
        }
        public Requisition GetRequisition(Guid ID)
        {
            return _requisitionRepository.GetRequisition(ID);
        }
    }
}
