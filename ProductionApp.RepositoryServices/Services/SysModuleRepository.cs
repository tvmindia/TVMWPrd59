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
    public class SysModuleRepository: ISysModuleRepository
    {
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SysModuleRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<SysModule> GetAllModule()
        {
            List<SysModule> moduleList = null;
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
                        cmd.CommandText = "[AMC].[GetAllModule]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                moduleList = new List<SysModule>();
                                while (sdr.Read())
                                {
                                    SysModule moduleObj = new SysModule();
                                    {
                                        moduleObj.Code = (sdr["Code"].ToString() != "" ?(sdr["Code"].ToString()) : moduleObj.Code);
                                        moduleObj.Name = (sdr["Name"].ToString() != "" ?(sdr["Name"].ToString()) : moduleObj.Name);

                                    }
                                    moduleList.Add(moduleObj);
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
            return moduleList;
        }
    }
}
