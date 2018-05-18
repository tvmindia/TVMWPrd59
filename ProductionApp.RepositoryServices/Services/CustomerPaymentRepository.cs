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
    public class CustomerPaymentRepository : ICustomerPaymentRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public CustomerPaymentRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetAllCustomerPayments
        public List<CustomerPayment> GetAllCustomerPayment(CustomerPaymentAdvanceSearch customerPaymentAdvanceSearch)
        {
            List<CustomerPayment> customerPaymentList = null;
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
                        cmd.CommandText = "[AMC].[GetAllCustomerPayment]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(customerPaymentAdvanceSearch.SearchTerm) ? "" : customerPaymentAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = customerPaymentAdvanceSearch.DataTablePaging.Start;
                        if (customerPaymentAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = customerPaymentAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = customerPaymentAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = customerPaymentAdvanceSearch.ToDate;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = customerPaymentAdvanceSearch.PaymentMode;
                        if (customerPaymentAdvanceSearch.CustomerID != Guid.Empty)
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = customerPaymentAdvanceSearch.CustomerID;
                        //if (customerPaymentAdvanceSearch.SupplierID != Guid.Empty)
                        //    cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = materialReturnAdvanceSearch.SupplierID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerPaymentList = new List<CustomerPayment>();
                                while (sdr.Read())
                                {
                                    CustomerPayment customerPayment = new CustomerPayment();
                                    {
                                        customerPayment.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customerPayment.ID);
                                        customerPayment.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : customerPayment.PaymentDateFormatted);
                                        customerPayment.PaymentDate = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()) : customerPayment.PaymentDate);
                                        customerPayment.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : customerPayment.PaymentRef);
                                        customerPayment.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : customerPayment.EntryNo);
                                        customerPayment.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : customerPayment.PaymentMode);
                                        customerPayment.TotalRecievedAmt = (sdr["AmountReceived"].ToString() != "" ? Decimal.Parse(sdr["AmountReceived"].ToString()) : customerPayment.TotalRecievedAmt);
                                        customerPayment.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : customerPayment.AdvanceAmount);
                                        customerPayment.Type = (sdr["TypeDesc"].ToString() != "" ? sdr["TypeDesc"].ToString() : customerPayment.Type);
                                        customerPayment.CustomerName = (sdr["Customer"].ToString() != "" ? sdr["Customer"].ToString() : customerPayment.CustomerName);
                                        customerPayment.CreditNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : customerPayment.CreditNo);
                                        customerPayment.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : customerPayment.TotalCount);
                                        customerPayment.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : customerPayment.FilteredCount);
                                    }
                                    customerPaymentList.Add(customerPayment);
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
            return customerPaymentList;
        }
        #endregion

        #region InsertUpdateCustomerPayment
        public object InsertUpdateCustomerPayment(CustomerPayment customerPayment)
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
                        cmd.CommandText = "[AMC].[InsertUpdateCustomerPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = customerPayment.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = customerPayment.ID;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = customerPayment.CustomerID;
                        cmd.Parameters.Add("@PaymentDate", SqlDbType.DateTime).Value = customerPayment.PaymentDateFormatted;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = customerPayment.ChequeDateFormatted;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = customerPayment.PaymentMode;
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 10).Value = customerPayment.BankCode;
                        cmd.Parameters.Add("@PaymentRef", SqlDbType.VarChar, 10).Value = customerPayment.PaymentRef;
                        cmd.Parameters.Add("@Refbank", SqlDbType.NVarChar, 50).Value = customerPayment.ReferenceBank;
                        cmd.Parameters.Add("@TotalRecdAmt", SqlDbType.Decimal).Value = customerPayment.TotalRecievedAmt;
                        cmd.Parameters.Add("@AdvanceAmount", SqlDbType.Decimal).Value = customerPayment.AdvanceAmount;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar, 1).Value = customerPayment.Type;
                        //cmd.Parameters.Add("@CreditID", SqlDbType.UniqueIdentifier).Value = customerPayment.CreditID;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = customerPayment.GeneralNotes; 
                        cmd.Parameters.Add("@DepWithdID", SqlDbType.UniqueIdentifier).Value = customerPayment.DepositWithdrawalID;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = customerPayment.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = customerPayment.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = customerPayment.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = customerPayment.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = customerPayment.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier, 5);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    switch (outputStatus.Value.ToString())
                    {
                        case "0":
                            throw new Exception(customerPayment.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                        case "1":
                            return new
                            {
                                ID = IDOut.Value.ToString(),
                                Status = outputStatus.Value.ToString(),
                                Message = customerPayment.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = customerPayment.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateCustomerPayment

        #region GetOutStandingInvoices
        public List<CustomerInvoice> GetOutStandingInvoices(Guid PaymentID, Guid CustID)
        {
            List<CustomerInvoice> CustomerInvoicesList = null;
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
                        cmd.CommandText = "[AMC].[GetOutStandingInvoices]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = CustID;
                        cmd.Parameters.Add("@PaymentID", SqlDbType.UniqueIdentifier).Value = PaymentID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                CustomerInvoicesList = new List<CustomerInvoice>();
                                while (sdr.Read())
                                {
                                    CustomerInvoice customerInvoice = new CustomerInvoice();
                                    {
                                        customerInvoice.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customerInvoice.ID);
                                        customerInvoice.InvoiceDate = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()) : customerInvoice.InvoiceDate);
                                        customerInvoice.InvoiceNo = sdr["InvoiceNo"].ToString();
                                        customerInvoice.PaymentDueDate = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()) : customerInvoice.PaymentDueDate);
                                        customerInvoice.InvoiceAmount = (sdr["TotalInvoiceAmount"].ToString() != "" ? Decimal.Parse(sdr["TotalInvoiceAmount"].ToString()) : customerInvoice.InvoiceAmount);
                                        customerInvoice.Balance = (sdr["BalanceDue"].ToString() != "" ? Decimal.Parse(sdr["BalanceDue"].ToString()) : customerInvoice.Balance);
                                        customerInvoice.PaymentReceived = (sdr["OtherPayments"].ToString() != "" ? Decimal.Parse(sdr["OtherPayments"].ToString()) : customerInvoice.PaymentReceived);
                                        customerInvoice.PaymentDueDate = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()) : customerInvoice.PaymentDueDate);
                                        //customerInvoice.OtherPayments = (sdr["OtherPayments"].ToString() != "" ? Decimal.Parse(sdr["OtherPayments"].ToString()) : customerInvoice.OtherPayments);
                                        customerInvoice.CustomerPayment = new CustomerPayment();
                                        customerInvoice.CustomerPayment.CustomerPaymentDetail = new CustomerPaymentDetail();
                                        customerInvoice.CustomerPayment.CustomerPaymentDetail.PaidAmount = (sdr["PaidAmountEdit"].ToString() != "" ? Decimal.Parse(sdr["PaidAmountEdit"].ToString()) : customerInvoice.CustomerPayment.CustomerPaymentDetail.PaidAmount);
                                        customerInvoice.CustomerPayment.CustomerPaymentDetail.ID = (sdr["PaymentDetailID"].ToString() != "" ? Guid.Parse(sdr["PaymentDetailID"].ToString()) : customerInvoice.CustomerPayment.CustomerPaymentDetail.ID);
                                        customerInvoice.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : customerInvoice.InvoiceDateFormatted);
                                        customerInvoice.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : customerInvoice.PaymentDueDateFormatted);
                                    }
                                    CustomerInvoicesList.Add(customerInvoice);
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

            return CustomerInvoicesList;
        }
        #endregion

        #region ValidateCustomerPayment
        public object ValidateCustomerPayment(Guid id,string paymentrefNo)
        {
            SqlParameter outputStatus,message = null;
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
                        cmd.CommandText = "[AMC].[ValidateCustomerPayment]";
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
        #endregion ValidateCustomerPayment

        #region GetCustomerPayment
        public CustomerPayment GetCustomerPayment(string Id)
        {
            CustomerPayment customerPayment = new CustomerPayment();
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
                        cmd.CommandText = "[AMC].[GetCustomerPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    customerPayment.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customerPayment.ID);
                                    customerPayment.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : customerPayment.PaymentDateFormatted);
                                    customerPayment.ChequeDateFormatted = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : customerPayment.ChequeDateFormatted);
                                    customerPayment.PaymentDate = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()) : customerPayment.PaymentDate);
                                    customerPayment.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()) : customerPayment.ChequeDate);
                                    customerPayment.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : customerPayment.PaymentRef);
                                    customerPayment.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : customerPayment.EntryNo);
                                    customerPayment.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : customerPayment.PaymentMode);
                                    customerPayment.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : customerPayment.Type);
                                    customerPayment.ReferenceBank = (sdr["ReferenceBank"].ToString() != "" ? sdr["ReferenceBank"].ToString() : customerPayment.ReferenceBank);
                                    customerPayment.CreditID = (sdr["CreditID"].ToString() != "" ? Guid.Parse(sdr["CreditID"].ToString()) : customerPayment.CreditID);
                                    customerPayment.CreditNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : customerPayment.CreditNo);
                                    customerPayment.TotalRecievedAmt = (sdr["AmountReceived"].ToString() != "" ? Decimal.Parse(sdr["AmountReceived"].ToString()) : customerPayment.TotalRecievedAmt);
                                    customerPayment.AdvanceAmount = (sdr["AdvanceAmount"].ToString() != "" ? Decimal.Parse(sdr["AdvanceAmount"].ToString()) : customerPayment.AdvanceAmount);
                                    customerPayment.BankCode = (sdr["BankCode"].ToString() != "" ? sdr["BankCode"].ToString() : customerPayment.BankCode);
                                    customerPayment.DepositWithdrawalID = (sdr["DepositWithdrawalID"].ToString() != "" ? Guid.Parse(sdr["DepositWithdrawalID"].ToString()) : customerPayment.DepositWithdrawalID);
                                    customerPayment.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : customerPayment.GeneralNotes);
                                    customerPayment.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : customerPayment.CustomerID);
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
            return customerPayment;

        }
        #endregion GetCustomerPayment

        #region DeleteCustomerPayment
        public object DeleteCustomerPayment(Guid id,string userName)
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
                        cmd.CommandText = "[AMC].[DeleteCustomerPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = userName;
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
        #endregion DeleteCustomerPayment

        #region GetOutstandingAmount
        public CustomerInvoice GetOutstandingAmount(Guid Id)
        {
            CustomerInvoice customerInvoice = null;
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
                        cmd.CommandText = "[AMC].[GetOutstandingAmountByCustomer]";
                        cmd.Parameters.Add("@CustomerId", SqlDbType.UniqueIdentifier).Value = Id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerInvoice = new CustomerInvoice();
                                while (sdr.Read())
                                {

                                    customerInvoice.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : customerInvoice.CustomerID);
                                    customerInvoice.InvoiceAmount = (sdr["OutstandingAmount"].ToString() != "" ? Decimal.Parse(sdr["OutstandingAmount"].ToString()) : customerInvoice.InvoiceAmount);
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

            return customerInvoice;
        }
        #endregion GetOutstandingAmount

        #region GetRecentCustomerPayment
        public List<CustomerPayment> GetRecentCustomerPayment()
        {
            List<CustomerPayment> customerPaymentList = new List<CustomerPayment>();
            CustomerPayment customerPayment = null;
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
                        cmd.CommandText = "[AMC].[GetRecentCustomerPayment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    customerPayment = new CustomerPayment();
                                    customerPayment.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);
                                    customerPayment.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : customerPayment.EntryNo);
                                    customerPayment.PaymentDateFormatted = (sdr["PaymentDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDate"].ToString()).ToString(settings.DateFormat) : customerPayment.PaymentDateFormatted);
                                    customerPayment.Customer = new Customer();
                                    customerPayment.Customer.CompanyName = (sdr["Customer"].ToString() != "" ? sdr["Customer"].ToString() : customerPayment.Customer.CompanyName);
                                    customerPaymentList.Add(customerPayment);
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
            return customerPaymentList;
        }
        #endregion GetRecentCustomerPayment
    }
}
