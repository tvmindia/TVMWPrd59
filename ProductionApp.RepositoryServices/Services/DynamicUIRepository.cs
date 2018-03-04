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
            List<AMCSysModule> sysModuleList = null;
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
                                sysModuleList = new List<AMCSysModule>();
                                while (sdr.Read())
                                {
                                    AMCSysModule sysModule = new AMCSysModule();
                                    {
                                        sysModule.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : sysModule.Code);
                                        sysModule.Name = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : sysModule.Name);
                                        sysModule.Action= sdr["Action"].ToString();
                                        sysModule.Controller = sdr["Controller"].ToString();
                                        sysModule.IconClass = sdr["IconClass"].ToString();
                                    }
                                    sysModuleList.Add(sysModule);
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
            return sysModuleList;
        }

        public List<AMCSysMenu> GetAllMenu(string code)
        {
            List<AMCSysMenu> sysMenuList = null;
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
                                sysMenuList = new List<AMCSysMenu>();
                                while (sdr.Read())
                                {
                                    AMCSysMenu sysMenu = new AMCSysMenu();
                                    {
                                        sysMenu.ID = (sdr["ID"].ToString() != "" ? Int16.Parse(sdr["ID"].ToString()) : sysMenu.ID);
                                        sysMenu.Module= sdr["Module"].ToString();
                                        sysMenu.AMCSysModuleObj = new AMCSysModule
                                        {
                                            Code = sdr["Module"].ToString(),
                                            Name = sdr["Name"].ToString()
                                        };
                                        sysMenu.ParentID = (sdr["ParentID"].ToString() != "" ? Int16.Parse(sdr["ParentID"].ToString()) : sysMenu.ParentID);
                                        sysMenu.MenuText = sdr["MenuText"].ToString();
                                        sysMenu.Controller = sdr["Controller"].ToString();
                                        sysMenu.Action = sdr["Action"].ToString();
                                        sysMenu.IconClass = sdr["IconClass"].ToString();
                                        sysMenu.IconURL = sdr["IconURL"].ToString();
                                        sysMenu.Parameters= sdr["Parameters"].ToString();
                                        sysMenu.MenuOrder= (sdr["MenuOrder"].ToString() != "" ? decimal.Parse(sdr["MenuOrder"].ToString()) : sysMenu.MenuOrder);
                                    }
                                    sysMenuList.Add(sysMenu);
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

            return sysMenuList;
        }


        public List<IncomeExpenseSummary> GetIncomeExpenseSummary()
        {
            List<IncomeExpenseSummary> result = new List<IncomeExpenseSummary>();
           
           
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
                        cmd.CommandText = "[AMC].[GetIncomeExpenseSummary]";
                       
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    IncomeExpenseSummary incomeExpense = new IncomeExpenseSummary();

                                    incomeExpense.Month = (sdr["Month"].ToString() != "" ? sdr["Month"].ToString() : incomeExpense.Month);
                                    incomeExpense.MonthCode = (sdr["MonthCode"].ToString() != "" ? int.Parse(sdr["MonthCode"].ToString()) : incomeExpense.MonthCode);
                                    incomeExpense.Year = (sdr["Year"].ToString() != "" ? int.Parse(sdr["Year"].ToString()) : incomeExpense.Year);
                                    incomeExpense.InAmount = (sdr["InAmount"].ToString() != "" ? decimal.Parse(sdr["InAmount"].ToString()) : incomeExpense.InAmount);
                                    incomeExpense.ExAmount = (sdr["ExAmount"].ToString() != "" ? decimal.Parse(sdr["ExAmount"].ToString()) : incomeExpense.ExAmount);

                                    result.Add(incomeExpense);


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

            return result;
        }
    }
}
