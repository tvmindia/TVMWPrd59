using ProductionApp.DataAccessObject.DTO;
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
    public class SupplierInvoiceRepository: ISupplierInvoiceRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public SupplierInvoiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<SupplierInvoice> GetAllSupplierInvoice(SupplierInvoiceAdvanceSearch supplierInvoiceAdvanceSearch)
        {
            List<SupplierInvoice> supplierInvoiceList = null;
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
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[GetAllSupplierInvoice]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(supplierInvoiceAdvanceSearch.SearchTerm) ? "" : supplierInvoiceAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = supplierInvoiceAdvanceSearch.DataTablePaging.Start;
                        if (supplierInvoiceAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = supplierInvoiceAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = supplierInvoiceAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = supplierInvoiceAdvanceSearch.ToDate;
                        if (supplierInvoiceAdvanceSearch.SupplierID != Guid.Empty)
                            cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = supplierInvoiceAdvanceSearch.SupplierID;
                         cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                supplierInvoiceList = new List<SupplierInvoice>();
                                while (sdr.Read())
                                {
                                    SupplierInvoice supplierInvoice = new SupplierInvoice();
                                    {
                                        supplierInvoice.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : supplierInvoice.ID);
                                        supplierInvoice.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : supplierInvoice.InvoiceNo);
                                        supplierInvoice.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.DateFormat) : supplierInvoice.InvoiceDateFormatted);
                                        supplierInvoice.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.DateFormat) : supplierInvoice.PaymentDueDateFormatted);
                                        supplierInvoice.Supplier = new Supplier();
                                        supplierInvoice.Supplier.CompanyName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : supplierInvoice.Supplier.CompanyName);
                                        supplierInvoice.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? decimal.Parse(sdr["InvoiceAmount"].ToString()) : supplierInvoice.InvoiceAmount);
                                        supplierInvoice.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : supplierInvoice.TotalCount);
                                        supplierInvoice.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : supplierInvoice.FilteredCount);
                                    }
                                    supplierInvoiceList.Add(supplierInvoice);
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

            return supplierInvoiceList;
        }

        public SupplierInvoice GetSupplierInvoice(Guid id)
        {
            SupplierInvoice supplierInvoice = new SupplierInvoice();
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
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[GetSupplierInvoice]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    supplierInvoice.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : supplierInvoice.InvoiceNo);
                                    supplierInvoice.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.DateFormat) : supplierInvoice.InvoiceDateFormatted);
                                    supplierInvoice.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.DateFormat) : supplierInvoice.PaymentDueDateFormatted);
                                    supplierInvoice.SupplierID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : supplierInvoice.SupplierID);
                                    supplierInvoice.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : supplierInvoice.GeneralNotes);
                                    supplierInvoice.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : supplierInvoice.BillingAddress);
                                    supplierInvoice.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : supplierInvoice.ShippingAddress);
                                    supplierInvoice.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : supplierInvoice.PaymentTermCode);
                                    supplierInvoice.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : supplierInvoice.Discount);
                                    supplierInvoice.TotalTaxableAmount = (sdr["TaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TaxableAmount"].ToString()) : supplierInvoice.TotalTaxableAmount);
                                    supplierInvoice.TotalTaxAmount = (sdr["TaxAmount"].ToString() != "" ? decimal.Parse(sdr["TaxAmount"].ToString()) : supplierInvoice.TotalTaxAmount);
                                    supplierInvoice.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? decimal.Parse(sdr["InvoiceAmount"].ToString()) : supplierInvoice.InvoiceAmount);

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
            return supplierInvoice;
        
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
