﻿using ProductionApp.BusinessService.Contracts;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;

namespace ProductionApp.BusinessService.Services
{
    public class SupplierInvoiceBusiness: ISupplierInvoiceBusiness
    {
        private ISupplierInvoiceRepository _supplierInvoiceRepository;
        private ICommonBusiness _commonBusiness;
        public SupplierInvoiceBusiness(ISupplierInvoiceRepository supplierInvoiceRepository, ICommonBusiness commonBusiness)
        {
            _supplierInvoiceRepository = supplierInvoiceRepository;
            _commonBusiness = commonBusiness;
        }

        public List<SupplierInvoice> GetAllSupplierInvoice(SupplierInvoiceAdvanceSearch supplierInvoiceAdvanceSearch)
        {
            return _supplierInvoiceRepository.GetAllSupplierInvoice(supplierInvoiceAdvanceSearch);
        }

        public SupplierInvoice GetSupplierInvoice(Guid id)
        {
            throw new NotImplementedException();
        }

        public object InsertUpdateSupplierInvoice(SupplierInvoice SupplierInvoice)
        {
            throw new NotImplementedException();
        }

        public object DeleteSupplierInvoice(Guid id)
        {
            throw new NotImplementedException();
        }

        public object DeleteSupplierInvoiceDetail(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}