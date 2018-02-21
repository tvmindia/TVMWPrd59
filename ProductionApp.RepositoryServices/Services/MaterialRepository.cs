﻿using ProductionApp.DataAccessObject.DTO;
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
    public class MaterialRepository: IMaterialRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public MaterialRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetMaterialForSelectList
        /// <summary>
        /// To Get List of All material for Select List
        /// </summary>
        /// <returns>List</returns>
        public List<Material> GetMaterialForSelectList()
        {
            List<Material> materialList = null;
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
                        cmd.CommandText = "[AMC].[GetMaterialForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialList = new List<Material>();
                                while (sdr.Read())
                                {
                                    Material material = new Material();
                                    {
                                        material.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : material.ID);
                                        material.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : material.MaterialCode);
                                        material.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : material.Rate);
                                        material.MaterialTypeCode = (sdr["MaterialTypeCode"].ToString() != "" ? sdr["MaterialTypeCode"].ToString() : material.MaterialTypeCode);
                                        material.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : material.Description);
                                        material.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : material.UnitCode);
                                        material.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? decimal.Parse(sdr["ReorderQty"].ToString()) : material.ReorderQty);
                                    }
                                    materialList.Add(material);
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
            return materialList;
        }
        #endregion GetMaterialForSelectList

        #region GetAllMaterial
        /// <summary>
        /// To Get List of All materials
        /// </summary>
        /// <param name="materialAdvanceSearch"></param>
        /// <returns>List</returns>
        public List<Material> GetAllMaterial(MaterialAdvanceSearch materialAdvanceSearch)
        {
            List<Material> materialList = null;
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
                        cmd.CommandText = "[AMC].[GetAllMaterial]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(materialAdvanceSearch.SearchTerm) ? "": materialAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = materialAdvanceSearch.DataTablePaging.Start;
                        if (materialAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = materialAdvanceSearch.DataTablePaging.Length;
                        //cmd.Parameters.Add("@OrderDir", SqlDbType.NVarChar, 5).Value = model.order[0].dir;
                        //cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar, -1).Value = model.order[0].column;
                        //
                        cmd.CommandType = CommandType.StoredProcedure;
                        using(SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if((sdr!=null) && (sdr.HasRows))
                            {
                                materialList = new List<Material>();
                                while (sdr.Read())
                                {
                                    Material material = new Material();
                                    {
                                        material.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : material.ID);
                                        material.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : material.MaterialCode);
                                        material.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : material.Rate);
                                        material.MaterialTypeCode = (sdr["MaterialTypeCode"].ToString() != "" ? sdr["MaterialTypeCode"].ToString() : material.MaterialTypeCode);
                                        material.MaterialType = new MaterialType();
                                        material.MaterialType.Description= (sdr["MaterialDescription"].ToString() != "" ? sdr["MaterialDescription"].ToString() : material.MaterialType.Description);
                                        material.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : material.Description);
                                        material.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : material.UnitCode);
                                        material.Unit = new Unit();
                                        material.Unit.Description= (sdr["UnitDescription"].ToString() != "" ? sdr["UnitDescription"].ToString() : material.UnitCode);
                                        material.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? decimal.Parse(sdr["ReorderQty"].ToString()) : material.ReorderQty);
                                        material.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : material.FilteredCount);
                                        material.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : material.TotalCount);
                                    }
                                    materialList.Add(material);
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
            return materialList;
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

        #region InsertUpdateMaterial
        /// <summary>
        /// To Insert and update Raw Material
        /// </summary>
        /// <param name="material"></param>
        /// <returns>object</returns>
        public object InsertUpdateMaterial(Material material)
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
                        cmd.CommandText = "[AMC].[InsertUpdateMaterial]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = material.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = material.ID;
                        cmd.Parameters.Add("@MaterialCode", SqlDbType.VarChar).Value = material.MaterialCode;
                        cmd.Parameters.Add("@Rate", SqlDbType.Decimal).Value = material.Rate;
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar).Value = material.MaterialTypeCode;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = material.Description;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = material.UnitCode;
                        cmd.Parameters.Add("@ReorderQty", SqlDbType.Decimal).Value = material.ReorderQty;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = material.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = material.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = material.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = material.Common.UpdatedDate;
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
                        throw new Exception(material.IsUpdate ? _appConst.UpdateFailure:_appConst.InsertFailure);
                    case "1":
                        material.ID = Guid.Parse(OutputID.Value.ToString());
                        return new
                        {
                            ID = Guid.Parse(OutputID.Value.ToString()),
                            Status = outputStatus.Value.ToString(),
                            Message = material.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = material.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateMaterial

        #region GetMaterial
        /// <summary>
        /// To Get Material Details corresponding to ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Material</returns>
        public Material GetMaterial(Guid id)
        {
            Material material=null;

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
                        cmd.CommandText = "[AMC].[GetMaterial]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    material = new Material();
                                    material.ID = sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : material.ID;
                                    material.MaterialCode = sdr["MaterialCode"].ToString() != "" ? (sdr["MaterialCode"].ToString()) : material.MaterialCode;
                                    material.Rate = sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : material.Rate;
                                    material.MaterialTypeCode = sdr["MaterialTypeCode"].ToString() != "" ? sdr["MaterialTypeCode"].ToString() : material.MaterialTypeCode;
                                    material.MaterialType = new MaterialType();
                                    material.MaterialType.Code= sdr["MaterialTypeCode"].ToString() != "" ? sdr["MaterialTypeCode"].ToString() : material.MaterialTypeCode;
                                    material.Description = sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : material.Description;
                                    material.UnitCode = sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : material.UnitCode;
                                    material.Unit = new Unit();
                                    material.Unit.Code= sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : material.UnitCode;
                                    material.CurrentStock = sdr["CurrentStock"].ToString() != "" ? sdr["CurrentStock"].ToString() : material.CurrentStock;
                                    material.ReorderQty = sdr["ReorderQty"].ToString() != "" ? decimal.Parse(sdr["ReorderQty"].ToString()) : material.ReorderQty;
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
            return material;
        }
        #endregion GetRawMaterial

    }
}