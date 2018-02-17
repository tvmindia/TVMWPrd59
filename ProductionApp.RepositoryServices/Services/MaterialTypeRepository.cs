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
    }
}
