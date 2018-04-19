﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface ISupplierPaymentRepository
    {
        List<SupplierPayment> GetAllSupplierPayment(SupplierPaymentAdvanceSearch supplierPaymentAdvanceSearch);
        List<SupplierInvoice> GetOutStandingInvoices(Guid PaymentID, Guid CustID);
        SupplierInvoice GetOutstandingAmount(Guid Id);
        object InsertUpdateSupplierPayment(SupplierPayment supplierPayment);
        SupplierPayment GetSupplierPayment(string Id);
        object DeleteSupplierPayment(Guid id);
        object ValidateSupplierPayment(Guid id, string paymentrefNo);
    }
}