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
                                        supplierInvoice.Supplier.CompanyName = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : supplierInvoice.Supplier.CompanyName);
                                        supplierInvoice.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? decimal.Parse(sdr["InvoiceAmount"].ToString()) : supplierInvoice.InvoiceAmount);
                                        supplierInvoice.Balance = (sdr["BalanceDue"].ToString() != "" ? decimal.Parse(sdr["BalanceDue"].ToString()) : supplierInvoice.Balance);
                                        supplierInvoice.PaymentBooked = (sdr["BookedAmount"].ToString() != "" ? decimal.Parse(sdr["BookedAmount"].ToString()) : supplierInvoice.PaymentBooked);
                                        supplierInvoice.PaymentProcessed = (sdr["ProcessedAmount"].ToString() != "" ? decimal.Parse(sdr["ProcessedAmount"].ToString()) : supplierInvoice.PaymentProcessed);
                                        supplierInvoice.LastPaymentDateFormatted = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()).ToString(settings.DateFormat) : supplierInvoice.LastPaymentDateFormatted);
                                        supplierInvoice.Status = (sdr["Status"].ToString() != "" ? sdr["Status"].ToString() : supplierInvoice.Status);
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
                                    supplierInvoice.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : supplierInvoice.SupplierID);
                                    supplierInvoice.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : supplierInvoice.GeneralNotes);
                                    supplierInvoice.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : supplierInvoice.BillingAddress);
                                    supplierInvoice.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : supplierInvoice.ShippingAddress);
                                    supplierInvoice.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : supplierInvoice.PaymentTermCode);
                                    supplierInvoice.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : supplierInvoice.Discount);
                                    supplierInvoice.AccountCode = (sdr["AccountCode"].ToString() != "" ? sdr["AccountCode"].ToString() : supplierInvoice.AccountCode);
                                    supplierInvoice.PurchaseOrderNo = (sdr["PurchaseOrderNo"].ToString() != "" ? sdr["PurchaseOrderNo"].ToString() : supplierInvoice.PurchaseOrderNo);
                                    supplierInvoice.PurchaseOrderID = (sdr["PurchaseOrderID"].ToString() != "" ? Guid.Parse(sdr["PurchaseOrderID"].ToString()) : supplierInvoice.PurchaseOrderID);
                                    supplierInvoice.TotalTaxableAmount = (sdr["TotalTaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TotalTaxableAmount"].ToString()) : supplierInvoice.TotalTaxableAmount);
                                    supplierInvoice.TotalTaxAmount = (sdr["TotalTaxAmount"].ToString() != "" ? decimal.Parse(sdr["TotalTaxAmount"].ToString()) : supplierInvoice.TotalTaxAmount);
                                    supplierInvoice.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? decimal.Parse(sdr["InvoiceAmount"].ToString()) : supplierInvoice.InvoiceAmount);
                                    supplierInvoice.Balance = (sdr["BalanceDue"].ToString() != "" ? decimal.Parse(sdr["BalanceDue"].ToString()) : supplierInvoice.Balance);
                                    supplierInvoice.PaymentBooked = (sdr["BookedAmount"].ToString() != "" ? decimal.Parse(sdr["BookedAmount"].ToString()) : supplierInvoice.PaymentBooked);
                                    supplierInvoice.PaymentProcessed = (sdr["ProcessedAmount"].ToString() != "" ? decimal.Parse(sdr["ProcessedAmount"].ToString()) : supplierInvoice.PaymentProcessed);

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
        public object InsertUpdateSupplierInvoice(SupplierInvoice supplierInvoice)
        {
            SqlParameter outputStatus, IDOut = null;
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
                        cmd.CommandText = "[AMC].[InsertUpdateSupplierInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = supplierInvoice.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = supplierInvoice.ID;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = supplierInvoice.hdnFileID;
                        cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar,20).Value = supplierInvoice.InvoiceNo;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = supplierInvoice.InvoiceDateFormatted;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = supplierInvoice.SupplierID;
                        cmd.Parameters.Add("@AccountCode", SqlDbType.VarChar,10).Value = supplierInvoice.AccountCode;
                        cmd.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar,20).Value = supplierInvoice.PaymentTermCode;
                        if(supplierInvoice.PurchaseOrderID!=Guid.Empty)
                            cmd.Parameters.Add("@PurchaseOrderID", SqlDbType.UniqueIdentifier).Value = supplierInvoice.PurchaseOrderID;
                        cmd.Parameters.Add("@PurchaseOrderNo", SqlDbType.VarChar, 20).Value = supplierInvoice.PurchaseOrderNo;
                        cmd.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value = supplierInvoice.PaymentDueDateFormatted;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = supplierInvoice.Discount;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.VarChar, -1).Value = supplierInvoice.BillingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.VarChar, -1).Value = supplierInvoice.ShippingAddress;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar, -1).Value = supplierInvoice.GeneralNotes;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.VarChar, -1).Value = supplierInvoice.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = supplierInvoice.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = supplierInvoice.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = supplierInvoice.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = supplierInvoice.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(supplierInvoice.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        //  requisition.ID = Guid.Parse(IDOut.Value.ToString());
                        return new
                        {
                            ID = IDOut.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = supplierInvoice.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Code = IDOut.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = supplierInvoice.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };

        }
        public object DeleteSupplierInvoice(Guid id)
        {
            SqlParameter outputStatus = null;
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
                        cmd.CommandText = "[AMC].[DeleteSupplierInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@supplierInvoiceId", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.DeleteFailure);
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConst.DeleteSuccess
            };
        }
        public object DeleteSupplierInvoiceDetail(Guid id)
        {
            SqlParameter outputStatus = null;
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
                        cmd.CommandText = "[AMC].[DeleteSupplierInvoiceDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.DeleteFailure);
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConst.DeleteSuccess
            };
        }
        public List<SupplierInvoiceDetail> GetAllSupplierInvoiceDetail(Guid id)
        {
            List<SupplierInvoiceDetail> supplierInvoiceDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetAllSupplierInvoiceDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@supplierInvoiceId", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                supplierInvoiceDetailList = new List<SupplierInvoiceDetail>();
                                while (sdr.Read())
                                {
                                    SupplierInvoiceDetail supplierInvoiceDetail = new SupplierInvoiceDetail();
                                    {
                                        supplierInvoiceDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : supplierInvoiceDetail.ID);
                                        supplierInvoiceDetail.MaterialID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : supplierInvoiceDetail.MaterialID);
                                        supplierInvoiceDetail.MaterialDesc = (sdr["MaterialDesc"].ToString() != "" ? sdr["MaterialDesc"].ToString() : supplierInvoiceDetail.MaterialDesc);
                                        supplierInvoiceDetail.MaterialTypeDesc = (sdr["MaterialTypeDesc"].ToString() != "" ? sdr["MaterialTypeDesc"].ToString() : supplierInvoiceDetail.MaterialTypeDesc);
                                        supplierInvoiceDetail.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : supplierInvoiceDetail.MaterialCode);
                                        supplierInvoiceDetail.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : supplierInvoiceDetail.TaxTypeCode);
                                        supplierInvoiceDetail.TaxTypeDescription = (sdr["TaxTypeDescription"].ToString() != "" ? sdr["TaxTypeDescription"].ToString() : supplierInvoiceDetail.TaxTypeDescription);
                                        supplierInvoiceDetail.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : supplierInvoiceDetail.Quantity);
                                        supplierInvoiceDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : supplierInvoiceDetail.Rate);
                                        supplierInvoiceDetail.TradeDiscountAmount = (sdr["TradeDiscountAmount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountAmount"].ToString()) : supplierInvoiceDetail.TradeDiscountAmount);
                                        supplierInvoiceDetail.Total = (sdr["Total"].ToString() != "" ? decimal.Parse(sdr["Total"].ToString()) : supplierInvoiceDetail.Total);
                                        supplierInvoiceDetail.TaxableAmount = (sdr["TaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TaxableAmount"].ToString()) : supplierInvoiceDetail.TaxableAmount);

                                    }
                                    supplierInvoiceDetailList.Add(supplierInvoiceDetail);
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

            return supplierInvoiceDetailList;
        }

        public SupplierInvoiceDetail GetSupplierInvoiceDetail(Guid id)
        {
            SupplierInvoiceDetail supplierInvoiceDetail = new SupplierInvoiceDetail();
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
                        cmd.CommandText = "[AMC].[GetSupplierInvoiceDetail]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    supplierInvoiceDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : supplierInvoiceDetail.ID);
                                    supplierInvoiceDetail.MaterialID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : supplierInvoiceDetail.MaterialID);
                                    supplierInvoiceDetail.MaterialDesc = (sdr["MaterialDesc"].ToString() != "" ? sdr["MaterialDesc"].ToString() : supplierInvoiceDetail.MaterialDesc);
                                    supplierInvoiceDetail.MaterialTypeDesc = (sdr["MaterialTypeDesc"].ToString() != "" ? sdr["MaterialTypeDesc"].ToString() : supplierInvoiceDetail.MaterialTypeDesc);
                                    supplierInvoiceDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : supplierInvoiceDetail.UnitCode);
                                    supplierInvoiceDetail.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : supplierInvoiceDetail.MaterialCode);
                                    supplierInvoiceDetail.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : supplierInvoiceDetail.TaxTypeCode);
                                    supplierInvoiceDetail.TaxTypeDescription = (sdr["TaxTypeDescription"].ToString() != "" ? sdr["TaxTypeDescription"].ToString() : supplierInvoiceDetail.TaxTypeDescription);
                                    supplierInvoiceDetail.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : supplierInvoiceDetail.Quantity);
                                    supplierInvoiceDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : supplierInvoiceDetail.Rate);
                                    supplierInvoiceDetail.TradeDiscountAmount = (sdr["TradeDiscountAmount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountAmount"].ToString()) : supplierInvoiceDetail.TradeDiscountAmount);
                                    supplierInvoiceDetail.Total = (sdr["Total"].ToString() != "" ? decimal.Parse(sdr["Total"].ToString()) : supplierInvoiceDetail.Total);
                                    supplierInvoiceDetail.TaxableAmount = (sdr["TaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TaxableAmount"].ToString()) : supplierInvoiceDetail.TaxableAmount);
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
            return supplierInvoiceDetail;
        }
    }
}
