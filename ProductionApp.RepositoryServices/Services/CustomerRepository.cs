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
    }
}
