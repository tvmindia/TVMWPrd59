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
    public class StageRepository: IStageRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public StageRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetAllStage
        /// <summary>
        /// To Get List of All Stage
        /// </summary>
        /// <param name="stageAdvanceSearch"></param>
        /// <returns>List</returns>
        public List<Stage> GetAllStage(StageAdvanceSearch stageAdvanceSearch)
        {
            List<Stage> stageList = null;
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
                        cmd.CommandText = "[AMC].[GetAllStage]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(stageAdvanceSearch.SearchTerm) ? "" : stageAdvanceSearch.SearchTerm.Trim();
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = stageAdvanceSearch.DataTablePaging.Start;
                        if (stageAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = stageAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                stageList = new List<Stage>();
                                while (sdr.Read())
                                {
                                    Stage stage = new Stage();
                                    {
                                        stage.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : stage.ID);
                                        stage.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : stage.Description);
                                        stage.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : stage.FilteredCount);
                                        stage.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : stage.TotalCount);
                                    }
                                    stageList.Add(stage);
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
            return stageList;
        }
        #endregion GetAllStage
    }
}
