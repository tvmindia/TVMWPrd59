using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;

namespace ProductionApp.RepositoryServices.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IDatabaseFactory _databaseFactory;
        public EmployeeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<Employee> GetEmployeeForSelectList()
        {
            List<Employee> employeeList = null;
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
                        cmd.CommandText = "[AMC].[GetEmployeeForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                employeeList = new List<Employee>();
                                while (sdr.Read())
                                {
                                    Employee employee = new Employee();
                                    {
                                        employee.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : employee.ID);
                                        employee.Code = (sdr["EmpCode"].ToString() != "" ? sdr["EmpCode"].ToString() : employee.Code);
                                        employee.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : employee.Name);
                                        employee.MobileNo = (sdr["MobileNo"].ToString() != "" ? sdr["MobileNo"].ToString() : employee.MobileNo);
                                        employee.EmpType = (sdr["EmpType"].ToString() != "" ? sdr["EmpType"].ToString() : employee.EmpType);
                                        employee.Address = (sdr["Address"].ToString() != "" ? sdr["Address"].ToString() : employee.Address);
                                        employee.ImageURL = (sdr["ImageURL"].ToString() != "" ? sdr["ImageURL"].ToString() : employee.ImageURL);
                                        employee.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : employee.GeneralNotes);
                                        employee.DepartmentCode = (sdr["DepartmentCode"].ToString() != "" ? sdr["DepartmentCode"].ToString() : employee.DepartmentCode);
                                        employee.EmployeeCategoryCode = (sdr["EmployeeCategoryCode"].ToString() != "" ? sdr["EmployeeCategoryCode"].ToString() : employee.EmployeeCategoryCode);
                                        employee.IsActive = (sdr["GeneralNotes"].ToString() != "" ? bool.Parse(sdr["GeneralNotes"].ToString()) : employee.IsActive);
                                    }
                                    employeeList.Add(employee);
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
            return employeeList;
        }

        #region GetAllEmployee
        public List<Employee> GetAllEmployee()
        {
            List<Employee> employeeList = null;
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
                        cmd.CommandText = "[AMC].[GetAllEmployee]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                employeeList = new List<Employee>();
                                while (sdr.Read())
                                {
                                    Employee employee = new Employee();
                                    {
                                        employee.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : employee.ID);
                                        employee.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : employee.Code);
                                        employee.Name = (sdr["Employee"].ToString() != "" ? sdr["Employee"].ToString() : employee.Name);
                                        employee.MobileNo = (sdr["MobileNo"].ToString() != "" ? sdr["MobileNo"].ToString() : employee.MobileNo);
                                        employee.EmpType = (sdr["EmpType"].ToString() != "" ? sdr["EmpType"].ToString() : employee.EmpType);
                                        employee.Address = (sdr["Address"].ToString() != "" ? sdr["Address"].ToString() : employee.Address);
                                        employee.ImageURL = (sdr["ImageURL"].ToString() != "" ? sdr["ImageURL"].ToString() : employee.ImageURL);
                                        employee.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : employee.GeneralNotes);
                                        employee.DepartmentCode = (sdr["Department"].ToString() != "" ? sdr["Department"].ToString() : employee.DepartmentCode);
                                        employee.EmployeeCategoryCode = (sdr["Category"].ToString() != "" ? sdr["Category"].ToString() : employee.EmployeeCategoryCode);
                                        employee.IsActive = (sdr["GeneralNotes"].ToString() != "" ? bool.Parse(sdr["GeneralNotes"].ToString()) : employee.IsActive);
                                    }
                                    employeeList.Add(employee);
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
            return employeeList;
        }
        #endregion GetAllEmployee

    }
}
