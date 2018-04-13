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
        AppConst _appConst = new AppConst();
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
                                        //employee.IsActive = (sdr["GeneralNotes"].ToString() != "" ? bool.Parse(sdr["GeneralNotes"].ToString()) : employee.IsActive);
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

        #region CheckEmployeeCodeExist
        /// <summary>
        /// To Check whether Employee Code Existing or not
        /// </summary>
        /// <param name="employeeCode"></param>
        /// <returns>bool</returns>
        public bool CheckEmployeeCodeExist(Employee employee)
        {
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
                        cmd.CommandText = "[AMC].[CheckEmployeeCodeExist]";
                        cmd.Parameters.Add("@EmployeeCode", SqlDbType.VarChar).Value = employee.Code;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = employee.ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        Object res = cmd.ExecuteScalar();
                        return (res.ToString() == "Exists" ? true : false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion CheckEmployeeCodeExist

        #region GetAllEmployee
        public List<Employee> GetAllEmployee(EmployeeAdvanceSearch employeeAdvanceSearch)
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
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(employeeAdvanceSearch.SearchTerm) ? "" : employeeAdvanceSearch.SearchTerm.Trim();
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = employeeAdvanceSearch.DataTablePaging.Start;
                        if (employeeAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = employeeAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@Department", SqlDbType.VarChar, 15).Value = employeeAdvanceSearch.Department.Code;
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
                                        //employee.ImageURL = (sdr["ImageURL"].ToString() != "" ? sdr["ImageURL"].ToString() : employee.ImageURL);
                                        employee.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : employee.GeneralNotes);
                                        employee.Department = new Department();
                                        employee.Department.Name = (sdr["Department"].ToString() != "" ? sdr["Department"].ToString() : employee.Department.Name);
                                        employee.EmployeeCategory = new EmployeeCategory();
                                        employee.EmployeeCategory.Name = (sdr["Category"].ToString() != "" ? sdr["Category"].ToString() : employee.EmployeeCategory.Name);
                                        //employee.IsActive = (sdr["GeneralNotes"].ToString() != "" ? bool.Parse(sdr["GeneralNotes"].ToString()) : employee.IsActive);
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

        #region GetEmployee
        /// <summary>
        /// To Get Employee Details corresponding to ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Employee</returns>
        public Employee GetEmployee(Guid id)
        {
            Employee employee = null;

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
                        cmd.CommandText = "[AMC].[GetEmployee]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    employee = new Employee();
                                    employee.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : employee.ID);
                                    employee.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : employee.Code);
                                    employee.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : employee.Name);
                                    employee.MobileNo = (sdr["MobileNo"].ToString() != "" ? sdr["MobileNo"].ToString() : employee.MobileNo);
                                    employee.Address = (sdr["Address"].ToString() != "" ? sdr["Address"].ToString() : employee.Address);
                                    //employee.ImageURL = (sdr["ImageURL"].ToString() != "" ? sdr["ImageURL"].ToString() : employee.ImageURL);
                                    employee.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : employee.GeneralNotes);
                                    employee.DepartmentCode = (sdr["DepartmentCode"].ToString() != "" ? sdr["DepartmentCode"].ToString() : employee.DepartmentCode);
                                    employee.EmployeeCategoryCode= (sdr["EmployeeCategoryCode"].ToString() != "" ? sdr["EmployeeCategoryCode"].ToString() : employee.EmployeeCategoryCode);
                                    employee.IsActive = sdr["IsActive"].ToString() != "" ? bool.Parse(sdr["IsActive"].ToString()) : employee.IsActive;
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
            return employee;
        }
        #endregion GetEmployee

        #region InsertUpdateEmployee
        /// <summary>
        /// To Insert and update Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>object</returns>
        public object InsertUpdateEmployee(Employee employee)
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
                        cmd.CommandText = "[AMC].[InsertUpdateEmployee]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = employee.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = employee.ID;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = employee.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = employee.Name;
                        cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = employee.MobileNo;
                        cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = employee.Address;
                        //cmd.Parameters.Add("@ImageURL", SqlDbType.VarChar).Value = employee.ImageURL;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar).Value = employee.GeneralNotes;
                        cmd.Parameters.Add("@DepartmentCode", SqlDbType.NVarChar).Value = employee.DepartmentCode;
                        cmd.Parameters.Add("@EmployeeCategoryCode", SqlDbType.NVarChar).Value = employee.EmployeeCategoryCode;
                        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = employee.IsActive;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = employee.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = employee.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = employee.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = employee.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        OutputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        OutputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(employee.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        employee.ID = Guid.Parse(OutputID.Value.ToString());
                        return new
                        {
                            ID = Guid.Parse(OutputID.Value.ToString()),
                            Status = outputStatus.Value.ToString(),
                            Message = employee.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                ID = Guid.Parse(OutputID.Value.ToString()),
                Status = outputStatus.Value.ToString(),
                Message = employee.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateProduct

        #region DeleteEmployee
        /// <summary>
        /// To Delete Employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object DeleteEmployee(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteEmployee]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.Int);
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
        #endregion DeleteEmployee

    }
}
