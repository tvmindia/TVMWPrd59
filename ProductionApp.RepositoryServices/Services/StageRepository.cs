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

        #region InsertUpdateStage
        /// <summary>
        /// To Insert and update Stage
        /// </summary>
        /// <param name="stage"></param>
        /// <returns>object</returns>
        public object InsertUpdateStage(Stage stage)
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
                        cmd.CommandText = "[AMC].[InsertUpdateStage]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = stage.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = stage.ID;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar,250).Value = stage.Description;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = stage.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = stage.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = stage.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = stage.Common.UpdatedDate;
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
                        throw new Exception(stage.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        stage.ID = Guid.Parse(OutputID.Value.ToString());
                        return new
                        {
                            ID = Guid.Parse(OutputID.Value.ToString()),
                            Status = outputStatus.Value.ToString(),
                            Message = stage.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = stage.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateStage

        #region GetStage
        /// <summary>
        /// To Get Stage Details corresponding to ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Stage</returns>
        public Stage GetStage(Guid id)
        {
            Stage stage = null;

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
                        cmd.CommandText = "[AMC].[GetStage]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    stage = new Stage();
                                    stage.ID = sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : stage.ID;
                                    stage.Description = sdr["Description"].ToString() != "" ? (sdr["Description"].ToString()) : stage.Description;
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
            return stage;
        }
        #endregion GetStage

        #region DeleteStage
        /// <summary>
        /// To Delete Stage
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object DeleteStage(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteStage]";
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
        #endregion DeleteStage

        #region GetStageForSelectList
        public List<Stage> GetStageForSelectList()
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
                        cmd.CommandText = "[AMC].[GetStageForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                stageList = new List<Stage>();
                                while (sdr.Read())
                                {
                                    Stage stage = new Stage();
                                    stage.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : stage.ID);
                                    stage.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : stage.Description);
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
        #endregion GetStageForSelectList
    }
}
