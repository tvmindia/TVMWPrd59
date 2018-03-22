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
    public class MaterialStockAdjRepository : IMaterialStockAdjRepository
    {
        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();
        public MaterialStockAdjRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllMaterialStockAdjustment
        public List<MaterialStockAdj> GetAllMaterialStockAdjustment(MaterialStockAdjAdvanceSearch materialStockAdjAdvanceSearch)
        {
            List<MaterialStockAdj> materialStockAdjustmentList = null;
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
                        cmd.CommandText = "[AMC].[GetAllMaterialStockAdjustment]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(materialStockAdjAdvanceSearch.SearchTerm) ? "" : materialStockAdjAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = materialStockAdjAdvanceSearch.DataTablePaging.Start;
                        if (materialStockAdjAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = materialStockAdjAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = materialStockAdjAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = materialStockAdjAdvanceSearch.ToDate;
                        if (materialStockAdjAdvanceSearch.AdjustedBy != Guid.Empty)
                            cmd.Parameters.Add("@AdjustedBy", SqlDbType.UniqueIdentifier).Value = materialStockAdjAdvanceSearch.AdjustedBy;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialStockAdjustmentList = new List<MaterialStockAdj>();
                                while (sdr.Read())
                                {
                                    MaterialStockAdj materialStockAdjustment = new MaterialStockAdj();
                                    {
                                        materialStockAdjustment.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materialStockAdjustment.ID);
                                        materialStockAdjustment.AdjustmentNo = (sdr["AdjustmentNo"].ToString() != "" ? sdr["AdjustmentNo"].ToString() : materialStockAdjustment.AdjustmentNo);
                                        materialStockAdjustment.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()) : materialStockAdjustment.Date);
                                        materialStockAdjustment.AdjustmentDateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.DateFormat) : materialStockAdjustment.AdjustmentDateFormatted);
                                        materialStockAdjustment.AdjustedByEmployeeName = (sdr["AdjustedByEmployeeName"].ToString() != "" ? sdr["AdjustedByEmployeeName"].ToString() : materialStockAdjustment.AdjustedByEmployeeName);
                                        materialStockAdjustment.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : materialStockAdjustment.Remarks);
                                        materialStockAdjustment.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : materialStockAdjustment.TotalCount);
                                        materialStockAdjustment.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : materialStockAdjustment.FilteredCount);
                                        materialStockAdjustment.ApprovalStatus = (sdr["Status"].ToString() != "" ? sdr["Status"].ToString() : materialStockAdjustment.ApprovalStatus);
                                    }
                                    materialStockAdjustmentList.Add(materialStockAdjustment);
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
            return materialStockAdjustmentList;
        }
        #endregion GetAllMaterialStockAdjustment

        #region InsertUpdateStockAdjustment
        public object InsertUpdateStockAdjustment(MaterialStockAdj materialStockAdj)
        {
            SqlParameter outputStatus, IDOut = null;
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
                        cmd.CommandText = "[AMC].[InsertUpdateStockAdjustment]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = materialStockAdj.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = materialStockAdj.ID;
                        cmd.Parameters.Add("@AdjustedBy", SqlDbType.UniqueIdentifier).Value = materialStockAdj.EmployeeID;
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = materialStockAdj.AdjustmentDateFormatted;
                        cmd.Parameters.Add("@Remarks", SqlDbType.VarChar).Value = materialStockAdj.Remarks;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = materialStockAdj.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = materialStockAdj.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = materialStockAdj.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = materialStockAdj.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = materialStockAdj.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    switch (outputStatus.Value.ToString())
                    {
                        case "0":
                            throw new Exception(materialStockAdj.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                        case "1":
                            return new
                            {
                                ID = IDOut.Value.ToString(),
                                Status = outputStatus.Value.ToString(),
                                Message = materialStockAdj.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                            };
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Code = IDOut.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = materialStockAdj.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion

        #region GetMaterialStockAdjustment
        public MaterialStockAdj GetMaterialStockAdjustment(Guid id)
        {
            MaterialStockAdj materialStockAdj = new MaterialStockAdj();
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
                        cmd.CommandText = "[AMC].[GetMaterialStockAdjustment]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    materialStockAdj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materialStockAdj.ID);
                                    materialStockAdj.AdjustmentNo = (sdr["AdjustmentNo"].ToString() != "" ? sdr["AdjustmentNo"].ToString() : materialStockAdj.AdjustmentNo);
                                    materialStockAdj.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()) : materialStockAdj.Date);
                                    materialStockAdj.AdjustmentDateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.DateFormat) : materialStockAdj.AdjustmentDateFormatted);
                                    materialStockAdj.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : materialStockAdj.Remarks);
                                    materialStockAdj.AdjustedByEmployeeName = (sdr["AdjustedBy"].ToString() != "" ? sdr["AdjustedBy"].ToString() : materialStockAdj.AdjustedByEmployeeName);
                                    materialStockAdj.EmployeeID = (sdr["EmployeeID"].ToString() != "" ? Guid.Parse(sdr["EmployeeID"].ToString()) : materialStockAdj.EmployeeID);
                                    materialStockAdj.LatestApprovalID = (sdr["LatestApprovalID"].ToString() != "" ? Guid.Parse(sdr["LatestApprovalID"].ToString()) : materialStockAdj.LatestApprovalID);
                                    materialStockAdj.LatestApprovalStatus = (sdr["LatestApprovalStatus"].ToString() != "" ? Int16.Parse(sdr["LatestApprovalStatus"].ToString()) : materialStockAdj.LatestApprovalStatus);
                                    materialStockAdj.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : materialStockAdj.ApprovalStatus);
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
            return materialStockAdj;
        }
        #endregion

        #region GetMaterialStockAdjustmentDetail
        public List<MaterialStockAdjDetail> GetMaterialStockAdjustmentDetail(Guid id)
        {
            List<MaterialStockAdjDetail> materialStockAdjList = null;
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
                        cmd.CommandText = "[AMC].[GetMaterialStockAdjustmentDetail]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialStockAdjList = new List<MaterialStockAdjDetail>();
                                while(sdr.Read())
                                {
                                    MaterialStockAdjDetail materialStokAdjDetail = new MaterialStockAdjDetail();
                                    {
                                        
                                        materialStokAdjDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materialStokAdjDetail.ID);
                                        materialStokAdjDetail.AdjustmentID=(sdr["AdjustmentID"].ToString() != "" ? Guid.Parse(sdr["AdjustmentID"].ToString()) : materialStokAdjDetail.AdjustmentID);
                                        materialStokAdjDetail.MaterialID= (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : materialStokAdjDetail.MaterialID);
                                        materialStokAdjDetail.Material = new Material();
                                        materialStokAdjDetail.Material.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : materialStokAdjDetail.Material.MaterialCode);
                                        materialStokAdjDetail.Material.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : materialStokAdjDetail.Material.UnitCode);
                                        materialStokAdjDetail.Material.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : materialStokAdjDetail.Material.Description);
                                        materialStokAdjDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : materialStokAdjDetail.Qty);
                                        materialStokAdjDetail.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : materialStokAdjDetail.Remarks);
                                        
                                    }
                                    materialStockAdjList.Add(materialStokAdjDetail);
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
            return materialStockAdjList;
        }
        #endregion

        #region DeleteMaterialStockAdjustment
        public object DeleteMaterialStockAdjustment(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteMaterialStockAdjustment]";
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

        #region DeleteMaterialStockAdjustmentDetail
        public object DeleteMaterialStockAdjustmentDetail(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteMaterialStockAdjustmentdetail]";
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


