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
    public class CustomerInvoiceRepository : ICustomerInvoiceRepository
    {
        private IDatabaseFactory _databaseFactory;

        Settings settings = new Settings();
        AppConst _appConst = new AppConst();

        public CustomerInvoiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<CustomerInvoiceDetail> GetPackingSlipListDetail(string packingSlipIDs, string id)
        {
            List<CustomerInvoiceDetail> customerInvoiceDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetPackingSlipListDetailForCustomerInvoice_grouping]";
                        cmd.Parameters.Add("@PackingSlipIDs", SqlDbType.NVarChar, -1).Value = packingSlipIDs;
                        if (id != null && Guid.Parse(id) != Guid.Empty)
                            cmd.Parameters.Add("@CustomerInvoiceID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(id);
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerInvoiceDetailList = new List<CustomerInvoiceDetail>();
                                while (sdr.Read())
                                {
                                    CustomerInvoiceDetail customerInvoiceDetail = new CustomerInvoiceDetail();

                                    {
                                        customerInvoiceDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : customerInvoiceDetail.ProductID);
                                        customerInvoiceDetail.ProductName = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : customerInvoiceDetail.ProductName);
                                        customerInvoiceDetail.GroupID = (sdr["GroupID"].ToString() != "" ? Guid.Parse(sdr["GroupID"].ToString()) : customerInvoiceDetail.GroupID);
                                        customerInvoiceDetail.GroupName = (sdr["GroupName"].ToString() != "" ? sdr["GroupName"].ToString() : customerInvoiceDetail.GroupName);
                                        customerInvoiceDetail.SlipNo = (sdr["SlipNo"].ToString() != "" ? sdr["SlipNo"].ToString() : customerInvoiceDetail.SlipNo);
                                        customerInvoiceDetail.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : customerInvoiceDetail.Quantity);
                                        customerInvoiceDetail.Weight = (sdr["Weight"].ToString() != "" ? decimal.Parse(sdr["Weight"].ToString()) : customerInvoiceDetail.Weight);
                                        customerInvoiceDetail.PackingSlipDetailID = (sdr["PackingSlipDetailID"].ToString() != "" ? Guid.Parse(sdr["PackingSlipDetailID"].ToString()) : customerInvoiceDetail.PackingSlipDetailID);
                                        customerInvoiceDetail.QuantityCheck = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : customerInvoiceDetail.QuantityCheck);
                                        customerInvoiceDetail.WeightCheck = (sdr["Weight"].ToString() != "" ? decimal.Parse(sdr["Weight"].ToString()) : customerInvoiceDetail.WeightCheck);
                                        customerInvoiceDetail.IsInvoiceInKG = (sdr["IsInvoiceInKG"].ToString() != "" ? bool.Parse(sdr["IsInvoiceInKG"].ToString()) : customerInvoiceDetail.IsInvoiceInKG);
                                        customerInvoiceDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : customerInvoiceDetail.Rate);
                                        customerInvoiceDetail.TradeDiscountPerc = (sdr["TradeDiscountPerc"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountPerc"].ToString()) : customerInvoiceDetail.TradeDiscountPerc);
                                        if (customerInvoiceDetail.TradeDiscountPerc != 0)
                                        {
                                            if(customerInvoiceDetail.IsInvoiceInKG ==false)
                                                customerInvoiceDetail.TradeDiscountAmount = customerInvoiceDetail.Quantity * customerInvoiceDetail.Rate * (customerInvoiceDetail.TradeDiscountPerc / 100);
                                            else
                                                customerInvoiceDetail.TradeDiscountAmount = customerInvoiceDetail.Weight * customerInvoiceDetail.Rate * (customerInvoiceDetail.TradeDiscountPerc / 100);
                                        }
                                            
                                        else
                                            customerInvoiceDetail.TradeDiscountAmount = 0;
                                        customerInvoiceDetail.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : customerInvoiceDetail.TaxTypeCode);
                                        customerInvoiceDetail.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : customerInvoiceDetail.SGSTPerc);
                                        customerInvoiceDetail.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : customerInvoiceDetail.CGSTPerc);
                                        customerInvoiceDetail.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : customerInvoiceDetail.IGSTPerc);
                                        customerInvoiceDetail.Total = (sdr["Total"].ToString() != "" ? decimal.Parse(sdr["Total"].ToString()) : customerInvoiceDetail.Total);
                                      //  customerInvoiceDetail.TaxTypeDescription = (sdr["TaxTypeDesc"].ToString() != "" ? sdr["TaxTypeDesc"].ToString() : customerInvoiceDetail.TaxTypeDescription);

                                    }
                                    customerInvoiceDetailList.Add(customerInvoiceDetail);
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
            return customerInvoiceDetailList;
        }

        public object InsertUpdateCustomerInvoice(CustomerInvoice customerInvoice)
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
                        if (customerInvoice.hdnIsRegular || customerInvoice.InvoiceType=="RI")
                            cmd.CommandText = "[AMC].[InsertUpdateCustomerInvoice_Grouping]";
                        else
                            cmd.CommandText = "[AMC].[InsertUpdateCustomerServiceInvoice]";

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = customerInvoice.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = customerInvoice.ID;
                        if (customerInvoice.CustomerID != Guid.Empty)
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = customerInvoice.CustomerID;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = customerInvoice.hdnFileID;
                        cmd.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar, 10).Value = customerInvoice.PaymentTermCode;
                        cmd.Parameters.Add("@InvoiceType", SqlDbType.VarChar, 2).Value = customerInvoice.InvoiceType;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = customerInvoice.InvoiceDateFormatted;
                        cmd.Parameters.Add("@PaymentDueDate", SqlDbType.DateTime).Value = customerInvoice.PaymentDueDateFormatted;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.VarChar, -1).Value = customerInvoice.DetailXML;
                        cmd.Parameters.Add("@Discount", SqlDbType.Decimal).Value = customerInvoice.Discount;

                        cmd.Parameters.Add("@BillingAddress", SqlDbType.VarChar, -1).Value = customerInvoice.BillingAddress;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar, -1).Value = customerInvoice.GeneralNotes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = customerInvoice.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = customerInvoice.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = customerInvoice.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = customerInvoice.Common.UpdatedDate;
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
                        throw new Exception(customerInvoice.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        return new
                        {
                            ID = IDOut.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = customerInvoice.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = customerInvoice.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }

        public List<PackingSlip> GetPackingSlipList(Guid customerID)
        {
            List<PackingSlip> packingSlipList = null;
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
                        cmd.CommandText = "[AMC].[GetPackingSlipListForCustomerInvoice]";
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = customerID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                packingSlipList = new List<PackingSlip>();
                                while (sdr.Read())
                                {
                                    PackingSlip packingSlip = new PackingSlip();
                                    {
                                        packingSlip.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : packingSlip.ID);
                                        packingSlip.SlipNo = (sdr["SlipNo"].ToString() != "" ? sdr["SlipNo"].ToString() : packingSlip.SlipNo);
                                        packingSlip.DateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.DateFormat) : packingSlip.DateFormatted);
                                        packingSlip.SalesOrder = new SalesOrder();
                                        packingSlip.SalesOrder.OrderNo = (sdr["OrderNo"].ToString() != "" ? sdr["OrderNo"].ToString() : packingSlip.SalesOrder.OrderNo);
                                        packingSlip.SalesOrder.SalesPersonName = (sdr["SalesPersonName"].ToString() != "" ? sdr["SalesPersonName"].ToString() : packingSlip.SalesOrder.SalesPersonName);
                                        packingSlip.SalesOrder.ReferenceCustomerName = (sdr["ReferenceCustomerName"].ToString() != "" ? sdr["ReferenceCustomerName"].ToString() : packingSlip.SalesOrder.ReferenceCustomerName);
                                    }
                                    packingSlipList.Add(packingSlip);
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
            return packingSlipList;
        }

        public CustomerInvoice GetCustomerInvoice(Guid id)
        {
            CustomerInvoice customerInvoice = new CustomerInvoice();
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
                        cmd.CommandText = "[AMC].[GetCustomerInvoice]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    customerInvoice.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : customerInvoice.InvoiceNo);
                                    customerInvoice.InvoiceType = (sdr["InvoiceType"].ToString() != "" ? sdr["InvoiceType"].ToString() : customerInvoice.InvoiceType);
                                    customerInvoice.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.DateFormat) : customerInvoice.InvoiceDateFormatted);
                                    customerInvoice.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.DateFormat) : customerInvoice.PaymentDueDateFormatted);
                                    customerInvoice.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : customerInvoice.CustomerID);
                                    customerInvoice.Customer = new Customer();
                                    customerInvoice.Customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : customerInvoice.Customer.CompanyName);
                                    customerInvoice.Customer.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : customerInvoice.Customer.ContactEmail);
                                    customerInvoice.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : customerInvoice.GeneralNotes);
                                    customerInvoice.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : customerInvoice.BillingAddress);
                                    customerInvoice.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : customerInvoice.PaymentTermCode);
                                    customerInvoice.Discount = (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : customerInvoice.Discount);
                                    customerInvoice.TotalTaxableAmount = (sdr["TaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TaxableAmount"].ToString()) : customerInvoice.TotalTaxableAmount);
                                    customerInvoice.TotalTaxAmount = (sdr["TaxAmount"].ToString() != "" ? decimal.Parse(sdr["TaxAmount"].ToString()) : customerInvoice.TotalTaxAmount);
                                    customerInvoice.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? decimal.Parse(sdr["InvoiceAmount"].ToString()) : customerInvoice.InvoiceAmount);
                                    customerInvoice.PaymentReceived = (sdr["PaidAmount"].ToString() != "" ? decimal.Parse(sdr["PaidAmount"].ToString()) : customerInvoice.PaymentReceived);
                                    customerInvoice.CompanyAddress = (sdr["EmailFrom"].ToString() != "" ? sdr["EmailFrom"].ToString() : customerInvoice.CompanyAddress);
                                    customerInvoice.CompanyName = (sdr["Company"].ToString() != "" ? sdr["Company"].ToString() : customerInvoice.CompanyName);
                                    customerInvoice.LogoURL = (sdr["Logo"].ToString() != "" ? sdr["Logo"].ToString() : customerInvoice.LogoURL);
                                    customerInvoice.BankName = (sdr["BankName"].ToString() != "" ? sdr["BankName"].ToString() : customerInvoice.BankName);
                                    customerInvoice.BankAccDetail = (sdr["BankAccDetail"].ToString() != "" ? sdr["BankAccDetail"].ToString() : customerInvoice.BankAccDetail);
                                    customerInvoice.InvoiceDeclaration = (sdr["InvoiceDeclaration"].ToString() != "" ? sdr["InvoiceDeclaration"].ToString() : customerInvoice.InvoiceDeclaration);
                                    customerInvoice.TotalCGSTAmount = (sdr["TotalCGST"].ToString() != "" ? decimal.Parse(sdr["TotalCGST"].ToString()) : customerInvoice.TotalCGSTAmount);
                                    customerInvoice.TotalSGSTAmount = (sdr["TotalSGST"].ToString() != "" ? decimal.Parse(sdr["TotalSGST"].ToString()) : customerInvoice.TotalSGSTAmount);
                                    customerInvoice.TotalIGSTAmount = (sdr["TotalIGST"].ToString() != "" ? decimal.Parse(sdr["TotalIGST"].ToString()) : customerInvoice.TotalIGSTAmount);
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

        public List<CustomerInvoiceDetail> GetCustomerInvoiceDetail(Guid id)
        {
            List<CustomerInvoiceDetail> customerInvoiceDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetCustomerInvoiceDetail_Group]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@customerInvoiceId", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerInvoiceDetailList = new List<CustomerInvoiceDetail>();
                                while (sdr.Read())
                                {
                                    CustomerInvoiceDetail customerInvoiceDetail = new CustomerInvoiceDetail();
                                    {
                                        customerInvoiceDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customerInvoiceDetail.ID);
                                        customerInvoiceDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : customerInvoiceDetail.ProductID);
                                        customerInvoiceDetail.ServiceItemID = (sdr["ServiceItemID"].ToString() != "" ? Guid.Parse(sdr["ServiceItemID"].ToString()) : customerInvoiceDetail.ServiceItemID);
                                        customerInvoiceDetail.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : customerInvoiceDetail.TaxTypeCode);
                                        customerInvoiceDetail.TaxTypeDescription = (sdr["TaxTypeDescription"].ToString() != "" ? sdr["TaxTypeDescription"].ToString() : customerInvoiceDetail.TaxTypeDescription);
                                        customerInvoiceDetail.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : customerInvoiceDetail.CGSTPerc);
                                        customerInvoiceDetail.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : customerInvoiceDetail.IGSTPerc);
                                        customerInvoiceDetail.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : customerInvoiceDetail.SGSTPerc);
                                        customerInvoiceDetail.ProductName = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : customerInvoiceDetail.ProductName);
                                        customerInvoiceDetail.Product = new Product();
                                        customerInvoiceDetail.Product.HSNNo = (sdr["HSNNo"].ToString() != "" ? sdr["HSNNo"].ToString() : customerInvoiceDetail.Product.HSNNo);
                                        customerInvoiceDetail.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : customerInvoiceDetail.Quantity);
                                        customerInvoiceDetail.Weight = (sdr["Weight"].ToString() != "" ? decimal.Parse(sdr["Weight"].ToString()) : customerInvoiceDetail.Weight);
                                        customerInvoiceDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : customerInvoiceDetail.Rate);
                                        customerInvoiceDetail.IsInvoiceInKG = (sdr["IsInvoiceInKG"].ToString() != "" ? bool.Parse(sdr["IsInvoiceInKG"].ToString()) : customerInvoiceDetail.IsInvoiceInKG);
                                        customerInvoiceDetail.TradeDiscountAmount = (sdr["TradeDiscountAmount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountAmount"].ToString()) : customerInvoiceDetail.TradeDiscountAmount);
                                        customerInvoiceDetail.Total = (sdr["Total"].ToString() != "" ? decimal.Parse(sdr["Total"].ToString()) : customerInvoiceDetail.Total);
                                        customerInvoiceDetail.TaxableAmount = (sdr["TaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TaxableAmount"].ToString()) : customerInvoiceDetail.TaxableAmount);
                                        customerInvoiceDetail.GroupID = (sdr["GroupID"].ToString() != "" ? Guid.Parse(sdr["GroupID"].ToString()) : customerInvoiceDetail.GroupID);
                                        customerInvoiceDetail.GroupName = (sdr["GroupName"].ToString() != "" ? sdr["GroupName"].ToString() : customerInvoiceDetail.GroupName);

                                    }
                                    customerInvoiceDetailList.Add(customerInvoiceDetail);
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

            return customerInvoiceDetailList;
        }

        public List<CustomerInvoice> GetAllCustomerInvoice(CustomerInvoiceAdvanceSearch customerInvoiceAdvanceSearch)
        {
            List<CustomerInvoice> CustomerInvoiceList = null;
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
                        cmd.CommandText = "[AMC].[GetAllCustomerInvocie]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(customerInvoiceAdvanceSearch.SearchTerm) ? "" : customerInvoiceAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = customerInvoiceAdvanceSearch.DataTablePaging.Start;
                        if (customerInvoiceAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = customerInvoiceAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = customerInvoiceAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = customerInvoiceAdvanceSearch.ToDate;
                        if (customerInvoiceAdvanceSearch.CustomerID != Guid.Empty)
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = customerInvoiceAdvanceSearch.CustomerID;
                        if(customerInvoiceAdvanceSearch.InvoiceType != "")
                            cmd.Parameters.Add("@InvoiceType", SqlDbType.NVarChar,2).Value = customerInvoiceAdvanceSearch.InvoiceType;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                CustomerInvoiceList = new List<CustomerInvoice>();
                                while (sdr.Read())
                                {
                                    CustomerInvoice customerInvoice = new CustomerInvoice();
                                    {
                                        customerInvoice.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customerInvoice.ID);
                                        customerInvoice.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : customerInvoice.InvoiceNo);
                                        customerInvoice.InvoiceType = (sdr["InvoiceType"].ToString() != "" ? sdr["InvoiceType"].ToString() : customerInvoice.InvoiceType);
                                        customerInvoice.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.DateFormat) : customerInvoice.InvoiceDateFormatted);
                                        customerInvoice.PaymentDueDateFormatted = (sdr["PaymentDueDate"].ToString() != "" ? DateTime.Parse(sdr["PaymentDueDate"].ToString()).ToString(settings.DateFormat) : customerInvoice.PaymentDueDateFormatted);
                                        customerInvoice.Customer = new Customer();
                                        customerInvoice.Customer.CompanyName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : customerInvoice.Customer.CompanyName);
                                        customerInvoice.InvoiceAmount = (sdr["InvoiceAmount"].ToString() != "" ? decimal.Parse(sdr["InvoiceAmount"].ToString()) : customerInvoice.InvoiceAmount);
                                        customerInvoice.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : customerInvoice.TotalCount);
                                        customerInvoice.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : customerInvoice.FilteredCount);
                                        customerInvoice.Status = (sdr["Status"].ToString() != "" ? sdr["Status"].ToString() : customerInvoice.Status);
                                        customerInvoice.BalanceDue = (sdr["BalanceDue"].ToString() != "" ? decimal.Parse(sdr["BalanceDue"].ToString()) : customerInvoice.BalanceDue);
                                        customerInvoice.LastPaymentDateFormatted = (sdr["LastPaymentDate"].ToString() != "" ? DateTime.Parse(sdr["LastPaymentDate"].ToString()).ToString(settings.DateFormat) : customerInvoice.LastPaymentDateFormatted);
                                    }
                                    CustomerInvoiceList.Add(customerInvoice);
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

            return CustomerInvoiceList;
        }

        public List<CustomerInvoiceDetail> GetCustomerInvoiceDetailLinkForEdit(string id)
        {
            List<CustomerInvoiceDetail> customerInvoiceDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetCustomerInvoiceDetailLinkForEdit]";
                        if (id != null)
                            cmd.Parameters.Add("@CustomerInvoiceDetailID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(id);
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerInvoiceDetailList = new List<CustomerInvoiceDetail>();
                                while (sdr.Read())
                                {
                                    CustomerInvoiceDetail customerInvoiceDetail = new CustomerInvoiceDetail();
                                    {
                                        customerInvoiceDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customerInvoiceDetail.ID);
                                        customerInvoiceDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : customerInvoiceDetail.ProductID);
                                        customerInvoiceDetail.ProductName = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : customerInvoiceDetail.ProductName);
                                        customerInvoiceDetail.SlipNo = (sdr["SlipNo"].ToString() != "" ? sdr["SlipNo"].ToString() : customerInvoiceDetail.SlipNo);
                                        customerInvoiceDetail.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : customerInvoiceDetail.Quantity);
                                        customerInvoiceDetail.Weight = (sdr["Weight"].ToString() != "" ? decimal.Parse(sdr["Weight"].ToString()) : customerInvoiceDetail.Weight);
                                        customerInvoiceDetail.PackingSlipDetailID = (sdr["PackingSlipDetailID"].ToString() != "" ? Guid.Parse(sdr["PackingSlipDetailID"].ToString()) : customerInvoiceDetail.PackingSlipDetailID);
                                        customerInvoiceDetail.CustomerInvoiceDetailLinkID = (sdr["LinkID"].ToString() != "" ? Guid.Parse(sdr["LinkID"].ToString()) : customerInvoiceDetail.CustomerInvoiceDetailLinkID);
                                        customerInvoiceDetail.QuantityCheck = (sdr["QuantityCheck"].ToString() != "" ? decimal.Parse(sdr["QuantityCheck"].ToString()) : customerInvoiceDetail.QuantityCheck);
                                        customerInvoiceDetail.WeightCheck = (sdr["WeightCheck"].ToString() != "" ? decimal.Parse(sdr["WeightCheck"].ToString()) : customerInvoiceDetail.WeightCheck);
                                        customerInvoiceDetail.IsInvoiceInKG = (sdr["IsInvoiceInKG"].ToString() != "" ? bool.Parse(sdr["IsInvoiceInKG"].ToString()) : customerInvoiceDetail.IsInvoiceInKG);
                                        customerInvoiceDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : customerInvoiceDetail.Rate);
                                        customerInvoiceDetail.TradeDiscountAmount = (sdr["TradeDiscountAmount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountAmount"].ToString()) : customerInvoiceDetail.TradeDiscountAmount);
                                        customerInvoiceDetail.TradeDiscountPerc = (sdr["TradeDiscountPerc"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountPerc"].ToString()) : customerInvoiceDetail.TradeDiscountPerc);
                                        customerInvoiceDetail.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : customerInvoiceDetail.TaxTypeCode);
                                        customerInvoiceDetail.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : customerInvoiceDetail.SGSTPerc);
                                        customerInvoiceDetail.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : customerInvoiceDetail.CGSTPerc);
                                        customerInvoiceDetail.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : customerInvoiceDetail.IGSTPerc);

                                    }
                                    customerInvoiceDetailList.Add(customerInvoiceDetail);
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
            return customerInvoiceDetailList;
        }
        public List<CustomerInvoiceDetail> GetCustomerInvoiceDetailLinkForEditGroup(string id, string groupID)
        {
            List<CustomerInvoiceDetail> customerInvoiceDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetCustomerInvoiceDetailLinkForEdit_Group]";
                        if (groupID != null)
                            cmd.Parameters.Add("@GroupID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(groupID);
                        if (id != null)
                            cmd.Parameters.Add("@InvoiceID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(id);
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerInvoiceDetailList = new List<CustomerInvoiceDetail>();
                                while (sdr.Read())
                                {
                                    CustomerInvoiceDetail customerInvoiceDetail = new CustomerInvoiceDetail();
                                    {
                                        customerInvoiceDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customerInvoiceDetail.ID);
                                        customerInvoiceDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : customerInvoiceDetail.ProductID);
                                        customerInvoiceDetail.ProductName = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : customerInvoiceDetail.ProductName);
                                        customerInvoiceDetail.SlipNo = (sdr["SlipNo"].ToString() != "" ? sdr["SlipNo"].ToString() : customerInvoiceDetail.SlipNo);
                                        customerInvoiceDetail.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : customerInvoiceDetail.Quantity);
                                        customerInvoiceDetail.Weight = (sdr["Weight"].ToString() != "" ? decimal.Parse(sdr["Weight"].ToString()) : customerInvoiceDetail.Weight);
                                        customerInvoiceDetail.PackingSlipDetailID = (sdr["PackingSlipDetailID"].ToString() != "" ? Guid.Parse(sdr["PackingSlipDetailID"].ToString()) : customerInvoiceDetail.PackingSlipDetailID);
                                        customerInvoiceDetail.CustomerInvoiceDetailLinkID = (sdr["LinkID"].ToString() != "" ? Guid.Parse(sdr["LinkID"].ToString()) : customerInvoiceDetail.CustomerInvoiceDetailLinkID);
                                        customerInvoiceDetail.QuantityCheck = (sdr["QuantityCheck"].ToString() != "" ? decimal.Parse(sdr["QuantityCheck"].ToString()) : customerInvoiceDetail.QuantityCheck);
                                        customerInvoiceDetail.WeightCheck = (sdr["WeightCheck"].ToString() != "" ? decimal.Parse(sdr["WeightCheck"].ToString()) : customerInvoiceDetail.WeightCheck);
                                        customerInvoiceDetail.IsInvoiceInKG = (sdr["IsInvoiceInKG"].ToString() != "" ? bool.Parse(sdr["IsInvoiceInKG"].ToString()) : customerInvoiceDetail.IsInvoiceInKG);
                                        customerInvoiceDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : customerInvoiceDetail.Rate);
                                        customerInvoiceDetail.TradeDiscountAmount = (sdr["TradeDiscountAmount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountAmount"].ToString()) : customerInvoiceDetail.TradeDiscountAmount);
                                        customerInvoiceDetail.TradeDiscountPerc = (sdr["TradeDiscountPerc"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountPerc"].ToString()) : customerInvoiceDetail.TradeDiscountPerc);
                                        customerInvoiceDetail.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : customerInvoiceDetail.TaxTypeCode);
                                        customerInvoiceDetail.GroupID = (sdr["GroupID"].ToString() != "" ? Guid.Parse(sdr["GroupID"].ToString()) : customerInvoiceDetail.GroupID);
                                        customerInvoiceDetail.GroupName = (sdr["GroupName"].ToString() != "" ? sdr["GroupName"].ToString() : customerInvoiceDetail.GroupName);
                                        customerInvoiceDetail.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : customerInvoiceDetail.SGSTPerc);
                                        customerInvoiceDetail.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : customerInvoiceDetail.CGSTPerc);
                                        customerInvoiceDetail.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : customerInvoiceDetail.IGSTPerc);

                                    }
                                    customerInvoiceDetailList.Add(customerInvoiceDetail);
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
            return customerInvoiceDetailList;
        }
        public object UpdateCustomerInvoiceDetail(CustomerInvoice customerInvoice)
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
                        cmd.CommandText = "[AMC].[UpdateCustomerInvoiceDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = customerInvoice.ID;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = customerInvoice.InvoiceDateFormatted;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.VarChar, -1).Value = customerInvoice.DetailXML;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = customerInvoice.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = customerInvoice.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.UpdateFailure);
                    case "1":
                        return new
                        { 
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.UpdateSuccess
                        };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConst.UpdateSuccess
            };

        }

        public object UpdateCustomerInvoiceDetailService(CustomerInvoice customerInvoice)
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
                        cmd.CommandText = "[AMC].[UpdateCustomerInvoiceDetailService]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = customerInvoice.ID;
                        cmd.Parameters.Add("@InvoiceDate", SqlDbType.DateTime).Value = customerInvoice.InvoiceDateFormatted;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.VarChar, -1).Value = customerInvoice.DetailXML;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = customerInvoice.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = customerInvoice.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.UpdateFailure);
                    case "1":
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.UpdateSuccess
                        };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConst.UpdateSuccess
            };

        }

        public object DeleteCustomerInvoice(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteCustomerInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                       // cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = userName;
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

        public object DeleteCustomerInvoiceDetail(Guid id,string isGroupItem,Guid invoiceID)
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
                        cmd.CommandText = "[AMC].[DeleteCustomerInvoiceDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.Parameters.Add("@InvoiceID", SqlDbType.UniqueIdentifier).Value = invoiceID;
                        if (isGroupItem == "1")
                        {
                            cmd.Parameters.Add("@isGroupItem", SqlDbType.Bit).Value = true;
                        }
                        else
                        {
                            cmd.Parameters.Add("@isGroupItem", SqlDbType.Bit).Value = false;
                        }
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


        #region GetRecentCustomerInvoice
        public List<CustomerInvoice> GetRecentCustomerInvoice()
        {
            List<CustomerInvoice> customerInvoiceList = new List<CustomerInvoice>();
            CustomerInvoice customerInvoice = null;
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
                        cmd.CommandText = "[AMC].[GetRecentCustomerInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    customerInvoice = new CustomerInvoice();
                                    customerInvoice.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);
                                    customerInvoice.InvoiceNo = (sdr["InvoiceNo"].ToString() != "" ? sdr["InvoiceNo"].ToString() : customerInvoice.InvoiceNo);
                                    customerInvoice.InvoiceDateFormatted = (sdr["InvoiceDate"].ToString() != "" ? DateTime.Parse(sdr["InvoiceDate"].ToString()).ToString(settings.DateFormat) : customerInvoice.InvoiceDateFormatted);

                                    customerInvoice.Customer = new Customer();
                                    customerInvoice.Customer.CompanyName = (sdr["Customer"].ToString() != "" ? sdr["Customer"].ToString() : customerInvoice.Customer.CompanyName);
                                    customerInvoiceList.Add(customerInvoice);
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
            return customerInvoiceList;
        }


        #endregion GetRecentCustomerInvoice
        public List<CustomerInvoiceDetail> GetGroupProductListForCustomerInvoiceDetail(string slipNo, Guid groupID)
        {
            List<CustomerInvoiceDetail> customerInvoiceDetailList = new List<CustomerInvoiceDetail>();
            CustomerInvoiceDetail customerInvoiceDetail = null;
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
                        cmd.CommandText = "[AMC].[GetGroupProductListForCustomerInvoiceDetail]";
                        cmd.Parameters.Add("@GroupID", SqlDbType.UniqueIdentifier).Value = groupID;
                        cmd.Parameters.Add("@SlipNo", SqlDbType.NVarChar, 50).Value = slipNo;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    customerInvoiceDetail = new CustomerInvoiceDetail(); 
                                    customerInvoiceDetail.ProductName = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : customerInvoiceDetail.ProductName);
                                    customerInvoiceDetail.SlipNo = (sdr["SlipNo"].ToString() != "" ? sdr["SlipNo"].ToString() : customerInvoiceDetail.SlipNo);
                                    customerInvoiceDetail.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : customerInvoiceDetail.Quantity);
                                    customerInvoiceDetail.Weight = (sdr["Weight"].ToString() != "" ? decimal.Parse(sdr["Weight"].ToString()) : customerInvoiceDetail.Weight);
                                    customerInvoiceDetail.PackingSlipDetailID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);
                                    customerInvoiceDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : Guid.Empty);
                                    // customerInvoiceDetail.TaxTypeDescription = (sdr["TaxTypeDesc"].ToString() != "" ? sdr["TaxTypeDesc"].ToString() : customerInvoiceDetail.TaxTypeDescription);
                                    //
                                    customerInvoiceDetailList.Add(customerInvoiceDetail);
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
            return customerInvoiceDetailList;
        }

        #region GetOutstandingCustomerInvoice
        public decimal GetOutstandingCustomerInvoice()
        {
            decimal outstandingCustomerInvoice = 0;
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
                        cmd.CommandText = "[AMC].[GetOutstandingCustomerInvoice]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    outstandingCustomerInvoice = (sdr["OutstandingInvoice"].ToString() != "" ? decimal.Parse(sdr["OutstandingInvoice"].ToString()) : outstandingCustomerInvoice);
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
            return outstandingCustomerInvoice;
        }
        #endregion GetOutstandingCustomerInvoice

        public List<CustomerInvoiceDetail> GetGroupCustomerInvoiceDetailLink(Guid id, Guid groupID)
        {
            List<CustomerInvoiceDetail> customerInvoiceDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetGroupCustomerInvoiceDetailLink]";
                        if (groupID != null)
                            cmd.Parameters.Add("@GroupID", SqlDbType.UniqueIdentifier).Value = groupID;
                        if (id != null)
                            cmd.Parameters.Add("@InvoiceID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerInvoiceDetailList = new List<CustomerInvoiceDetail>();
                                while (sdr.Read())
                                {
                                    CustomerInvoiceDetail customerInvoiceDetail = new CustomerInvoiceDetail();
                                    {
                                        customerInvoiceDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customerInvoiceDetail.ID);
                                        customerInvoiceDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : customerInvoiceDetail.ProductID);
                                        //customerInvoiceDetail.ProductName = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : customerInvoiceDetail.ProductName);
                                        customerInvoiceDetail.SlipNo = (sdr["SlipNo"].ToString() != "" ? sdr["SlipNo"].ToString() : customerInvoiceDetail.SlipNo);
                                        customerInvoiceDetail.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : customerInvoiceDetail.Quantity);
                                        customerInvoiceDetail.Weight = (sdr["Weight"].ToString() != "" ? decimal.Parse(sdr["Weight"].ToString()) : customerInvoiceDetail.Weight);
                                        customerInvoiceDetail.PackingSlipDetailID = (sdr["PackingSlipDetailID"].ToString() != "" ? Guid.Parse(sdr["PackingSlipDetailID"].ToString()) : customerInvoiceDetail.PackingSlipDetailID);
                                        customerInvoiceDetail.CustomerInvoiceDetailLinkID = (sdr["LinkID"].ToString() != "" ? Guid.Parse(sdr["LinkID"].ToString()) : customerInvoiceDetail.CustomerInvoiceDetailLinkID);
                                      }
                                    customerInvoiceDetailList.Add(customerInvoiceDetail);
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
            return customerInvoiceDetailList;
        }

        public CustomerInvoiceDetail GetCustomerServiceInvoiceEdit(string id)
        {
            CustomerInvoiceDetail customerInvoiceDetail = null;
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
                        cmd.CommandText = "[AMC].[GetCustomerServiceInvoiceEdit]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DetailID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(id); 
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerInvoiceDetail = new CustomerInvoiceDetail();
                                if (sdr.Read())
                                {
                                    customerInvoiceDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customerInvoiceDetail.ID);
                                    customerInvoiceDetail.ServiceItemID = (sdr["ServiceItemID"].ToString() != "" ? Guid.Parse(sdr["ServiceItemID"].ToString()) : customerInvoiceDetail.ServiceItemID);
                                    customerInvoiceDetail.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : customerInvoiceDetail.TaxTypeCode);
                                    customerInvoiceDetail.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : customerInvoiceDetail.Quantity);
                                    customerInvoiceDetail.Weight = (sdr["Weight"].ToString() != "" ? decimal.Parse(sdr["Weight"].ToString()) : customerInvoiceDetail.Weight);
                                    customerInvoiceDetail.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : customerInvoiceDetail.Rate);
                                    customerInvoiceDetail.TradeDiscountAmount = (sdr["TradeDiscountAmount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountAmount"].ToString()) : customerInvoiceDetail.TradeDiscountAmount);
                                    customerInvoiceDetail.TradeDiscountPerc = (sdr["TradeDiscountPerc"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountPerc"].ToString()) : customerInvoiceDetail.TradeDiscountPerc);
                                    customerInvoiceDetail.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? decimal.Parse(sdr["SGSTPerc"].ToString()) : customerInvoiceDetail.SGSTPerc);
                                    customerInvoiceDetail.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? decimal.Parse(sdr["CGSTPerc"].ToString()) : customerInvoiceDetail.CGSTPerc);
                                    customerInvoiceDetail.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? decimal.Parse(sdr["IGSTPerc"].ToString()) : customerInvoiceDetail.IGSTPerc);
                                    customerInvoiceDetail.Total = (sdr["Total"].ToString() != "" ? decimal.Parse(sdr["Total"].ToString()) : customerInvoiceDetail.Total);
                                    customerInvoiceDetail.TaxableAmount = (sdr["TaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TaxableAmount"].ToString()) : customerInvoiceDetail.TaxableAmount);
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
            return customerInvoiceDetail;
        }
    }
}
