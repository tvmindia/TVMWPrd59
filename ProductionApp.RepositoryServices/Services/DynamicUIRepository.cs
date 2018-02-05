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



        public List<Menu> GetAllMenues()
        {
            List<Menu> menuList = null;
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
                        cmd.CommandText = "[Accounts].[GetAllMenuItems]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                menuList = new List<Menu>();
                                while (sdr.Read())
                                {
                                    Menu menuObj = new Menu();
                                    {
                                        menuObj.ID = (sdr["ID"].ToString() != "" ? Int16.Parse(sdr["ID"].ToString()) : menuObj.ID);
                                        menuObj.ParentID = (sdr["ParentID"].ToString() != "" ? Int16.Parse(sdr["ParentID"].ToString()) : menuObj.ParentID);
                                        menuObj.MenuText = sdr["MenuText"].ToString();
                                        menuObj.Controller = sdr["Controller"].ToString();
                                        menuObj.Action = sdr["Action"].ToString();
                                        menuObj.Parameter = sdr["Parameters"].ToString();
                                        menuObj.IconClass = sdr["IconClass"].ToString();
                                        menuObj.IconURL = sdr["IconURL"].ToString();

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
