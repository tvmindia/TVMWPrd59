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
    public class DynamicUIRepository:IDynamicUIRepository
    {
        private IDatabaseFactory _databaseFactory;
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public DynamicUIRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<AMCSysModule> GetAllModule()
        {
            List<AMCSysModule> moduleList = null;
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
                                moduleList = new List<AMCSysModule>();
                                while (sdr.Read())
                                {
                                    AMCSysModule moduleObj = new AMCSysModule();
                                    {
                                        moduleObj.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : moduleObj.Code);
                                        moduleObj.Name = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : moduleObj.Name);
                                        moduleObj.Controller = sdr["Controller"].ToString();
                                        moduleObj.IconClass = sdr["IconClass"].ToString();
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

        public List<AMCSysMenu> GetAllMenu(string code)
        {
            List<AMCSysMenu> menuList = null;
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
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 5).Value = code;
                        cmd.CommandText = "[AMC].[GetAllMenu]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                menuList = new List<AMCSysMenu>();
                                while (sdr.Read())
                                {
                                    AMCSysMenu menuObj = new AMCSysMenu();
                                    {
                                        menuObj.ID = (sdr["ID"].ToString() != "" ? Int16.Parse(sdr["ID"].ToString()) : menuObj.ID);
                                        menuObj.Module= sdr["Module"].ToString();
                                        menuObj.AMCSysModuleObj = new AMCSysModule
                                        {
                                            Code = sdr["Module"].ToString(),
                                            Name = sdr["Name"].ToString()
                                        };
                                        menuObj.ParentID = (sdr["ParentID"].ToString() != "" ? Int16.Parse(sdr["ParentID"].ToString()) : menuObj.ParentID);
                                        menuObj.MenuText = sdr["MenuText"].ToString();
                                        menuObj.Controller = sdr["Controller"].ToString();
                                        menuObj.Action = sdr["Action"].ToString();
                                        menuObj.IconClass = sdr["IconClass"].ToString();
                                        menuObj.IconURL = sdr["IconURL"].ToString();
                                        menuObj.Parameters= sdr["Parameters"].ToString();
                                        menuObj.MenuOrder= (sdr["MenuOrder"].ToString() != "" ? decimal.Parse(sdr["MenuOrder"].ToString()) : menuObj.MenuOrder);
                                    }
                                    menuList.Add(menuObj);
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

            return menuList;
        }
    }
}
