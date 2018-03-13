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

        #region GetProductForSelectList
        /// <summary>
        /// To Get Product List For DropDown
        /// </summary>
        /// <returns>List</returns>
        public List<Product> GetProductForSelectList()
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
                        cmd.CommandText = "[AMC].[GetProductForSelectList]";
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
                                        product.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : product.Code);
                                        product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : product.Name);
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
        #endregion GetProductForSelectList

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
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(productAdvanceSearch.SearchTerm) ? "" : productAdvanceSearch.SearchTerm.Trim();
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productAdvanceSearch.DataTablePaging.Start;
                        if (productAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productAdvanceSearch.DataTablePaging.Length;
                        //cmd.Parameters.Add("@OrderDir", SqlDbType.NVarChar, 5).Value = model.order[0].dir;
                        //cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar, -1).Value = model.order[0].column;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar,15).Value = productAdvanceSearch.Unit.Code;
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
                                        product.Unit = new Unit();
                                        product.Unit.Description= (sdr["UnitDescrption"].ToString() != "" ? sdr["UnitDescrption"].ToString() : product.Unit.Description);
                                        product.Category = (sdr["Category"].ToString() != "" ? sdr["Category"].ToString() : product.Category);
                                        product.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : product.Rate);
                                        product.OpeningStock = (sdr["OpeningStock"].ToString() != "" ? decimal.Parse(sdr["OpeningStock"].ToString()) : product.OpeningStock);
                                        product.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : product.CurrentStock);
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

        #region CheckProductCodeExist
        /// <summary>
        /// To Check whether Product Code Existing or not
        /// </summary>
        /// <param name="productCode"></param>
        /// <returns>bool</returns>
        public bool CheckProductCodeExist(string productCode)
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
                        cmd.CommandText = "[AMC].[CheckProductCodeExist]";
                        cmd.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = productCode;
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
        #endregion CheckProductCodeExist

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
                                    product.Description = sdr["Description"].ToString() != "" ? sdr["Description"].ToString() :product.Description;
                                    product.UnitCode = sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : product.UnitCode;
                                    product.Unit = new Unit();
                                    product.Unit.Code = sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : product.UnitCode;
                                    product.Category = sdr["Category"].ToString() != "" ? sdr["Category"].ToString() : product.Category;
                                    product.Rate = sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : product.Rate;
                                    product.HSNNo = sdr["HSNNo"].ToString() != "" ? sdr["HSNNo"].ToString() : product.HSNNo;
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

        #region DeleteProduct
        /// <summary>
        /// To Delete Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object DeleteProduct(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteProduct]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
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
        #endregion DeleteProduct


        #region Get FG Summary
        /// <summary>
        /// To Get List of All Product
        /// </summary>
     
        /// <returns>List</returns>
        public List<FinishedGoodSummary> GetFinishGoodsSummary()
        {
            List<FinishedGoodSummary> FinishedGoodSummaryList = null;
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
                        cmd.CommandText = "[AMC].[GetFinishGoodsSummary]";
                       
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                FinishedGoodSummaryList = new List<FinishedGoodSummary>();
                                while (sdr.Read())
                                {
                                    FinishedGoodSummary FinishedGoodSummary = new FinishedGoodSummary();
                                    {
                                       
                                        FinishedGoodSummary.Category = (sdr["Category"].ToString() != "" ? sdr["Category"].ToString() : FinishedGoodSummary.Category);                                       
                                        FinishedGoodSummary.Value = (sdr["Value"].ToString() != "" ? decimal.Parse(sdr["Value"].ToString()) : FinishedGoodSummary.Value);
                                        
                                    }
                                    FinishedGoodSummaryList.Add(FinishedGoodSummary);
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
            return FinishedGoodSummaryList;
        }
        #endregion  Get FG Summary

    }
}
