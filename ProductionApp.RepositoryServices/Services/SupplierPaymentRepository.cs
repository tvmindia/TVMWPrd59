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
    public class SupplierPaymentRepository : ISupplierPaymentRepository
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
                        cmd.Connection = con;  
                        cmd.CommandText = "[AMC].[GetAllSupplierPayment]"; 
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
                                        supplierPayment.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : supplierPayment.EntryNo);
                                        supplierPayment.SupplierName = (sdr["Supplier"].ToString() != "" ? sdr["Supplier"].ToString() : supplierPayment.SupplierName);
                                        supplierPayment.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : supplierPayment.PaymentDateFormatted);
                                        supplierPayment.PaymentDate = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()) : supplierPayment.PaymentDate);
                                        supplierPayment.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : supplierPayment.PaymentRef);
                                        supplierPayment.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : supplierPayment.PaymentMode);
                                        supplierPayment.TotalPaidAmt = (sdr["AmountPaid"].ToString() != "" ? Decimal.Parse(sdr["AmountPaid"].ToString()) : supplierPayment.TotalPaidAmt);
                                        supplierPayment.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : supplierPayment.AdvanceAmount);
                                        supplierPayment.Type = (sdr["TypeDesc"].ToString() != "" ? sdr["TypeDesc"].ToString() : supplierPayment.Type);
                                        supplierPayment.CreditNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : supplierPayment.CreditNo);
                                        supplierPayment.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : supplierPayment.TotalCount);
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

        public List<SupplierInvoice> GetOutStandingSupplierInvoices(Guid PaymentID, Guid supplierId)
        {
            List<SupplierInvoice> SupplierInvoicesList = null;
            Settings settings = new Settings();
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
                        cmd.CommandText = "[AMC].[GetOutStandingSupplierInvoices]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = supplierId;
                        cmd.Parameters.Add("@PaymentID", SqlDbType.UniqueIdentifier).Value = PaymentID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierInvoicesList = new List<SupplierInvoice>();
                                while (sdr.Read())
                                {
                                    SupplierInvoice supplierInvoice = new SupplierInvoice();
                                    {
                                        supplierInvoice.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : supplierInvoice.ID);
                                        supplierInvoice.InvoiceNo = sdr["InvoiceNo"].ToString();
                                        supplierInvoice.InvoiceAmount = (sdr["TotalInvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["TotalInvoiceAmount"].ToString()) : supplierInvoice.InvoiceAmount);
                                        supplierInvoice.Balance = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : supplierInvoice.Balance);
                                        supplierInvoice.PaymentReceived = (sdr["OtherPayments"].ToString() != "" ? Decimal.Parse(sdr["OtherPayments"].ToString()) : supplierInvoice.PaymentReceived);
                                        // supplierInvoice.OtherPayments = (sdr["OtherPayments"].ToString() != "" ? Decimal.Parse(sdr["OtherPayments"].ToString()) : SupplierInvoice.OtherPayments);
                                        supplierInvoice.SupplierPayment = new SupplierPayment();
                                        supplierInvoice.SupplierPayment.SupplierPaymentDetail = new SupplierPaymentDetail();
                                        supplierInvoice.SupplierPayment.SupplierPaymentDetail.PaidAmount = (sdr["PaidAmountEdit"].ToString() != "" ? Decimal.Parse(sdr["PaidAmountEdit"].ToString()) : supplierInvoice.SupplierPayment.SupplierPaymentDetail.PaidAmount);
                                        supplierInvoice.SupplierPayment.SupplierPaymentDetail.ID = (sdr["PaymentDetailID"].ToString() != "" ? Guid.Parse(sdr["PaymentDetailID"].ToString()) : supplierInvoice.SupplierPayment.SupplierPaymentDetail.ID);
                                        supplierInvoice.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : supplierInvoice.InvoiceDateFormatted);
                                        supplierInvoice.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : supplierInvoice.PaymentDueDateFormatted);
                                    }
                                    SupplierInvoicesList.Add(supplierInvoice);
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

            return SupplierInvoicesList;
        }

        public SupplierInvoice GetOutstandingAmount(Guid Id)
        {
            SupplierInvoice supplierInvoice = null;
            Settings settings = new Settings();
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
                        cmd.CommandText = "[AMC].[GetOutstandingAmountBySupplier]";
                        cmd.Parameters.Add("@SupplierId", SqlDbType.UniqueIdentifier).Value = Id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                supplierInvoice = new SupplierInvoice();
                                while (sdr.Read())
                                {

                                    supplierInvoice.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : supplierInvoice.SupplierID);
                                    supplierInvoice.InvoiceAmount = (sdr["OutstandingAmount"].ToString() != "" ? Decimal.Parse(sdr["OutstandingAmount"].ToString()) : supplierInvoice.InvoiceAmount);
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

        public object InsertUpdateSupplierPayment(SupplierPayment supplierPayment)
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
                        cmd.CommandText = "[AMC].[InsertUpdateSupplierPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = supplierPayment.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = supplierPayment.ID;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = supplierPayment.SupplierID;
                        cmd.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = supplierPayment.PaymentDateFormatted;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = supplierPayment.ChequeDateFormatted;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = supplierPayment.PaymentMode;
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 10).Value = supplierPayment.BankCode;
                        cmd.Parameters.Add("@PaymentRef", SqlDbType.VarChar, 10).Value = supplierPayment.PaymentRef;
                        cmd.Parameters.Add("@Refbank", SqlDbType.NVarChar, 50).Value = supplierPayment.ReferenceBank;
                        cmd.Parameters.Add("@TotalPaidAmt", SqlDbType.Decimal).Value = supplierPayment.TotalPaidAmt;
                        cmd.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = supplierPayment.AdvanceAmount;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 1).Value = supplierPayment.Type;
                        //cmd.Parameters.Add("@CreditID", SqlDbType.UniqueIdentifier).Value = supplierPayment.CreditID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = supplierPayment.GeneralNotes;
                        cmd.Parameters.Add("@DepWithdID", SqlDbType.UniqueIdentifier).Value = supplierPayment.DepositWithdrawalID;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = supplierPayment.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = supplierPayment.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = supplierPayment.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = supplierPayment.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = supplierPayment.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier, 5);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    switch (outputStatus.Value.ToString())
                    {
                        case "0":
                            throw new Exception(supplierPayment.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                        case "1":
                            return new
                            {
                                ID = IDOut.Value.ToString(),
                                Status = outputStatus.Value.ToString(),
                                Message = supplierPayment.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                            };
                    }
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
                Message = supplierPayment.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }

        public SupplierPayment GetSupplierPayment(string Id)
        {
            SupplierPayment supplierPayment = new SupplierPayment();
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(Id);
                        cmd.CommandText = "[AMC].[GetSupplierPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    supplierPayment.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : supplierPayment.ID);
                                    supplierPayment.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : supplierPayment.PaymentDateFormatted);
                                    supplierPayment.ChequeDateFormatted = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : supplierPayment.ChequeDateFormatted);
                                    supplierPayment.PaymentDate = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()) : supplierPayment.PaymentDate);
                                    supplierPayment.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()) : supplierPayment.ChequeDate);
                                    supplierPayment.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : supplierPayment.PaymentRef);
                                    supplierPayment.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : supplierPayment.EntryNo);
                                    supplierPayment.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : supplierPayment.PaymentMode);
                                    supplierPayment.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : supplierPayment.Type);
                                    supplierPayment.ReferenceBank = (sdr["ReferenceBank"].ToString() != "" ? sdr["ReferenceBank"].ToString() : supplierPayment.ReferenceBank);
                                    supplierPayment.CreditID = (sdr["CreditID"].ToString() != "" ? Guid.Parse(sdr["CreditID"].ToString()) : supplierPayment.CreditID);
                                    supplierPayment.CreditNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : supplierPayment.CreditNo);
                                    supplierPayment.TotalPaidAmt = (sdr["AmountPaid"].ToString() != "" ? Decimal.Parse(sdr["AmountPaid"].ToString()) : supplierPayment.TotalPaidAmt);
                                    supplierPayment.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : supplierPayment.AdvanceAmount);
                                    supplierPayment.BankCode = (sdr["BankCode"].ToString() != "" ? sdr["BankCode"].ToString() : supplierPayment.BankCode);
                                    supplierPayment.DepositWithdrawalID = (sdr["DepositWithdrawalID"].ToString() != "" ? Guid.Parse(sdr["DepositWithdrawalID"].ToString()) : supplierPayment.DepositWithdrawalID);
                                    supplierPayment.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : supplierPayment.GeneralNotes);
                                    supplierPayment.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : supplierPayment.SupplierID);
                                    supplierPayment.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : supplierPayment.ApprovalStatus);
                                    supplierPayment.LatestApprovalStatus = (sdr["LatestApprovalStatus"].ToString() != "" ? Int32.Parse(sdr["LatestApprovalStatus"].ToString()) : supplierPayment.LatestApprovalStatus);
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
            return supplierPayment;


        }

        public object DeleteSupplierPayment(Guid id,string userName)
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
                        cmd.CommandText = "[AMC].[DeleteSupplierPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.Parameters.Add("@Username", SqlDbType.NVarChar,20).Value = userName;
                        
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
            };
        }

        public object ValidateSupplierPayment(Guid id, string paymentrefNo)
        {
            SqlParameter outputStatus, message = null;
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
                        cmd.CommandText = "[AMC].[ValidateSupplierPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ReferenceNo", SqlDbType.VarChar, 20).Value = paymentrefNo;
                        if (id != Guid.Empty)
                            cmd.Parameters.Add("@id", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        message = cmd.Parameters.Add("@message", SqlDbType.VarChar, 100);
                        outputStatus.Direction = ParameterDirection.Output;
                        message.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return new { Message = ex.ToString(), Status = -1 };
            }
            return new { Message = message.Value.ToString(), Status = outputStatus.Value };
        }

    }
}
