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
    public class CustomerRepository: ICustomerRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        public CustomerRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetCustomer
        /// <summary>
        /// To Get Customer By ID
        /// </summary>
        /// <param name="id></param>
        /// <returns>Customer</returns>
        public Customer GetCustomer(Guid id)
        {
            Customer customer = new Customer();
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
                        cmd.CommandText = "[AMC].[GetCustomer]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {

                                while (sdr.Read())
                                {
                                    customer.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customer.ID);
                                    customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : customer.CompanyName);
                                    customer.ContactPerson = (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : customer.ContactPerson);
                                    customer.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : customer.ContactEmail);
                                    customer.ContactTitle= (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : customer.ContactTitle);
                                    customer.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : customer.Website);
                                    customer.LandLine = (sdr["LandLine"].ToString() != "" ? sdr["LandLine"].ToString() : customer.LandLine);
                                    customer.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : customer.Mobile);
                                    customer.Fax = (sdr["Fax"].ToString() != "" ? sdr["Fax"].ToString() : customer.Fax);
                                    customer.OtherPhoneNumbers = (sdr["OtherPhoneNumbers"].ToString() != "" ? sdr["OtherPhoneNumbers"].ToString() : customer.OtherPhoneNumbers);
                                    customer.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : customer.BillingAddress);
                                    customer.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : customer.ShippingAddress);
                                    customer.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : customer.PaymentTermCode);
                                    customer.TaxRegNo = (sdr["TaxRegNo"].ToString() != "" ? sdr["TaxRegNo"].ToString() : customer.TaxRegNo);
                                    customer.PANNo = (sdr["PANNo"].ToString() != "" ? sdr["PANNo"].ToString() : customer.PANNo);
                                    customer.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : customer.GeneralNotes);
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
            return customer;
        }
        #endregion GetCustomer

        #region GetCustomerForSelectList
        public List<Customer> GetCustomerForSelectList()
        {
            List<Customer> customerList = null;
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
                        cmd.CommandText = "[AMC].[GetCustomerForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerList = new List<Customer>();
                                while (sdr.Read())
                                {
                                    Customer customer = new Customer();
                                    {
                                        customer.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customer.ID);
                                        customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : customer.CompanyName);
                                    }
                                    customerList.Add(customer);
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
            return customerList;
        }
        #endregion GetCustomerForSelectList

        #region GetContactTitleForSelectList
        /// <summary>
        /// To Get ContactTitle For SelectList
        /// </summary>
        /// <returns>List</returns>
        public List<ContactTitle> GetContactTitleForSelectList()
        {
            List<ContactTitle> contactTitleList = null;
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
                        cmd.CommandText = "[AMC].[GetAllTitle]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                contactTitleList = new List<ContactTitle>();
                                while (sdr.Read())
                                {
                                    ContactTitle contactTitle = new ContactTitle();
                                    {
                                        contactTitle.Title = (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : contactTitle.Title);
                                    }
                                    contactTitleList.Add(contactTitle);
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
            return contactTitleList;
        }
        #endregion GetContactTitleForSelectList

        #region GetAllCustomer
        /// <summary>
        /// To Get List of All Customer
        /// </summary>
        /// <param name="customerAdvanceSearch"></param>
        /// <returns>List</returns>
        public List<Customer> GetAllCustomer(CustomerAdvanceSearch customerAdvanceSearch)
        {
            List<Customer> customerList = null;
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
                        cmd.CommandText = "[AMC].[GetAllCustomer]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(customerAdvanceSearch.SearchTerm) ? "" : customerAdvanceSearch.SearchTerm.Trim();
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = customerAdvanceSearch.DataTablePaging.Start;
                        if (customerAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = customerAdvanceSearch.DataTablePaging.Length;
                        //cmd.Parameters.Add("@OrderDir", SqlDbType.NVarChar, 5).Value = model.order[0].dir;
                        //cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar, -1).Value = model.order[0].column;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerList = new List<Customer>();
                                while (sdr.Read())
                                {
                                    Customer customer = new Customer();
                                    {
                                        customer.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customer.ID);
                                        customer.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : customer.CompanyName);
                                        customer.ContactPerson= (sdr["ContactPerson"].ToString() != "" ? sdr["ContactPerson"].ToString() : customer.ContactPerson);
                                        customer.ContactEmail = (sdr["ContactEmail"].ToString() != "" ? sdr["ContactEmail"].ToString() : customer.ContactEmail);
                                        customer.ContactTitle = (sdr["ContactTitle"].ToString() != "" ? sdr["ContactTitle"].ToString() : customer.ContactTitle);
                                        customer.Website = (sdr["Website"].ToString() != "" ? sdr["Website"].ToString() : customer.Website);
                                        customer.LandLine = (sdr["LandLine"].ToString() != "" ? sdr["LandLine"].ToString() : customer.LandLine);
                                        customer.Mobile = (sdr["Mobile"].ToString() != "" ? sdr["Mobile"].ToString() : customer.Mobile);
                                        customer.Fax = (sdr["Fax"].ToString() != "" ? sdr["Fax"].ToString() : customer.Fax);
                                        customer.OtherPhoneNumbers = (sdr["OtherPhoneNumbers"].ToString() != "" ? sdr["OtherPhoneNumbers"].ToString() : customer.OtherPhoneNumbers);
                                        customer.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : customer.BillingAddress);
                                        customer.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : customer.ShippingAddress);
                                        customer.PaymentTermCode = (sdr["PaymentTermCode"].ToString() != "" ? sdr["PaymentTermCode"].ToString() : customer.PaymentTermCode);
                                        customer.TaxRegNo = (sdr["TaxRegNo"].ToString() != "" ? sdr["TaxRegNo"].ToString() : customer.TaxRegNo);
                                        customer.PANNo = (sdr["PANNo"].ToString() != "" ? sdr["PANNo"].ToString() : customer.PANNo);
                                        customer.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : customer.GeneralNotes);
                                        customer.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : customer.FilteredCount);
                                        customer.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : customer.TotalCount);
                                        customer.OutStanding = (sdr["OutStanding"].ToString() != "" ? decimal.Parse(sdr["OutStanding"].ToString()) : customer.OutStanding);
                                    }
                                    customerList.Add(customer);
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
            return customerList;
        }
        #endregion GetAllCustomer

        #region InsertUpdateCustomer
        /// <summary>
        /// To Insert and update Customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>object</returns>
        public object InsertUpdateCustomer(Customer customer)
        {
            SqlParameter outputStatus, OutputID;
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
                        cmd.CommandText = "[AMC].[InsertUpdateCustomer]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = customer.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = customer.ID;
                        cmd.Parameters.Add("@CompanyName", SqlDbType.VarChar, 150).Value = customer.CompanyName;
                        cmd.Parameters.Add("@ContactPerson", SqlDbType.VarChar, 100).Value = customer.ContactPerson;
                        cmd.Parameters.Add("@ContactEmail", SqlDbType.VarChar, 150).Value = customer.ContactEmail;
                        cmd.Parameters.Add("@ContactTitle", SqlDbType.VarChar, 10).Value = customer.ContactTitle;
                        cmd.Parameters.Add("@Website", SqlDbType.NVarChar, 500).Value = customer.Website;
                        cmd.Parameters.Add("@LandLine", SqlDbType.VarChar, 50).Value = customer.LandLine;
                        cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 50).Value = customer.Mobile;
                        cmd.Parameters.Add("@Fax", SqlDbType.VarChar, 50).Value = customer.Fax;
                        cmd.Parameters.Add("@OtherPhoneNos", SqlDbType.VarChar, 250).Value = customer.OtherPhoneNumbers;
                        cmd.Parameters.Add("@BillingAddress", SqlDbType.NVarChar, -1).Value = customer.BillingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.NVarChar, -1).Value = customer.ShippingAddress;
                        cmd.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar, 10).Value = customer.PaymentTermCode;
                        cmd.Parameters.Add("@TaxRegNo", SqlDbType.VarChar, 50).Value = customer.TaxRegNo;
                        cmd.Parameters.Add("@PANNo", SqlDbType.VarChar, 50).Value = customer.PANNo;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar, -1).Value = customer.GeneralNotes;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 50).Value = customer.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = customer.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outputStatus.Direction = ParameterDirection.Output;
                        OutputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        OutputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(customer.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        customer.ID = Guid.Parse(OutputID.Value.ToString());
                        return new
                        {
                            ID = customer.ID,
                            Status = outputStatus.Value.ToString(),
                            Message = customer.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                ID = customer.ID,
                Status = outputStatus.Value.ToString(),
                Message = customer.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateCustomer

        #region DeleteCustomer
        /// <summary>
        /// To Delete Customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>object</returns>
        public object DeleteCustomer(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteCustomer]";
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
                Message =  _appConst.DeleteSuccess
            };
        }
        #endregion DeleteCustomer

    }
}
