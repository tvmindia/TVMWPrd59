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
    public class SalesInvoieBusiness: ISalesInvoieBusiness
    {
        #region Constructor Injection
        ISalesInvoiceRepository _salesInvoiceRepository;
        ICommonBusiness _commonBusiness;
        public SalesInvoieBusiness(ISalesInvoiceRepository salesInvoiceRepository, ICommonBusiness commonBusiness)
        {
            _salesInvoiceRepository = salesInvoiceRepository;
            _commonBusiness = commonBusiness;
        }
        #endregion Constructor Injection

        public List<SalesSummary> GetSalesSummary() {
            return _salesInvoiceRepository.GetSalesSummary();
        }
    }
}
