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
    public class RawMaterialRepository: IRawMaterialRepository
    {
        private IDatabaseFactory _databaseFactory;
        public RawMaterialRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetAllRawMaterial
        /// <summary>
        /// To Get List of All raw materials
        /// </summary>
        /// <param name="rawMaterialAdvanceSearch"></param>
        /// <returns>List</returns>
        public List<RawMaterial> GetAllRawMaterial(RawMaterialAdvanceSearch rawMaterialAdvanceSearch)
        {
            List<RawMaterial> rawMaterialList = null;
            try
            {
                using(SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd=new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[GetAllRawMaterial]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(rawMaterialAdvanceSearch.SearchTerm) ? "": rawMaterialAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = rawMaterialAdvanceSearch.DataTablePaging.Start;
                        if (rawMaterialAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = rawMaterialAdvanceSearch.DataTablePaging.Length;
                        //cmd.Parameters.Add("@OrderDir", SqlDbType.NVarChar, 5).Value = model.order[0].dir;
                        //cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar, -1).Value = model.order[0].column;
                        //
                        cmd.CommandType = CommandType.StoredProcedure;
                        using(SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if((sdr!=null) && (sdr.HasRows))
                            {
                                rawMaterialList = new List<RawMaterial>();
                                while (sdr.Read())
                                {
                                    RawMaterial rawMaterial = new RawMaterial();
                                    {
                                        rawMaterial.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : rawMaterial.ID);
                                        rawMaterial.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : rawMaterial.MaterialCode);
                                        rawMaterial.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : rawMaterial.Rate);
                                        rawMaterial.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : rawMaterial.Type);
                                        rawMaterial.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : rawMaterial.Description);
                                        rawMaterial.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : rawMaterial.UnitCode);
                                        rawMaterial.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : rawMaterial.FilteredCount);
                                        rawMaterial.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : rawMaterial.TotalCount);
                                    }
                                    rawMaterialList.Add(rawMaterial);
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
            return rawMaterialList;
        }
        #endregion GetAllRawMaterial

        #region InsertUpdateRawMaterial
        //public object InsertUpdateRawMaterial(RawMaterial rawMaterial)
        //{
        //    SqlParameter outputStatus, OutputID;
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
        //                cmd.CommandText = "[AMC].[InsertUpdateRawMaterial]";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = rawMaterial.IsUpdate;
        //                cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = rawMaterial.ID;
        //                cmd.Parameters.Add("@MaterialCode", SqlDbType.VarChar).Value = rawMaterial.MaterialCode;
        //                cmd.Parameters.Add("@Rate", SqlDbType.Decimal).Value = rawMaterial.Rate;
        //                cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = rawMaterial.Type;
        //                cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = rawMaterial.Description;
        //                cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = rawMaterial.UnitCode;
        //                cmd.Parameters.Add("@ReorderQty", SqlDbType.Decimal).Value = rawMaterial.ReorderQty;
        //                cmd.Parameters.Add("@CreatedBy",SqlDbType.VarChar).Value=rawMaterial.
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion InsertUpdateRawMaterial

    }
}
