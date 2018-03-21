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
        public CustomerRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public Customer GetCustomer(Guid customerId)
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
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = customerId;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {

                                while (sdr.Read())
                                {
                                    customer.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : customer.BillingAddress);
                                    customer.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : customer.ShippingAddress);
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
                                       // customer.MobileNo = (sdr["MobileNo"].ToString() != "" ? sdr["MobileNo"].ToString() : customer.MobileNo);
                                        //customer.EmpType = (sdr["EmpType"].ToString() != "" ? sdr["EmpType"].ToString() : customer.EmpType);
                                        //customer.Address = (sdr["Address"].ToString() != "" ? sdr["Address"].ToString() : customer.Address);
                                        //customer.ImageURL = (sdr["ImageURL"].ToString() != "" ? sdr["ImageURL"].ToString() : customer.ImageURL);
                                        //customer.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : customer.GeneralNotes);
                                        //customer.Department = (sdr["Department"].ToString() != "" ? sdr["Department"].ToString() : customer.Department);
                                        //customer.EmployeeCategory = (sdr["EmployeeCategory"].ToString() != "" ? sdr["EmployeeCategory"].ToString() : customer.EmployeeCategory);
                                        //customer.IsActive = (sdr["GeneralNotes"].ToString() != "" ? bool.Parse(sdr["GeneralNotes"].ToString()) : customer.IsActive);
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
      

    }
}
