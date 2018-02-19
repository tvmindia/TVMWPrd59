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
    public class ProductRepository: IProductRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ProductRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetAllProduct
        /// <summary>
        /// To Get List of All Product
        /// </summary>
        /// <param name="productAdvanceSearch"></param>
        /// <returns>List</returns>
        public List<Product> GetAllProduct(ProductAdvanceSearch productAdvanceSearch)
        {
            List<Product> productList = null;
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
                        cmd.CommandText = "[AMC].[GetAllProduct]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(productAdvanceSearch.SearchTerm) ? "" : productAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productAdvanceSearch.DataTablePaging.Start;
                        if (productAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productAdvanceSearch.DataTablePaging.Length;
                        //cmd.Parameters.Add("@OrderDir", SqlDbType.NVarChar, 5).Value = model.order[0].dir;
                        //cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar, -1).Value = model.order[0].column;
                        //
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productList = new List<Product>();
                                while (sdr.Read())
                                {
                                    Product product = new Product();
                                    {
                                        product.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : product.ID);
                                        product.Code  = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : product.Code);
                                        product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : product.Name);
                                        product.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : product.Description);
                                        product.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : product.UnitCode);
                                        product.Category = (sdr["Category"].ToString() != "" ? sdr["Category"].ToString() : product.Category);
                                        product.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : product.Rate);
                                        product.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : product.FilteredCount);
                                        product.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : product.TotalCount);
                                    }
                                    productList.Add(product);
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
            return productList;
        }
        #endregion GetAllProduct

        #region InsertUpdateProduct
        /// <summary>
        /// To Insert and update Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>object</returns>
        public object InsertUpdateProduct(Product product)
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
                        cmd.CommandText = "[AMC].[InsertUpdateProduct]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = product.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = product.ID;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = product.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = product.Name;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar).Value = product.Description;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar).Value = product.UnitCode;
                        cmd.Parameters.Add("@Category", SqlDbType.VarChar).Value = product.Category;
                        cmd.Parameters.Add("@Rate", SqlDbType.Decimal).Value = product.Rate;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = product.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = product.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = product.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = product.Common.UpdatedDate;
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
                        throw new Exception(product.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        product.ID = Guid.Parse(OutputID.Value.ToString());
                        return new
                        {
                            ID = Guid.Parse(OutputID.Value.ToString()),
                            Status = outputStatus.Value.ToString(),
                            Message = product.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = product.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateProduct

        #region GetProduct
        /// <summary>
        /// To Get Product Details corresponding to ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product</returns>
        public Product GetProduct(Guid id)
        {
            Product product = null;

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
                        cmd.CommandText = "[AMC].[GetProduct]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    product = new Product();
                                    product.ID = sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : product.ID;
                                    product.Code = sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : product.Code;
                                    product.Name = sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : product.Name;
                                    product.Description = sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : product.Description;
                                    product.UnitCode = sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : product.UnitCode;
                                    product.Category = sdr["Category"].ToString() != "" ? sdr["Category"].ToString() : product.Category;
                                    product.Rate = sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : product.Rate;
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
            return product;
        }
        #endregion GetProduct

    }
}