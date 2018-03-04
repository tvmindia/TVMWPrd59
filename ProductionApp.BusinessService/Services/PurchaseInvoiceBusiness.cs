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
    public class PurchaseInvoiceBusiness: IPurchaseInvoiceBusiness
    {
        #region Constructor Injection
        IPurchaseInvoiceRepository _PurchaseInvoiceRepository;
        ICommonBusiness _commonBusiness;
        public PurchaseInvoiceBusiness(IPurchaseInvoiceRepository PurchaseInvoiceRepository, ICommonBusiness commonBusiness)
        {
            _PurchaseInvoiceRepository = PurchaseInvoiceRepository;
            _commonBusiness = commonBusiness;
        }
        #endregion Constructor Injection

        public List<PurchaseSummary> GetPurchaseSummary()
        {
            return _PurchaseInvoiceRepository.GetPurchaseSummary();
        }
    }
}
