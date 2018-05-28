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
    public class ProductionTrackingRepository: IProductionTrackingRepository
    {

        #region Constructor Injection
        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();
        public ProductionTrackingRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetAllProductionTracking
        public List<ProductionTracking> GetAllProductionTracking(ProductionTrackingAdvanceSearch productionTrackingAdvanceSearch)
        {
            List<ProductionTracking> productionTrackingList = null;
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
                        cmd.CommandText = "[AMC].[GetAllProductionTracking]";
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = productionTrackingAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = productionTrackingAdvanceSearch.ToDate;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productionTrackingAdvanceSearch.DataTablePaging.Start;
                        if (productionTrackingAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productionTrackingAdvanceSearch.DataTablePaging.Length;
                        if (productionTrackingAdvanceSearch.ProductID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@ProductID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = productionTrackingAdvanceSearch.ProductID;
                        if (productionTrackingAdvanceSearch.EmployeeID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@EmployeeID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@EmployeeID", SqlDbType.UniqueIdentifier).Value = productionTrackingAdvanceSearch.EmployeeID;
                        if (productionTrackingAdvanceSearch.StageID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@StageID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@StageID", SqlDbType.UniqueIdentifier).Value = productionTrackingAdvanceSearch.StageID;
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(productionTrackingAdvanceSearch.SearchTerm) ? "" : productionTrackingAdvanceSearch.SearchTerm;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionTrackingList = new List<ProductionTracking>();
                                while (sdr.Read())
                                {
                                    ProductionTracking productionTracking = new ProductionTracking();
                                    productionTracking.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionTracking.ID);
                                    productionTracking.EntryDate = (sdr["EntryDate"].ToString() != "" ? DateTime.Parse(sdr["EntryDate"].ToString()) : productionTracking.EntryDate);
                                    productionTracking.EntryDateFormatted = productionTracking.EntryDate.ToString(settings.DateFormat);
                                    productionTracking.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : productionTracking.Remarks);
                                    productionTracking.ProductionRefNo = (sdr["ProductionRefNo"].ToString() != "" ? sdr["ProductionRefNo"].ToString() : productionTracking.ProductionRefNo);
                                    productionTracking.AcceptedQty = (sdr["AcceptedQty"].ToString() != "" ? int.Parse(sdr["AcceptedQty"].ToString()) : productionTracking.AcceptedQty);
                                    productionTracking.AcceptedWt = (sdr["AcceptedWt"].ToString() != "" ? decimal.Parse(sdr["AcceptedWt"].ToString()) : productionTracking.AcceptedWt);
                                    productionTracking.DamagedQty = (sdr["DamagedQty"].ToString() != "" ? int.Parse(sdr["DamagedQty"].ToString()) : productionTracking.DamagedQty);
                                    productionTracking.DamagedWt = (sdr["DamagedWt"].ToString() != "" ? decimal.Parse(sdr["DamagedWt"].ToString()) : productionTracking.DamagedWt);

                                    productionTracking.Employee = new Employee();
                                    productionTracking.Employee.ID = productionTracking.ForemanID= (sdr["ForemanID"].ToString() != "" ? Guid.Parse(sdr["ForemanID"].ToString()) : productionTracking.ForemanID);
                                    productionTracking.Employee.Name = (sdr["Employee"].ToString() != "" ? sdr["Employee"].ToString() : productionTracking.Employee.Name);
                                    productionTracking.BOMComponentLineStageDetail = new BOMComponentLineStageDetail();
                                    productionTracking.LineStageDetailID = productionTracking.BOMComponentLineStageDetail.ID = (sdr["LineStageDetailID"].ToString() != "" ? Guid.Parse(sdr["LineStageDetailID"].ToString()) : productionTracking.BOMComponentLineStageDetail.ID);
                                    productionTracking.Product = new Product();
                                    productionTracking.ProductID = productionTracking.Product.ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : productionTracking.Product.ID);
                                    productionTracking.Product.Name = (sdr["Product"].ToString() != "" ? sdr["Product"].ToString() : productionTracking.Product.Description);

                                    productionTracking.Stage = new Stage();
                                    productionTracking.Stage.Description= (sdr["Stage"].ToString() != "" ? sdr["Stage"].ToString() : productionTracking.Stage.Description);
                                    productionTracking.SubComponent = new SubComponent();
                                    productionTracking.SubComponent.Description = (sdr["SubComponent"].ToString() != "" ? sdr["SubComponent"].ToString() : productionTracking.SubComponent.Description);
                                    productionTracking.Component = new Product();
                                    productionTracking.Component.Name = (sdr["Component"].ToString() != "" ? sdr["Component"].ToString() : productionTracking.Component.Description);
                                    productionTracking.OutputComponent = new Product();
                                    productionTracking.OutputComponent.Name = (sdr["Output"].ToString() != "" ? sdr["Output"].ToString() : productionTracking.OutputComponent.Description);

                                    productionTracking.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : productionTracking.FilteredCount);
                                    productionTracking.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : productionTracking.TotalCount);

                                    productionTrackingList.Add(productionTracking);
                                }
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productionTrackingList;
        }
        #endregion GetAllProductionTracking

        #region GetProductionTrackingSearchList
        public List<ProductionTracking> GetProductionTrackingSearchList(string searchTerm)
        {
            try
            {
                List<ProductionTracking> productionTrackingList = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "SELECT * FROM [AMC].[VWComponetSubCompDetailForProductionTracking] WHERE Search LIKE '%" + searchTerm + "%'";
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionTrackingList = new List<ProductionTracking>();
                                while (sdr.Read())
                                {
                                    ProductionTracking productionTracking = new ProductionTracking();
                                    productionTracking.Product = new Product();
                                    productionTracking.SubComponent = new SubComponent();
                                    productionTracking.BOMComponentLineStageDetail = new BOMComponentLineStageDetail();
                                    productionTracking.SearchDetail = (sdr["Details"].ToString() != "" ? sdr["Details"].ToString() : productionTracking.SearchDetail);
                                    productionTracking.SubComponent.Description = (sdr["SubComponent"].ToString() != "" ? sdr["SubComponent"].ToString() : productionTracking.SubComponent.Description);
                                    productionTracking.LineStageDetailID = productionTracking.BOMComponentLineStageDetail.ID = (sdr["LineStageDetailID"].ToString() != "" ? Guid.Parse(sdr["LineStageDetailID"].ToString()) : productionTracking.BOMComponentLineStageDetail.ID);
                                    productionTracking.ProductID = productionTracking.Product.ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : productionTracking.Product.ID);
                                    productionTrackingList.Add(productionTracking);
                                }
                            }
                        }
                        con.Close();
                    }
                }
                return productionTrackingList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion GetProductionTrackingSearchList    

        #region InsertUpdateProductionTracking
        public object InsertUpdateProductionTracking(ProductionTracking productionTracking)
        {
            SqlParameter outputStatus, IDOut = null;
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
                        cmd.CommandText = "[AMC].[InsertUpdateProductionTracking]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = productionTracking.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = productionTracking.ID;
                        cmd.Parameters.Add("@EntryDate", SqlDbType.DateTime).Value = productionTracking.EntryDateFormatted;
                        cmd.Parameters.Add("@ForemanID", SqlDbType.UniqueIdentifier).Value = productionTracking.ForemanID;
                        cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = productionTracking.ProductID;
                        cmd.Parameters.Add("@LineStageDetailID", SqlDbType.UniqueIdentifier).Value = productionTracking.LineStageDetailID;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, -1).Value = productionTracking.Remarks;
                        cmd.Parameters.Add("@ProductionRefNo", SqlDbType.NVarChar, 250).Value = productionTracking.ProductionRefNo;

                        cmd.Parameters.Add("@AcceptedQty", SqlDbType.Int).Value = productionTracking.AcceptedQty;
                        cmd.Parameters.Add("@DamagedQty", SqlDbType.Int).Value = productionTracking.DamagedQty;
                        cmd.Parameters.Add("@AcceptedWt", SqlDbType.Decimal).Value = productionTracking.AcceptedWt;
                        cmd.Parameters.Add("@DamagedWt", SqlDbType.Decimal).Value = productionTracking.DamagedWt;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 50).Value = productionTracking.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = productionTracking.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 50).Value = productionTracking.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = productionTracking.Common.UpdatedDate;

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        IDOut.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(productionTracking.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        return new
                        {
                            ID = IDOut.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = productionTracking.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Code = IDOut.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = productionTracking.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateProductionTracking

        #region DeleteProductionTracking
        public object DeleteProductionTracking(ProductionTracking productionTracking)
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
                        cmd.CommandText = "[AMC].[DeleteProductionTracking]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = productionTracking.ID;
                        cmd.Parameters.Add("@DeletedDate", SqlDbType.DateTime).Value = productionTracking.Common.CreatedDate;
                        cmd.Parameters.Add("@DeletedBy", SqlDbType.VarChar, 50).Value = productionTracking.Common.CreatedBy;
                        cmd.Parameters.Add("@LineStageDetailID", SqlDbType.UniqueIdentifier).Value = productionTracking.LineStageDetailID;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
            };
        }
        #endregion DeleteProductionTracking

        #region GetProductionTracking
        public ProductionTracking GetProductionTracking(Guid id)
        {
            ProductionTracking productionTracking = new ProductionTracking();
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
                        cmd.CommandText = "[AMC].[GetProductionTracking]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionTracking = new ProductionTracking();
                                if (sdr.Read())
                                {
                                    productionTracking.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionTracking.ID);
                                    productionTracking.EntryDate = (sdr["EntryDate"].ToString() != "" ? DateTime.Parse(sdr["EntryDate"].ToString()) : productionTracking.EntryDate);
                                    productionTracking.EntryDateFormatted = productionTracking.EntryDate.ToString(settings.DateFormat);
                                    productionTracking.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : productionTracking.Remarks);
                                    productionTracking.ProductionRefNo = (sdr["ProductionRefNo"].ToString() != "" ? sdr["ProductionRefNo"].ToString() : productionTracking.ProductionRefNo);
                                    productionTracking.AcceptedQty = (sdr["AcceptedQty"].ToString() != "" ? int.Parse(sdr["AcceptedQty"].ToString()) : productionTracking.AcceptedQty);
                                    productionTracking.AcceptedWt = (sdr["AcceptedWt"].ToString() != "" ? decimal.Parse(sdr["AcceptedWt"].ToString()) : productionTracking.AcceptedWt);
                                    productionTracking.DamagedQty = (sdr["DamagedQty"].ToString() != "" ? int.Parse(sdr["DamagedQty"].ToString()) : productionTracking.DamagedQty);
                                    productionTracking.DamagedWt = (sdr["DamagedWt"].ToString() != "" ? decimal.Parse(sdr["DamagedWt"].ToString()) : productionTracking.DamagedWt);

                                    productionTracking.Employee = new Employee();
                                    productionTracking.Employee.ID = productionTracking.ForemanID = (sdr["ForemanID"].ToString() != "" ? Guid.Parse(sdr["ForemanID"].ToString()) : productionTracking.ForemanID);
                                    productionTracking.Employee.Name = (sdr["Employee"].ToString() != "" ? sdr["Employee"].ToString() : productionTracking.Employee.Name);
                                    productionTracking.BOMComponentLineStageDetail = new BOMComponentLineStageDetail();
                                    productionTracking.LineStageDetailID = productionTracking.BOMComponentLineStageDetail.ID = (sdr["LineStageDetailID"].ToString() != "" ? Guid.Parse(sdr["LineStageDetailID"].ToString()) : productionTracking.BOMComponentLineStageDetail.ID);
                                    productionTracking.Product = new Product();
                                    productionTracking.ProductID = productionTracking.Product.ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : productionTracking.Product.ID);
                                    productionTracking.Product.Name = (sdr["Product"].ToString() != "" ? sdr["Product"].ToString() : productionTracking.Product.Description);

                                    productionTracking.Stage = new Stage();
                                    productionTracking.Stage.Description = (sdr["Stage"].ToString() != "" ? sdr["Stage"].ToString() : productionTracking.Stage.Description);
                                    productionTracking.SubComponent = new SubComponent();
                                    productionTracking.SubComponent.Description = (sdr["SubComponent"].ToString() != "" ? sdr["SubComponent"].ToString() : productionTracking.SubComponent.Description);
                                    productionTracking.Component = new Product();
                                    productionTracking.Component.Name = (sdr["Component"].ToString() != "" ? sdr["Component"].ToString() : productionTracking.Component.Description);
                                    productionTracking.OutputComponent = new Product();
                                    productionTracking.OutputComponent.Name = (sdr["Output"].ToString() != "" ? sdr["Output"].ToString() : productionTracking.OutputComponent.Description);

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
            return productionTracking;
        }
        #endregion GetBOMComponentLineStage

        #region GetRecentProductionTracking
        public List<ProductionTracking> GetRecentProductionTracking()
        {
            List<ProductionTracking> productionTrackingList = new List<ProductionTracking>();
            ProductionTracking productionTracking = null;
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
                        cmd.CommandText = "[AMC].[GetRecentProductionTracking]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    productionTracking = new ProductionTracking();
                                    productionTracking.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);                                    
                                    productionTracking.EntryDateFormatted = (sdr["EntryDate"].ToString() != "" ? DateTime.Parse(sdr["EntryDate"].ToString()).ToString(settings.DateFormat) : productionTracking.EntryDateFormatted);
                                    productionTracking.Product = new Product();
                                    productionTracking.Product.Name = (sdr["Product"].ToString() != "" ? sdr["Product"].ToString() : productionTracking.Product.Name);
                                    productionTracking.Stage = new Stage();
                                    productionTracking.Stage.Description = (sdr["Stage"].ToString() != "" ? sdr["Stage"].ToString() : productionTracking.Stage.Description);
                                    productionTracking.Employee = new Employee();
                                    productionTracking.Employee.Name = (sdr["EmployeeName"].ToString() != "" ? sdr["EmployeeName"].ToString() :productionTracking.Employee.Name);
                                    productionTracking.AcceptedQty = (sdr["AcceptedQty"].ToString() != "" ? int.Parse(sdr["AcceptedQty"].ToString()) : productionTracking.AcceptedQty);
                                    productionTracking.DamagedQty = (sdr["DamagedQty"].ToString() != "" ? int.Parse(sdr["DamagedQty"].ToString()) : productionTracking.DamagedQty);
                                    productionTracking.SubComponent = new SubComponent();
                                    productionTracking.SubComponent.Description = (sdr["SubComponent"].ToString() != "" ? sdr["SubComponent"].ToString() : productionTracking.SubComponent.Description);
                                    productionTracking.Component = new Product();
                                    productionTracking.Component.Name = (sdr["Component"].ToString() != "" ? sdr["Component"].ToString() : productionTracking.Component.Description);
                                    productionTracking.OutputComponent = new Product();
                                    productionTracking.OutputComponent.Name = (sdr["Output"].ToString() != "" ? sdr["Output"].ToString() : productionTracking.OutputComponent.Description);
                                    productionTrackingList.Add(productionTracking);
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
            return productionTrackingList;
        }
        #endregion GetRecentProductionTracking

        #region GetPendingProductionTracking
        public List<ProductionTracking> GetPendingProductionTracking(string postDate)
        {
            List<ProductionTracking> productionTrackingList = null;
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
                        cmd.CommandText = "[AMC].[GetPendingProductionTracking]";
                        cmd.Parameters.Add("@PostDate", SqlDbType.DateTime).Value = postDate;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionTrackingList = new List<ProductionTracking>();
                                while (sdr.Read())
                                {
                                    ProductionTracking productionTracking = new ProductionTracking();
                                    productionTracking.AcceptedQty = (sdr["AcceptedQty"].ToString() != "" ? int.Parse(sdr["AcceptedQty"].ToString()) : productionTracking.AcceptedQty);
                                    productionTracking.DamagedQty = (sdr["DamagedQty"].ToString() != "" ? int.Parse(sdr["DamagedQty"].ToString()) : productionTracking.DamagedQty);

                                    productionTracking.SlNo = (sdr["RowNum"].ToString() != "" ? int.Parse(sdr["RowNum"].ToString()) : productionTracking.SlNo);
                                    productionTracking.PreviousQty = (sdr["Qty"].ToString() != "" ? int.Parse(sdr["Qty"].ToString()) : productionTracking.PreviousQty);
                                    productionTracking.TotalQty = (sdr["TotalQty"].ToString() != "" ? int.Parse(sdr["TotalQty"].ToString()) : productionTracking.TotalQty);
                                    productionTracking.IsValid = (sdr["isValid"].ToString() != "" ? bool.Parse(sdr["isValid"].ToString()) : productionTracking.IsValid);

                                    productionTracking.LineStageDetailID = (sdr["LineStageDetailID"].ToString() != "" ? Guid.Parse(sdr["LineStageDetailID"].ToString()) : productionTracking.LineStageDetailID);
                                    productionTracking.Product = new Product();
                                    productionTracking.ProductID = productionTracking.Product.ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : productionTracking.Product.ID);
                                    productionTracking.Product.Name = (sdr["Product"].ToString() != "" ? sdr["Product"].ToString() : productionTracking.Product.Description);

                                    productionTracking.SubComponent = new SubComponent();
                                    productionTracking.SubComponent.Description = (sdr["SubComponent"].ToString() != "" ? sdr["SubComponent"].ToString() : productionTracking.SubComponent.Description);
                                    productionTracking.OutputComponent = new Product();
                                    productionTracking.OutputComponent.Name = (sdr["Output"].ToString() != "" ? sdr["Output"].ToString() : productionTracking.OutputComponent.Description);

                                    productionTrackingList.Add(productionTracking);
                                }
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productionTrackingList;
        }
        #endregion GetPendingProductionTracking

        #region GetPendingProductionTrackingDetail
        public List<ProductionTracking> GetPendingProductionTrackingDetail(ProductionTrackingAdvanceSearch productionTrackingAdvanceSearch)
        {
            List<ProductionTracking> productionTrackingList = null;
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
                        cmd.CommandText = "[AMC].[GetPendingProductionTrackingDetail]";
                        cmd.Parameters.Add("@PostDate", SqlDbType.DateTime).Value = productionTrackingAdvanceSearch.PostDate;
                        if (productionTrackingAdvanceSearch.LineStageDetailID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@LineStageDetailID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@LineStageDetailID", SqlDbType.UniqueIdentifier).Value = productionTrackingAdvanceSearch.LineStageDetailID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionTrackingList = new List<ProductionTracking>();
                                while (sdr.Read())
                                {
                                    ProductionTracking productionTracking = new ProductionTracking();
                                    productionTracking.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : productionTracking.ID);
                                    productionTracking.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : productionTracking.Remarks);
                                    productionTracking.ProductionRefNo = (sdr["ProductionRefNo"].ToString() != "" ? sdr["ProductionRefNo"].ToString() : productionTracking.ProductionRefNo);
                                    productionTracking.AcceptedQty = (sdr["AcceptedQty"].ToString() != "" ? int.Parse(sdr["AcceptedQty"].ToString()) : productionTracking.AcceptedQty);
                                    productionTracking.AcceptedWt = (sdr["AcceptedWt"].ToString() != "" ? decimal.Parse(sdr["AcceptedWt"].ToString()) : productionTracking.AcceptedWt);
                                    productionTracking.DamagedQty = (sdr["DamagedQty"].ToString() != "" ? int.Parse(sdr["DamagedQty"].ToString()) : productionTracking.DamagedQty);
                                    productionTracking.DamagedWt = (sdr["DamagedWt"].ToString() != "" ? decimal.Parse(sdr["DamagedWt"].ToString()) : productionTracking.DamagedWt);
                                    productionTracking.LineStageDetailID = (sdr["LineStageDetailID"].ToString() != "" ? Guid.Parse(sdr["LineStageDetailID"].ToString()) : productionTracking.LineStageDetailID);
                                    productionTracking.EntryDateFormatted= (sdr["EntryDate"].ToString() != "" ? (DateTime.Parse(sdr["EntryDate"].ToString()).ToString(settings.DateFormat)) : productionTracking.EntryDateFormatted);
                                    productionTracking.Employee = new Employee();
                                    productionTracking.Employee.ID = productionTracking.ForemanID = (sdr["ForemanID"].ToString() != "" ? Guid.Parse(sdr["ForemanID"].ToString()) : productionTracking.ForemanID);
                                    productionTracking.Employee.Name = (sdr["Employee"].ToString() != "" ? sdr["Employee"].ToString() : productionTracking.Employee.Name);

                                    productionTrackingList.Add(productionTracking);
                                }
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productionTrackingList;
        }
        #endregion GetPendingProductionTrackingDetail

        #region GetAllAvailableProductionTrackingEntryDate()
        public List<string> GetAllAvailableProductionTrackingEntryDate()
        {
            List<string> dateList = null;
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
                        cmd.CommandText = "[AMC].[GetAllAvailableProductionTrackingEntryDate]";
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                dateList = new List<string>();
                                while (sdr.Read())
                                {
                                    string entryDate = (sdr["EntryDate"].ToString() != "" ? (DateTime.Parse(sdr["EntryDate"].ToString()).Date.ToString("dd-MM-yyyy")) : "");
                                    dateList.Add(entryDate);
                                }
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dateList;
        }
        #endregion GetAllAvailableProductionTrackingEntryDate()

        #region UpdateProductionTrackingByXML
        public object UpdateProductionTrackingByXML(Common common,string productionTrackingXML)
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
                        cmd.CommandText = "[AMC].[UpdateProductionTrackingByXML]";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 50).Value = common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = common.UpdatedDate;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = productionTrackingXML;

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.UpdateFailure);
                    case "1":
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.UpdateSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConst.UpdateSuccess
            };
        }
        #endregion UpdateProductionTrackingByXML

        #region ProductionTrackingPosting
        public object ProductionTrackingPosting(Common common, string postDate)
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
                        cmd.CommandText = "[AMC].[ProductionTrackingPosting]";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 50).Value = common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = common.UpdatedDate;
                        cmd.Parameters.Add("@PostDate", SqlDbType.SmallDateTime).Value = postDate;

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;

                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.UpdateFailure);
                    case "1":
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.UpdateSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConst.UpdateSuccess
            };
        }
        #endregion

    }
}
