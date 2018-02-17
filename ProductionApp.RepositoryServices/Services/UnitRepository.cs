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
    public class UnitRepository:IUnitRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public UnitRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetUnitForSelectList
        /// <summary>
        /// To Get List of All unit for Select List
        /// </summary>
        /// <returns>List</returns>
        public List<Unit> GetUnitForSelectList()
        {
            List<Unit> unitList = null;
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
                        cmd.CommandText = "[AMC].[GetUnitForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                unitList = new List<Unit>();
                                while (sdr.Read())
                                {
                                    Unit unit = new Unit();
                                    {
                                        unit.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : unit.Code);
                                        unit.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : unit.Description);
                                    }
                                    unitList.Add(unit);
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
            return unitList;
        }
        #endregion GetUnitForSelectList

    }
}
