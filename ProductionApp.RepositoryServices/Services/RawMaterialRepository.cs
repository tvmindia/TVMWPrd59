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
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public RawMaterialRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetRawMaterialForSelectList
        /// <summary>
        /// To Get List of All raw materials for Select List
        /// </summary>
        /// <returns>List</returns>
        public List<RawMaterial> GetRawMaterialForSelectList()
        {
            List<RawMaterial> rawMaterialList = null;
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
                        cmd.CommandText = "[AMC].[GetRawMaterialForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                rawMaterialList = new List<RawMaterial>();
                                while (sdr.Read())
                                {
                                    RawMaterial rawMaterial = new RawMaterial();
                                    {
                                        rawMaterial.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : rawMaterial.ID);
                                        rawMaterial.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : rawMaterial.MaterialCode);
                                        rawMaterial.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : rawMaterial.Rate);
                                        rawMaterial.MaterialType = (sdr["MaterialType"].ToString() != "" ? sdr["MaterialType"].ToString() : rawMaterial.MaterialType);
                                        rawMaterial.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : rawMaterial.Description);
                                        rawMaterial.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : rawMaterial.UnitCode);
                                        rawMaterial.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? decimal.Parse(sdr["ReorderQty"].ToString()) : rawMaterial.ReorderQty);
                                    }
                                    rawMaterialList.Add(rawMaterial);
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
            return rawMaterialList;
        }
        #endregion GetRawMaterialForSelectList

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
                                        rawMaterial.MaterialType = (sdr["MaterialType"].ToString() != "" ? sdr["MaterialType"].ToString() : rawMaterial.MaterialType);
                                        rawMaterial.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : rawMaterial.Description);
                                        rawMaterial.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : rawMaterial.UnitCode);
                                        rawMaterial.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? decimal.Parse(sdr["ReorderQty"].ToString()) : rawMaterial.ReorderQty);
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

        #region CheckMaterialCodeExist
        /// <summary>
        /// To Check whether MaterialCode Existing or not
        /// </summary>
        /// <param name="materialCode"></param>
        /// <returns>bool</returns>
        public bool CheckMaterialCodeExist(string materialCode)
       {
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
                        cmd.CommandText = "[AMC].[CheckMaterialCodeExist]";
                        cmd.Parameters.Add("@MaterialCode", SqlDbType.VarChar).Value = materialCode;
                        cmd.CommandType = CommandType.StoredProcedure;
                        Object res = cmd.ExecuteScalar();
                        return (res.ToString() == "Exists" ? true : false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion CheckMaterialCodeExist

        #region InsertUpdateRawMaterial
        /// <summary>
        /// To Insert and update Raw Material
        /// </summary>
        /// <param name="rawMaterial"></param>
        /// <returns>object</returns>
        public object InsertUpdateRawMaterial(RawMaterial rawMaterial)
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
                        cmd.CommandText = "[AMC].[InsertUpdateRawMaterial]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = rawMaterial.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = rawMaterial.ID;
                        cmd.Parameters.Add("@MaterialCode", SqlDbType.VarChar).Value = rawMaterial.MaterialCode;
                        cmd.Parameters.Add("@Rate", SqlDbType.Decimal).Value = rawMaterial.Rate;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = rawMaterial.MaterialType;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = rawMaterial.Description;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = rawMaterial.UnitCode;
                        cmd.Parameters.Add("@ReorderQty", SqlDbType.Decimal).Value = rawMaterial.ReorderQty;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = rawMaterial.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = rawMaterial.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = rawMaterial.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = rawMaterial.Common.UpdatedDate;
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
                        throw new Exception(rawMaterial.IsUpdate ? _appConst.UpdateFailure:_appConst.InsertFailure);
                    case "1":
                        rawMaterial.ID = Guid.Parse(OutputID.Value.ToString());
                        return new
                        {
                            ID = Guid.Parse(OutputID.Value.ToString()),
                            Status = outputStatus.Value.ToString(),
                            Message = rawMaterial.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = rawMaterial.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateRawMaterial

        #region GetRawMaterial
        /// <summary>
        /// To Get RawMaterial Details corresponding to ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>RawMaterial</returns>
        public RawMaterial GetRawMaterial(Guid id)
        {
            RawMaterial rawMaterial=null;

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
                        cmd.CommandText = "[AMC].[GetRawMaterial]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    rawMaterial = new RawMaterial();
                                    rawMaterial.ID = sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : rawMaterial.ID;
                                    rawMaterial.MaterialCode = sdr["MaterialCode"].ToString() != "" ? (sdr["MaterialCode"].ToString()) : rawMaterial.MaterialCode;
                                    rawMaterial.Rate = sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : rawMaterial.Rate;
                                    rawMaterial.MaterialType = sdr["MaterialType"].ToString() != "" ? sdr["MaterialType"].ToString() : rawMaterial.MaterialType;
                                    rawMaterial.Description = sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : rawMaterial.Description;
                                    rawMaterial.UnitCode = sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : rawMaterial.UnitCode;
                                    rawMaterial.ReorderQty = sdr["ReorderQty"].ToString() != "" ? decimal.Parse(sdr["ReorderQty"].ToString()) : rawMaterial.ReorderQty;
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
            return rawMaterial;
        }
        #endregion GetRawMaterial

    }
}
