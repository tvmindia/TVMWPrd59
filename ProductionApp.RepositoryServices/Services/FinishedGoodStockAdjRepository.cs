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
    public class FinishedGoodStockAdjRepository:IFinishedGoodStockAdjRepository
    {
        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();
        public FinishedGoodStockAdjRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllFinishedGoodStockAdj
        public List<FinishedGoodStockAdj> GetAllFinishedGoodStockAdj(FinishedGoodStockAdjAdvanceSearch finishedGoodStockAdjAdvanceSearch)
        {
            List<FinishedGoodStockAdj> finishedGoodStockAdjList = null;
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
                        cmd.CommandText = "[AMC].[GetAllFinishedGoodStockAdj]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(finishedGoodStockAdjAdvanceSearch.SearchTerm) ? "" : finishedGoodStockAdjAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = finishedGoodStockAdjAdvanceSearch.DataTablePaging.Start;
                        if (finishedGoodStockAdjAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = finishedGoodStockAdjAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = finishedGoodStockAdjAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = finishedGoodStockAdjAdvanceSearch.ToDate;
                        if (finishedGoodStockAdjAdvanceSearch.AdjustedBy != Guid.Empty)
                            cmd.Parameters.Add("@AdjustedBy", SqlDbType.UniqueIdentifier).Value = finishedGoodStockAdjAdvanceSearch.AdjustedBy;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                finishedGoodStockAdjList = new List<FinishedGoodStockAdj>();
                                while(sdr.Read())
                                {
                                    FinishedGoodStockAdj finishedStockAdj = new FinishedGoodStockAdj();
                                    {
                                        finishedStockAdj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : finishedStockAdj.ID);
                                        finishedStockAdj.ReferenceNo = (sdr["ReferenceNo"].ToString() != "" ? sdr["ReferenceNo"].ToString() : finishedStockAdj.ReferenceNo);
                                        finishedStockAdj.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()) : finishedStockAdj.Date);
                                        finishedStockAdj.AdjustmentDateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.DateFormat) : finishedStockAdj.AdjustmentDateFormatted);
                                        finishedStockAdj.AdjustedByEmployeeName = (sdr["AdjustedByEmployeeName"].ToString() != "" ? sdr["AdjustedByEmployeeName"].ToString() : finishedStockAdj.AdjustedByEmployeeName);
                                        finishedStockAdj.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : finishedStockAdj.Remarks);
                                        finishedStockAdj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : finishedStockAdj.TotalCount);
                                        finishedStockAdj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : finishedStockAdj.FilteredCount);
                                        finishedStockAdj.ApprovalStatus = (sdr["Status"].ToString() != "" ? sdr["Status"].ToString() : finishedStockAdj.ApprovalStatus);
                                    }
                                    finishedGoodStockAdjList.Add(finishedStockAdj);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return finishedGoodStockAdjList;
        }
        #endregion

        #region InsertUpdateFinishedGoodStockAdj
        public object InsertUpdateFinishedGoodStockAdj(FinishedGoodStockAdj finishedGoodStockAdj)
        {
            SqlParameter outputStatus, IDOut = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if(con.State==ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[InsertUpdateFinishedGoodStockAdj]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = finishedGoodStockAdj.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = finishedGoodStockAdj.ID;
                        cmd.Parameters.Add("@AdjustedBy", SqlDbType.UniqueIdentifier).Value = finishedGoodStockAdj.EmployeeID;
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = finishedGoodStockAdj.AdjustmentDateFormatted;
                        cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = finishedGoodStockAdj.Remarks;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = finishedGoodStockAdj.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = finishedGoodStockAdj.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = finishedGoodStockAdj.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = finishedGoodStockAdj.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = finishedGoodStockAdj.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    switch (outputStatus.Value.ToString())
                    {
                        case "0":
                            throw new Exception(finishedGoodStockAdj.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                        case "1":
                            return new
                            {
                                ID = IDOut.Value.ToString(),
                                Status = outputStatus.Value.ToString(),
                                Message = finishedGoodStockAdj.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                            };
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return new
            {
                Code = IDOut.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = finishedGoodStockAdj.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion

        #region GetFinishedGoodStockAdj
        public FinishedGoodStockAdj GetFinishedGoodStockAdj(Guid id)
        {
            FinishedGoodStockAdj finishedGoodStockAdj = new FinishedGoodStockAdj();
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if(con.State==ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[GetFinishedGoodStockAdj]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    finishedGoodStockAdj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : finishedGoodStockAdj.ID);
                                    finishedGoodStockAdj.ReferenceNo = (sdr["ReferenceNo"].ToString() != "" ? sdr["ReferenceNo"].ToString() : finishedGoodStockAdj.ReferenceNo);
                                    finishedGoodStockAdj.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()) : finishedGoodStockAdj.Date);
                                    finishedGoodStockAdj.AdjustmentDateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.DateFormat) : finishedGoodStockAdj.AdjustmentDateFormatted);
                                    finishedGoodStockAdj.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : finishedGoodStockAdj.Remarks);
                                    finishedGoodStockAdj.AdjustedByEmployeeName = (sdr["AdjustedBy"].ToString() != "" ? sdr["AdjustedBy"].ToString() : finishedGoodStockAdj.AdjustedByEmployeeName);
                                    finishedGoodStockAdj.EmployeeID = (sdr["EmployeeID"].ToString() != "" ? Guid.Parse(sdr["EmployeeID"].ToString()) : finishedGoodStockAdj.EmployeeID);
                                    finishedGoodStockAdj.LatestApprovalID = (sdr["LatestApprovalID"].ToString() != "" ? Guid.Parse(sdr["LatestApprovalID"].ToString()) : finishedGoodStockAdj.LatestApprovalID);
                                    finishedGoodStockAdj.LatestApprovalStatus = (sdr["LatestApprovalStatus"].ToString() != "" ? Int16.Parse(sdr["LatestApprovalStatus"].ToString()) : finishedGoodStockAdj.LatestApprovalStatus);
                                    finishedGoodStockAdj.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : finishedGoodStockAdj.ApprovalStatus);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return finishedGoodStockAdj;
        }
        #endregion

        #region GetFinishedGoodStockAdjDetail
        public List<FinishedGoodStockAdjDetail> GetFinishedGoodStockAdjDetail(Guid id)
        {
            List<FinishedGoodStockAdjDetail> finishedGoodStockAdjDetailList = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if(con.State==ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[GetFinishedGoodStockAdjDetail]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                finishedGoodStockAdjDetailList = new List<FinishedGoodStockAdjDetail>();
                                while (sdr.Read())
                                {
                                    FinishedGoodStockAdjDetail finishedGoodStockAdjDetail = new FinishedGoodStockAdjDetail();
                                    {

                                        finishedGoodStockAdjDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : finishedGoodStockAdjDetail.ID);
                                        finishedGoodStockAdjDetail.AdjustmentID = (sdr["AdjustmentID"].ToString() != "" ? Guid.Parse(sdr["AdjustmentID"].ToString()) : finishedGoodStockAdjDetail.AdjustmentID);
                                        finishedGoodStockAdjDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : finishedGoodStockAdjDetail.ProductID);
                                        finishedGoodStockAdjDetail.Product = new Product();
                                        finishedGoodStockAdjDetail.Product.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : finishedGoodStockAdjDetail.Product.Code);
                                        finishedGoodStockAdjDetail.Product.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : finishedGoodStockAdjDetail.Product.UnitCode);
                                        finishedGoodStockAdjDetail.Product.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : finishedGoodStockAdjDetail.Product.Description);
                                        finishedGoodStockAdjDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : finishedGoodStockAdjDetail.Qty);
                                        finishedGoodStockAdjDetail.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : finishedGoodStockAdjDetail.Remarks);

                                    }
                                    finishedGoodStockAdjDetailList.Add(finishedGoodStockAdjDetail);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return finishedGoodStockAdjDetailList;
        }
        #endregion

        #region DeleteFinishedGoodStockAdj
        public object DeleteFinishedGoodStockAdj(Guid id)
        {
            SqlParameter outputStatus = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if(con.State==ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[DeleteFinishedGoodStockAdj]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
            };
        }
        #endregion

        #region DeleteFinishedGoodStockAdjDetail
        public object DeleteFinishedGoodStockAdjDetail(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteFinishedGoodStockAdjDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
            };
        }
        #endregion
    }
}
