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

    }
}
