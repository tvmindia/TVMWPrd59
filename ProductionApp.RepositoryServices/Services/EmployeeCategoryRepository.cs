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
    public class EmployeeCategoryRepository : IEmployeeCategoryRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public EmployeeCategoryRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection
        #region GetEmployeeCategoryForSelectList
        public List<EmployeeCategory> GetEmployeeCategoryForSelectList()
        {
            List<EmployeeCategory> categorytList = null;
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
                        cmd.CommandText = "[AMC].[GetEmployeeCategoryForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                categorytList = new List<EmployeeCategory>();
                                while (sdr.Read())
                                {
                                    EmployeeCategory employeeCategory = new EmployeeCategory();
                                    {
                                        employeeCategory.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : employeeCategory.Code);
                                        employeeCategory.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : employeeCategory.Name);
                                    }
                                    categorytList.Add(employeeCategory);
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
            return categorytList;
        }
        #endregion
    }
}
