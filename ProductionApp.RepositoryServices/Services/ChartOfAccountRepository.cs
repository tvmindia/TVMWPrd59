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
    public class ChartOfAccountRepository : IChartOfAccountRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ChartOfAccountRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection 

        #region GetChartOfAccountForSelectList
        /// <summary>
        /// To Get ChartOfAccount List For DropDown
        /// </summary>
        /// <returns>List</returns>
        public List<ChartOfAccount> GetChartOfAccountForSelectList()
        {
            List<ChartOfAccount> chartOfAccountList = null;
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
                        cmd.CommandText = "[AMC].[GetChartOfAccountForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                chartOfAccountList = new List<ChartOfAccount>();
                                while (sdr.Read())
                                {
                                    ChartOfAccount chartOfAccount = new ChartOfAccount();
                                    {
                                        chartOfAccount.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : chartOfAccount.Code);
                                        chartOfAccount.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : chartOfAccount.Type);
                                        chartOfAccount.TypeDesc = (sdr["TypeDesc"].ToString() != "" ? sdr["TypeDesc"].ToString() : chartOfAccount.TypeDesc);
                                        chartOfAccount.IsSubHeadApplicable = (sdr["IsSubHeadApplicable"].ToString() != "" ? bool.Parse(sdr["IsSubHeadApplicable"].ToString()) : chartOfAccount.IsSubHeadApplicable);
                                    }
                                    chartOfAccountList.Add(chartOfAccount);
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
            return chartOfAccountList;
        }
        #endregion GetChartOfAccountForSelectList

        #region GetAllChartOfAccount
        /// <summary>
        /// To Get List of All ChartOfAccount
        /// </summary>
        /// <param name="chartOfAccountAdvanceSearch"></param>
        /// <returns>List</returns>
        public List<ChartOfAccount> GetAllChartOfAccount(ChartOfAccountAdvanceSearch chartOfAccountAdvanceSearch)
        {
            List<ChartOfAccount> chartOfAccountList = null;
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
                        cmd.CommandText = "[AMC].[GetAllChartOfAccount]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(chartOfAccountAdvanceSearch.SearchTerm) ? "" : chartOfAccountAdvanceSearch.SearchTerm.Trim();
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = chartOfAccountAdvanceSearch.DataTablePaging.Start;
                        if (chartOfAccountAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = chartOfAccountAdvanceSearch.DataTablePaging.Length;
                        //cmd.Parameters.Add("@OrderDir", SqlDbType.NVarChar, 5).Value = model.order[0].dir;
                        //cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar, -1).Value = model.order[0].column;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                chartOfAccountList = new List<ChartOfAccount>();
                                while (sdr.Read())
                                {
                                    ChartOfAccount chartOfAccount = new ChartOfAccount();
                                    {
                                        chartOfAccount.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : chartOfAccount.Code);
                                        chartOfAccount.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : chartOfAccount.Type);
                                        chartOfAccount.TypeDesc = (sdr["TypeDesc"].ToString() != "" ? sdr["TypeDesc"].ToString() : chartOfAccount.TypeDesc);
                                        chartOfAccount.IsSubHeadApplicable = (sdr["IsSubHeadApplicable"].ToString() != "" ? bool.Parse(sdr["IsSubHeadApplicable"].ToString()) : chartOfAccount.IsSubHeadApplicable);
                                        chartOfAccount.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : chartOfAccount.FilteredCount);
                                        chartOfAccount.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : chartOfAccount.TotalCount);
                                    }
                                    chartOfAccountList.Add(chartOfAccount);
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
            return chartOfAccountList;
        }
        #endregion GetAllProduct

        #region InsertUpdateChartOfAccount
        /// <summary>
        /// To Insert and update ChartOfAccount
        /// </summary>
        /// <param name="chartOfAccount"></param>
        /// <returns>object</returns>
        public object InsertUpdateChartOfAccount(ChartOfAccount chartOfAccount)
        {
            SqlParameter outputStatus, outputCode;
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
                        cmd.CommandText = "[AMC].[InsertUpdateChartOfAccount]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = chartOfAccount.IsUpdate;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = chartOfAccount.Code;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = chartOfAccount.Type;
                        cmd.Parameters.Add("@TypeDesc", SqlDbType.VarChar).Value = chartOfAccount.TypeDesc;
                        cmd.Parameters.Add("@IsSubHeadApplicable", SqlDbType.Bit).Value = chartOfAccount.IsSubHeadApplicable;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = chartOfAccount.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = chartOfAccount.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = chartOfAccount.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = chartOfAccount.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 10);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(chartOfAccount.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        chartOfAccount.Code = outputCode.Value.ToString();
                        return new
                        {
                            Code = chartOfAccount.Code,
                            Status = outputStatus.Value.ToString(),
                            Message = chartOfAccount.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Code = chartOfAccount.Code,
                Status = outputStatus.Value.ToString(),
                Message = chartOfAccount.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateChartOfAccount

        //#region CheckChartOfAccountTypeExist
        ///// <summary>
        ///// To Check whether ChartOfAccount Type Existing or not
        ///// </summary>
        ///// <param name="chartOfAccount"></param>
        ///// <returns>bool</returns>
        //public bool CheckChartOfAccountTypeExist(ChartOfAccount chartOfAccount)
        //{
        //    try
        //    {
        //        using (SqlConnection con = _databaseFactory.GetDBConnection())
        //        {
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                if (con.State == ConnectionState.Closed)
        //                {
        //                    con.Open();
        //                }
        //                cmd.Connection = con;
        //                cmd.CommandText = "[AMC].[CheckChartOfAccountTypeExist]";
        //                cmd.Parameters.Add("@Type", SqlDbType.VarChar,50).Value = chartOfAccount.Type;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                Object res = cmd.ExecuteScalar();
        //                return (res.ToString() == "Exists" ? true : false);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion CheckChartOfAccountTypeExist

        #region GetChartOfAccount
        /// <summary>
        /// To Get ChartOfAccount Details corresponding to Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Product</returns>
        public ChartOfAccount GetChartOfAccount(string code)
        {
            ChartOfAccount chartOfAccount = null;

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
                        cmd.CommandText = "[AMC].[GetChartOfAccount]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@code", SqlDbType.VarChar,10).Value = code;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    chartOfAccount = new ChartOfAccount();
                                    chartOfAccount.Code = sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : chartOfAccount.Code;
                                    chartOfAccount.Type = sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : chartOfAccount.Type;
                                    chartOfAccount.TypeDesc = sdr["TypeDesc"].ToString() != "" ? sdr["TypeDesc"].ToString() : chartOfAccount.TypeDesc;
                                    chartOfAccount.IsSubHeadApplicable = sdr["IsSubHeadApplicable"].ToString() != "" ? bool.Parse(sdr["IsSubHeadApplicable"].ToString()) : chartOfAccount.IsSubHeadApplicable;
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
            return chartOfAccount;
        }
        #endregion GetChartOfAccount

        #region DeleteChartOfAccount
        /// <summary>
        /// To Delete ChartOfAccount
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public object DeleteChartOfAccount(string code)
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
                        cmd.CommandText = "[AMC].[DeleteChartOfAccount]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@code", SqlDbType.VarChar).Value = code;
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
        #endregion DeleteChartOfAccount
    }
}
