﻿using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class CustomerCreditNoteBusiness : ICustomerCreditNoteBusiness
    {
        private ICustomerCreditNoteRepository _customerCreditNoteRepository;

        Common _commonBusiness = new Common();
        public CustomerCreditNoteBusiness(ICustomerCreditNoteRepository customerCreditNoteRepository)
        {
            _customerCreditNoteRepository = customerCreditNoteRepository;
        }

        public List<CustomerCreditNote> GetAllCustomerCreditNote(CustomerCreditNoteAdvanceSearch customerCreditNoteAdvanceSearch)
        {

            List<CustomerCreditNote> CustomerCreditNoteList = new List<CustomerCreditNote>();
            try
            {
                CustomerCreditNoteList = _customerCreditNoteRepository.GetAllCustomerCreditNote(customerCreditNoteAdvanceSearch);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return CustomerCreditNoteList;
        }
        public CustomerCreditNote GetCustomerCreditNote(Guid ID)
        {
            CustomerCreditNote customerCreditNote = new CustomerCreditNote();
            try
            {
                customerCreditNote = _customerCreditNoteRepository.GetCustomerCreditNote(ID);
                //if (customerCreditNote != null)
                //{
                //    customerCreditNote.creditAmountFormatted = _commonBusiness.ConvertCurrency(customerCreditNote.CreditAmount, 2);
                //    customerCreditNote.adjustedAmountFormatted = _commonBusiness.ConvertCurrency(customerCreditNote.adjustedAmount, 2);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customerCreditNote;
        }
        public object InsertUpdateCustomerCreditNote(CustomerCreditNote customerCreditNote)
        {
            object result = null;
            try
            {
                result = _customerCreditNoteRepository.InsertUpdateCustomerCreditNote(customerCreditNote);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public object DeleteCustomerCreditNote(Guid ID, string userName)
        {
            object result = null;
            try
            {
                result = _customerCreditNoteRepository.DeleteCustomerCreditNote(ID, userName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public List<CustomerCreditNote> GetCreditNoteByCustomer(Guid ID)
        {
            List<CustomerCreditNote> customerCreditNotelist = new List<CustomerCreditNote>();
            try
            {
                customerCreditNotelist = _customerCreditNoteRepository.GetCreditNoteByCustomer(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customerCreditNotelist;
        }

        public List<CustomerCreditNote> GetCreditNoteByPaymentID(Guid ID, Guid PaymentID)
        {
            List<CustomerCreditNote> customerCreditNotelist = new List<CustomerCreditNote>();
            try
            {
                customerCreditNotelist = _customerCreditNoteRepository.GetCreditNoteByPaymentID(ID, PaymentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customerCreditNotelist;
        }

        public CustomerCreditNote GetCreditNoteAmount(Guid CreditID, Guid CustomerID)
        {
            CustomerCreditNote customerCreditNote = new CustomerCreditNote();
            List<CustomerCreditNote> custcreditlist = new List<CustomerCreditNote>();
            try
            {
                custcreditlist = _customerCreditNoteRepository.GetCreditNoteByCustomer(CustomerID);
                custcreditlist = custcreditlist.Where(m => m.ID == CreditID).ToList();
                customerCreditNote = custcreditlist[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return customerCreditNote;
        }

    }
}
