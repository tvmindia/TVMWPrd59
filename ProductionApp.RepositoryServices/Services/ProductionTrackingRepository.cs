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

        #region GetProductionTrackingSearchList
        public List<ProductionTracking> GetProductionTrackingSearchList(string searchTerm)
        {
            try
            {
                List<ProductionTracking> productionTrackingSearchList = null;
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
                                productionTrackingSearchList = new List<ProductionTracking>();
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
                                    productionTrackingSearchList.Add(productionTracking);
                                }
                            }
                        }
                        con.Close();
                    }
                }
                return productionTrackingSearchList;
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
    }
}
