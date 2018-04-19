﻿using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Services
{
    public class SupplierPaymentRepository: ISupplierPaymentRepository
    {

        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public SupplierPaymentRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<SupplierPayment> GetAllSupplierPayment(SupplierPaymentAdvanceSearch supplierPaymentAdvanceSearch)
        {
            List<SupplierPayment> supplierPaymentList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con; //SP Should be renamed as GetAllSupplierPayment
                        cmd.CommandText = "[AMC].[GetAllSupplierInvoice]";//SP Should be renamed
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(supplierPaymentAdvanceSearch.SearchTerm) ? "" : supplierPaymentAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = supplierPaymentAdvanceSearch.DataTablePaging.Start;
                        if (supplierPaymentAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = supplierPaymentAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = supplierPaymentAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = supplierPaymentAdvanceSearch.ToDate;
                        if (supplierPaymentAdvanceSearch.SupplierID != Guid.Empty)
                            cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = supplierPaymentAdvanceSearch.SupplierID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                supplierPaymentList = new List<SupplierPayment>();
                                while (sdr.Read())
                                {
                                    SupplierPayment supplierPayment = new SupplierPayment();
                                    {
                                        supplierPayment.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : supplierPayment.ID);
                                        //supplierPayment.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : supplierPayment.EntryNo);
                                        //supplierPayment.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.DateFormat) : supplierPayment.InvoiceDateFormatted);
                                        //supplierPayment.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.DateFormat) : supplierPayment.PaymentDueDateFormatted);
                                        //supplierPayment.Supplier = new Supplier();
                                        //supplierPayment.Supplier.CompanyName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : supplierPayment.Supplier.CompanyName);
                                        //supplierPayment.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? decimal.Parse(sdr["InvoiceAmount"].ToString()) : supplierPayment.InvoiceAmount);
                                        //supplierPayment.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : supplierPayment.TotalCount);
                                        supplierPayment.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : supplierPayment.FilteredCount);
                                    }
                                    supplierPaymentList.Add(supplierPayment);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return supplierPaymentList;
        }

        public List<SupplierInvoice> GetOutStandingInvoices(Guid PaymentID, Guid CustID)
        {
            throw new NotImplementedException();
        }

        public SupplierInvoice GetOutstandingAmount(Guid Id)
        {
            throw new NotImplementedException();
        }

        public object InsertUpdateSupplierPayment(SupplierPayment supplierPayment)
        {
            throw new NotImplementedException();
        }

        public SupplierPayment GetSupplierPayment(string Id)
        {
            throw new NotImplementedException();
        }

        public object DeleteSupplierPayment(Guid id)
        {
            throw new NotImplementedException();
        }

        public object ValidateSupplierPayment(Guid id, string paymentrefNo)
        {
            throw new NotImplementedException();
        }
    }
}
