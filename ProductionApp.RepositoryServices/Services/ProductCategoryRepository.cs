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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ProductCategoryRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection 

        #region GetProductCategoryForSelectList
        /// <summary>
        /// To Get Product Category List For DropDown
        /// </summary>
        /// <returns>List</returns>
        public List<ProductCategory> GetProductCategoryForSelectList()
        {
            List<ProductCategory> productCategoryList = null;
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
                        cmd.CommandText = "[AMC].[GetProductCategoryForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productCategoryList = new List<ProductCategory>();
                                while (sdr.Read())
                                {
                                    ProductCategory productCategory = new ProductCategory();
                                    {
                                        productCategory.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : productCategory.Code);
                                        productCategory.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : productCategory.Description);
                                    }
                                    productCategoryList.Add(productCategory);
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
            return productCategoryList;
        }
        #endregion GetProductCategoryForSelectList

        #region GetAllProductCategory
        /// <summary>
        /// To Get List of All Product Category
        /// </summary>
        /// <param name="productCategoryAdvanceSearch"></param>
        /// <returns>List</returns>
        public List<ProductCategory> GetAllProductCategory(ProductCategoryAdvanceSearch productCategoryAdvanceSearch)
        {
            List<ProductCategory> productCategoryList = null;
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
                        cmd.CommandText = "[AMC].[GetAllProductCategory]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(productCategoryAdvanceSearch.SearchTerm) ? "" : productCategoryAdvanceSearch.SearchTerm.Trim();
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productCategoryAdvanceSearch.DataTablePaging.Start;
                        if (productCategoryAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productCategoryAdvanceSearch.DataTablePaging.Length;
                        //cmd.Parameters.Add("@OrderDir", SqlDbType.NVarChar, 5).Value = model.order[0].dir;
                        //cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar, -1).Value = model.order[0].column;
                        //cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar, 15).Value = productCategoryAdvanceSearch.Unit.Code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productCategoryList = new List<ProductCategory>();
                                while (sdr.Read())
                                {
                                    ProductCategory productCategory = new ProductCategory();
                                    {
                                        productCategory.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : productCategory.Code);
                                        productCategory.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : productCategory.Description);
                                        productCategory.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : productCategory.FilteredCount);
                                        productCategory.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : productCategory.TotalCount);
                                    }
                                    productCategoryList.Add(productCategory);
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
            return productCategoryList;
        }
        #endregion GetAllProductCategory

        #region GetProductCategory
        /// <summary>
        /// To Get Product Category Details corresponding to ID
        /// </summary>
        /// <param name="code"></param>
        /// <returns>ProductCategory</returns>
        public ProductCategory GetProductCategory(string code)
        {
            ProductCategory productCategory = null;

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
                        cmd.CommandText = "[AMC].[GetProductCategory]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = code;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    productCategory = new ProductCategory();
                                    productCategory.Code = sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : productCategory.Code;
                                    productCategory.Description = sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : productCategory.Description;
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
            return productCategory;
        }
        #endregion GetProductCategory

        #region InsertUpdateProductCategory
        /// <summary>
        /// To Insert and update Product Category
        /// </summary>
        /// <param name="productCategory"></param>
        /// <returns>object</returns>
        public object InsertUpdateProductCategory(ProductCategory productCategory)
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
                        cmd.CommandText = "[AMC].[InsertUpdateProductCategory]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = productCategory.IsUpdate;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar,20).Value = productCategory.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar,250).Value = productCategory.Description;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = productCategory.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = productCategory.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = productCategory.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = productCategory.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar,20);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(productCategory.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        productCategory.Code = outputCode.Value.ToString();
                        return new
                        {
                            Code = productCategory.Code,
                            Status = outputStatus.Value.ToString(),
                            Message = productCategory.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Code = productCategory.Code,
                Status = outputStatus.Value.ToString(),
                Message = productCategory.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateProductCategory

        #region DeleteProductCategory
        /// <summary>
        /// To Delete Product Category
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public object DeleteProductCategory(string code)
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
                        cmd.CommandText = "[AMC].[DeleteProductCategory]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar,20).Value = code;
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
        #endregion DeleteProductCategory

    }
}
