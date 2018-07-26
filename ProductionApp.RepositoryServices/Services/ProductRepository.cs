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
        public List<Product> GetProductForSelectList(string type)
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
                        cmd.Parameters.Add("@Type", SqlDbType.VarChar,5).Value = type;
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
                        cmd.Parameters.Add("@ProductCategoryCode", SqlDbType.VarChar,20).Value = productAdvanceSearch.ProductCategory.Code;
                        cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 20).Value = productAdvanceSearch.Type;
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
                                        product.ProductCategoryCode = (sdr["ProductCategoryCode"].ToString() != "" ? sdr["ProductCategoryCode"].ToString() : product.ProductCategoryCode);
                                        product.ProductCategory = new ProductCategory();
                                        product.ProductCategory.Description= (sdr["ProductCategoryDescription"].ToString() != "" ? sdr["ProductCategoryDescription"].ToString() : product.ProductCategoryCode);
                                        product.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? decimal.Parse(sdr["ReorderQty"].ToString()) : product.ReorderQty);
                                        product.OpeningStock = (sdr["OpeningStock"].ToString() != "" ? decimal.Parse(sdr["OpeningStock"].ToString()) : product.OpeningStock);
                                        product.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : product.CurrentStock);
                                        product.HSNNo = (sdr["HSNNo"].ToString() != "" ? sdr["HSNNo"].ToString() : product.HSNNo);
                                        product.WeightInKG = (sdr["WeightInKG"].ToString() != "" ? decimal.Parse(sdr["WeightInKG"].ToString()) : product.WeightInKG);
                                        product.CostPrice = (sdr["CostPrice"].ToString() != "" ? decimal.Parse(sdr["CostPrice"].ToString()) : product.CostPrice);
                                        product.CostPricePerPiece = (sdr["CostPricePerPiece"].ToString() != "" ? decimal.Parse(sdr["CostPricePerPiece"].ToString()) : product.CostPricePerPiece);
                                        product.SellingPriceInKG = (sdr["SellingPriceInKG"].ToString() != "" ? decimal.Parse(sdr["SellingPriceInKG"].ToString()) : product.SellingPriceInKG);
                                        product.SellingPricePerPiece = (sdr["SellingPricePerPiece"].ToString() != "" ? decimal.Parse(sdr["SellingPricePerPiece"].ToString()) : product.SellingPricePerPiece);
                                        product.IsInvoiceInKG= (sdr["IsInvoiceInKG"].ToString() != "" ? bool.Parse(sdr["IsInvoiceInKG"].ToString()) : product.IsInvoiceInKG);
                                        product.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : product.Type);
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
                        cmd.Parameters.Add("@ProductCategoryCode", SqlDbType.VarChar).Value = product.ProductCategoryCode;
                        cmd.Parameters.Add("@ReorderQty", SqlDbType.Decimal).Value = product.ReorderQty;
                        cmd.Parameters.Add("@OpeningStock", SqlDbType.Decimal).Value = product.OpeningStock;
                        cmd.Parameters.Add("@CurrentStock", SqlDbType.Decimal).Value = product.CurrentStock;
                        cmd.Parameters.Add("@HSNNo", SqlDbType.NVarChar,30).Value = product.HSNNo;
                        cmd.Parameters.Add("@WeightInKG", SqlDbType.Decimal).Value = product.WeightInKG;
                        cmd.Parameters.Add("@CostPrice", SqlDbType.Decimal).Value = product.CostPrice;
                        cmd.Parameters.Add("@CostPricePerPiece", SqlDbType.Decimal).Value = product.CostPricePerPiece;
                        cmd.Parameters.Add("@SellingPriceInKG", SqlDbType.Decimal).Value = product.SellingPriceInKG;
                        cmd.Parameters.Add("@SellingPricePerPiece", SqlDbType.Decimal).Value = product.SellingPricePerPiece;
                        cmd.Parameters.Add("@IsInvoiceInKG", SqlDbType.Bit).Value = product.IsInvoiceInKG;
                        cmd.Parameters.Add("@Type", SqlDbType.NVarChar,5).Value = product.Type;
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
        public bool CheckProductCodeExist(Product product)
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
                        cmd.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = product.Code;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = product.ID;
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
                        cmd.CommandText = "[AMC].[GetProductStockDetail]";
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
                                    product.ProductCategory = new ProductCategory();
                                    product.ProductCategory.Code= sdr["ProductCategoryCode"].ToString() != "" ? sdr["ProductCategoryCode"].ToString() : product.ProductCategoryCode;
                                    product.ProductCategoryCode = sdr["ProductCategoryCode"].ToString() != "" ? sdr["ProductCategoryCode"].ToString() : product.ProductCategoryCode;
                                    product.ReorderQty = sdr["ReorderQty"].ToString() != "" ? decimal.Parse(sdr["ReorderQty"].ToString()) : product.ReorderQty;
                                    product.OpeningStock = sdr["OpeningStock"].ToString() != "" ? decimal.Parse(sdr["OpeningStock"].ToString()) : product.OpeningStock;
                                    product.CurrentStock = sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : product.CurrentStock;
                                    product.WeightInKG = sdr["WeightInKG"].ToString() != "" ? decimal.Parse(sdr["WeightInKG"].ToString()) : product.WeightInKG;
                                    product.CostPrice = sdr["CostPrice"].ToString() != "" ? decimal.Parse(sdr["CostPrice"].ToString()) : product.CostPrice;
                                    product.CostPricePerPiece = sdr["CostPricePerPiece"].ToString() != "" ? decimal.Parse(sdr["CostPricePerPiece"].ToString()) : product.CostPricePerPiece;
                                    product.SellingPriceInKG = sdr["SellingPriceInKG"].ToString() != "" ? decimal.Parse(sdr["SellingPriceInKG"].ToString()) : product.SellingPriceInKG;
                                    product.SellingPricePerPiece = sdr["SellingPricePerPiece"].ToString() != "" ? decimal.Parse(sdr["SellingPricePerPiece"].ToString()) : product.SellingPricePerPiece;
                                    product.IsInvoiceInKG = sdr["IsInvoiceInKG"].ToString() != "" ? bool.Parse(sdr["IsInvoiceInKG"].ToString()) : product.IsInvoiceInKG;
                                    product.Type = sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : product.Type;
                                    product.HSNNo = sdr["HSNNo"].ToString() != "" ? sdr["HSNNo"].ToString() : product.HSNNo;
                                    product.Rate = sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : product.Rate;
                                    product.OrderDue = (sdr["OrderDue"].ToString() != "" ? decimal.Parse(sdr["OrderDue"].ToString()) : product.OrderDue);
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
        public object DeleteProduct(Guid id ,string deletedBy)
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
                        cmd.Parameters.Add("@DeletedBy", SqlDbType.NVarChar, 255).Value = deletedBy;
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

        #region GetProductListForBillOfMaterial
        public List<Product> GetProductListForBillOfMaterial(string componentIDs)
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
                        cmd.CommandText = "[AMC].[GetProductListForBillOfMaterial]";
                        cmd.Parameters.Add("@ComponentIDs", SqlDbType.NVarChar).Value = componentIDs;
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
                                        product.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : product.Description);
                                        product.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : product.UnitCode);
                                        product.Unit = new Unit();
                                        product.Unit.Description = (sdr["UnitDescrption"].ToString() != "" ? sdr["UnitDescrption"].ToString() : product.Unit.Description);
                                        product.ProductCategoryCode = (sdr["ProductCategoryCode"].ToString() != "" ? sdr["ProductCategoryCode"].ToString() : product.ProductCategoryCode);
                                        product.ProductCategory = new ProductCategory();
                                        product.ProductCategory.Description = (sdr["ProductCategoryDescription"].ToString() != "" ? sdr["ProductCategoryDescription"].ToString() : product.ProductCategoryCode);
                                        product.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? decimal.Parse(sdr["ReorderQty"].ToString()) : product.ReorderQty);
                                        product.OpeningStock = (sdr["OpeningStock"].ToString() != "" ? decimal.Parse(sdr["OpeningStock"].ToString()) : product.OpeningStock);
                                        product.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : product.CurrentStock);
                                        product.HSNNo = (sdr["HSNNo"].ToString() != "" ? sdr["HSNNo"].ToString() : product.HSNNo);
                                        product.WeightInKG = (sdr["WeightInKG"].ToString() != "" ? decimal.Parse(sdr["WeightInKG"].ToString()) : product.WeightInKG);
                                        product.CostPrice = (sdr["CostPrice"].ToString() != "" ? decimal.Parse(sdr["CostPrice"].ToString()) : product.CostPrice);
                                        //product.SellingPrice = (sdr["SellingPrice"].ToString() != "" ? decimal.Parse(sdr["SellingPrice"].ToString()) : product.SellingPrice);
                                        product.SellingPriceInKG = (sdr["SellingPriceInKG"].ToString() != "" ? decimal.Parse(sdr["SellingPriceInKG"].ToString()) : product.SellingPriceInKG);
                                        product.SellingPricePerPiece = (sdr["SellingPricePerPiece"].ToString() != "" ? decimal.Parse(sdr["SellingPricePerPiece"].ToString()) : product.SellingPricePerPiece);
                                        product.IsInvoiceInKG = (sdr["IsInvoiceInKG"].ToString() != "" ? bool.Parse(sdr["IsInvoiceInKG"].ToString()) : product.IsInvoiceInKG);
                                        product.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : product.Type);
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
        #endregion GetProductListForBillOfMaterial

        #region GetProductListByCategoryCode
        public List<Product> GetProductListByCategoryCode(string code)
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
                        cmd.CommandText = "[AMC].[GetProductListByCategoryCode]";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar).Value = code;
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
                                        product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : product.Name);
                                        product.CostPrice = (sdr["SellingPrice"].ToString() != "" ? decimal.Parse(sdr["SellingPrice"].ToString()) : product.CostPrice);
                                        product.UnitCode = sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : product.UnitCode;
                                        product.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : product.CurrentStock);
                                        product.WeightInKG = (sdr["WeightInKG"].ToString() != "" ? decimal.Parse(sdr["WeightInKG"].ToString()) : product.WeightInKG);
                                        product.IsInvoiceInKG = sdr["IsInvoiceInKG"].ToString() != "" ? bool.Parse(sdr["IsInvoiceInKG"].ToString()) : product.IsInvoiceInKG;
                                        product.OrderDue = (sdr["OrderDue"].ToString() != "" ? decimal.Parse(sdr["OrderDue"].ToString()) : product.OrderDue);
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
        #endregion GetProductListByCategoryCode

    }
}
