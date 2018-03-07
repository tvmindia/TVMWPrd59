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
    public class MaterialStockAdjRepository:IMaterialStockAdjRepository
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
                                        materialStockAdjustment.ReferenceNo = (sdr["ReferenceNo"].ToString() != "" ? sdr["ReferenceNo"].ToString() : materialStockAdjustment.ReferenceNo);
                                        materialStockAdjustment.AdjustmentDate = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()) : materialStockAdjustment.AdjustmentDate);
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
            catch(Exception ex)
            {
                throw ex;
            }
            return materialStockAdjustmentList;
        }
        #endregion GetAllMaterialStockAdjustment
    }
}
