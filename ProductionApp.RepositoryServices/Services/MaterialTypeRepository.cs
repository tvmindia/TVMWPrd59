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
    public class MaterialTypeRepository: IMaterialTypeRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public MaterialTypeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetMaterialTypeForSelectList
        /// <summary>
        /// To Get List of All MaterialType for Select List
        /// </summary>
        /// <returns>List</returns>
        public List<MaterialType> GetMaterialTypeForSelectList()
        {
            List<MaterialType> materialTypeList = null;
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
                        cmd.CommandText = "[AMC].[GetMaterialTypeForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialTypeList = new List<MaterialType>();
                                while (sdr.Read())
                                {
                                    MaterialType materialType = new MaterialType();
                                    {
                                        materialType.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : materialType.Code);
                                        materialType.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : materialType.Description);
                                    }
                                    materialTypeList.Add(materialType);
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
            return materialTypeList;
        }
        #endregion GetMaterialTypeForSelectList

        #region InsertUpdateMaterialType
        /// <summary>
        /// To Insert and update Material Type
        /// </summary>
        /// <param name="materialType"></param>
        /// <returns>object</returns>
        public object InsertUpdateMaterialType(MaterialType materialType)
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
                        cmd.CommandText = "[AMC].[InsertUpdateMaterialType]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = materialType.IsUpdate;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = materialType.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 250).Value = materialType.Description;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 20);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(materialType.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        materialType.Code = outputCode.Value.ToString();
                        return new
                        {
                            Code = materialType.Code,
                            Status = outputStatus.Value.ToString(),
                            Message = materialType.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                ID = Guid.Parse(outputCode.Value.ToString()),
                Status = outputStatus.Value.ToString(),
                Message = materialType.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateMaterialType

        public MaterialType GetMaterialType(string code)
        {
            MaterialType materialType = null;
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
                        cmd.CommandText = "[AMC].[GetMaterialType]";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 10).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    materialType = new MaterialType();
                                    materialType.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : materialType.Code);
                                    materialType.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : materialType.Description);
                                    
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return materialType;
        }
    }
}
