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
    public class BillOfMaterialRepository : IBillOfMaterialRepository
    {
        #region Constructor Injection
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        public BillOfMaterialRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetAllBillOfMaterial
        public List<BillOfMaterial> GetAllBillOfMaterial(BillOfMaterialAdvanceSearch billOfMaterialAdvanceSearch)
        {
            List<BillOfMaterial> billOfMaterialList = null;
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
                        cmd.CommandText = "[AMC].[GetAllBillOfMaterial]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(billOfMaterialAdvanceSearch.SearchTerm) ? "" : billOfMaterialAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = billOfMaterialAdvanceSearch.DataTablePaging.Start;
                        if (billOfMaterialAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = billOfMaterialAdvanceSearch.DataTablePaging.Length;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                billOfMaterialList = new List<BillOfMaterial>();
                                while (sdr.Read())
                                {
                                    BillOfMaterial billOfMaterial = new BillOfMaterial();
                                    {
                                        billOfMaterial.Product = new Product();
                                        billOfMaterial.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : billOfMaterial.ID);
                                        billOfMaterial.Product.ID = billOfMaterial.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : billOfMaterial.ProductID);
                                        billOfMaterial.Product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : billOfMaterial.Product.Name);
                                        billOfMaterial.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : billOfMaterial.Description);
                                        billOfMaterial.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : billOfMaterial.TotalCount);
                                        billOfMaterial.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : billOfMaterial.FilteredCount);
                                    }
                                    billOfMaterialList.Add(billOfMaterial);
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
            return billOfMaterialList;
        }
        #endregion GetAllBillOfMaterial

        #region CheckBillOfMaterialExist
        public bool CheckBillOfMaterialExist(Guid productID)
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
                        cmd.CommandText = "[AMC].[CheckBillOfMaterialExist]";
                        cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = productID;
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
        #endregion CheckBillOfMaterialExist

        #region InsertUpdateBillOfMaterial
        public object InsertUpdateBillOfMaterial(BillOfMaterial billOfMaterial)
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
                        cmd.CommandText = "[AMC].[InsertUpdateBillOfMaterial]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = billOfMaterial.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = billOfMaterial.ID;
                        cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = billOfMaterial.ProductID;
                        cmd.Parameters.Add("@Description", SqlDbType.VarChar, -1).Value = billOfMaterial.Description;

                        cmd.Parameters.Add("@DetailXML", SqlDbType.VarChar, -1).Value = billOfMaterial.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 50).Value = billOfMaterial.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = billOfMaterial.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 50).Value = billOfMaterial.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = billOfMaterial.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(billOfMaterial.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        return new
                        {
                            ID = IDOut.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = billOfMaterial.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = billOfMaterial.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateBillOfMaterial

        #region GetBillOfMaterial
        public BillOfMaterial GetBillOfMaterial(Guid id)
        {
            BillOfMaterial billOfMaterial = null;
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
                        cmd.CommandText = "[AMC].[GetBillOfMaterial]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    billOfMaterial = new BillOfMaterial();
                                    {
                                        billOfMaterial.Product = new Product();
                                        billOfMaterial.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : billOfMaterial.ID);
                                        billOfMaterial.Product.ID = billOfMaterial.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : billOfMaterial.ProductID);
                                        billOfMaterial.Product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : billOfMaterial.Product.Name);
                                        billOfMaterial.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : billOfMaterial.Description);
                                    }
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
            return billOfMaterial;
        }
        #endregion GetBillOfMaterial

        #region GetBillOfMaterialDetail
        public List<BillOfMaterialDetail> GetBillOfMaterialDetail(Guid id)
        {
            List<BillOfMaterialDetail> billOfMaterialDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetBillOfMaterialDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                billOfMaterialDetailList = new List<BillOfMaterialDetail>();
                                while (sdr.Read())
                                {
                                    BillOfMaterialDetail billOfMaterialDetail = new BillOfMaterialDetail();
                                    {
                                        billOfMaterialDetail.Product = new Product();
                                        billOfMaterialDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : billOfMaterialDetail.ID);
                                        billOfMaterialDetail.Product.ID = billOfMaterialDetail.ComponentID = (sdr["ComponentID"].ToString() != "" ? Guid.Parse(sdr["ComponentID"].ToString()) : billOfMaterialDetail.ComponentID);
                                        billOfMaterialDetail.Product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : billOfMaterialDetail.Product.Name);
                                        billOfMaterialDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : billOfMaterialDetail.Qty);
                                        billOfMaterialDetail.Product.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : billOfMaterialDetail.Product.UnitCode);
                                    }
                                    billOfMaterialDetailList.Add(billOfMaterialDetail);
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
            return billOfMaterialDetailList;
        }
        #endregion GetBillOfMaterialDetail

        #region DeleteBillOfMaterial
        public object DeleteBillOfMaterial(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteBillOfMaterial]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
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
        #endregion DeleteBillOfMaterial

        #region DeleteBillOfMaterialDetail
        public object DeleteBillOfMaterialDetail(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteBillOfMaterialDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
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
        #endregion DeleteBillOfMaterialDetail


        #region CheckLineNameExist
        public bool CheckLineNameExist(string lineName)
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
                        cmd.CommandText = "[AMC].[CheckLineNameExist]";
                        cmd.Parameters.Add("@LineName", SqlDbType.NVarChar,250).Value = lineName;
                        //cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = bOMComponentLine.ID;
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
        #endregion CheckLineNameExist


        #region InsertUpdateBOMComponentLine
        public object InsertUpdateBOMComponentLine(BOMComponentLine bOMComponentLine)
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
                        cmd.CommandText = "[AMC].[InsertUpdateBOMComponentLine]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = bOMComponentLine.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = bOMComponentLine.ID;
                        cmd.Parameters.Add("@ComponentID", SqlDbType.UniqueIdentifier).Value = bOMComponentLine.ComponentID;
                        cmd.Parameters.Add("@LineName", SqlDbType.VarChar, -1).Value = bOMComponentLine.LineName;

                        cmd.Parameters.Add("@StageXML", SqlDbType.VarChar, -1).Value = bOMComponentLine.StageXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 50).Value = bOMComponentLine.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = bOMComponentLine.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 50).Value = bOMComponentLine.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = bOMComponentLine.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(bOMComponentLine.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        return new
                        {
                            ID = IDOut.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = bOMComponentLine.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = bOMComponentLine.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateBOMComponentLine

        #region DeleteBOMComponentLine
        public object DeleteBOMComponentLine(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteBOMComponentLine]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
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
        #endregion DeleteBOMComponentLine

        #region GetBOMComponentLineByComponentID
        public List<BOMComponentLine> GetBOMComponentLineByComponentID(Guid componentID)
        {
            List<BOMComponentLine> bOMComponentLineList = null;
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
                        cmd.CommandText = "[AMC].[GetBOMComponentLineByComponentID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ComponentID", SqlDbType.UniqueIdentifier).Value = componentID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                bOMComponentLineList = new List<BOMComponentLine>();
                                while (sdr.Read())
                                {
                                    BOMComponentLine bOMComponentLine = new BOMComponentLine();
                                    {
                                        bOMComponentLine.Product = new Product();
                                        bOMComponentLine.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : bOMComponentLine.ID);
                                        bOMComponentLine.Product.ID = bOMComponentLine.ComponentID = (sdr["ComponentID"].ToString() != "" ? Guid.Parse(sdr["ComponentID"].ToString()) : bOMComponentLine.ComponentID);
                                        //bOMComponentLine.Product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : bOMComponentLine.Product.Name);
                                        bOMComponentLine.LineName = (sdr["LineName"].ToString() != "" ? sdr["LineName"].ToString() : bOMComponentLine.LineName);
                                    }
                                    bOMComponentLineList.Add(bOMComponentLine);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex) {
                throw ex;
            }
            return bOMComponentLineList;
        }
        #endregion GetBOMComponentLineByComponentID

        #region GetBOMComponentLineStage
        public List<BOMComponentLineStage> GetBOMComponentLineStage(Guid id)
        {
            List<BOMComponentLineStage> bOMComponentLineStageList = null;
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
                        cmd.CommandText = "[AMC].[GetBOMComponentLineStage]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ComponentLineID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                bOMComponentLineStageList = new List<BOMComponentLineStage>();
                                while (sdr.Read())
                                {
                                    BOMComponentLineStage bOMComponentLineStage = new BOMComponentLineStage();
                                    {
                                        bOMComponentLineStage.Stage = new Stage();
                                        bOMComponentLineStage.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : bOMComponentLineStage.ID);
                                        bOMComponentLineStage.ComponentLineID = (sdr["ComponentLineID"].ToString() != "" ? Guid.Parse(sdr["ComponentLineID"].ToString()) : bOMComponentLineStage.ComponentLineID);
                                        bOMComponentLineStage.Stage.ID = bOMComponentLineStage.StageID = (sdr["StageID"].ToString() != "" ? Guid.Parse(sdr["StageID"].ToString()) : bOMComponentLineStage.StageID);
                                        bOMComponentLineStage.Stage.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : bOMComponentLineStage.Stage.Description);
                                        bOMComponentLineStage.StageOrder = (sdr["StageOrder"].ToString() != "" ? int.Parse(sdr["StageOrder"].ToString()) : bOMComponentLineStage.StageOrder);
                                    }
                                    bOMComponentLineStageList.Add(bOMComponentLineStage);
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
            return bOMComponentLineStageList;
        }
        #endregion GetBOMComponentLineStage

        #region InsertUpdateBOMComponentLineStageDetail
        public object InsertUpdateBOMComponentLineStageDetail(BOMComponentLineStageDetail bOMComponentLineStageDetail)
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
                        cmd.CommandText = "[AMC].[InsertUpdateBOMComponentLineStageDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = bOMComponentLineStageDetail.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = bOMComponentLineStageDetail.ID;
                        cmd.Parameters.Add("@ComponentLineID", SqlDbType.UniqueIdentifier).Value = bOMComponentLineStageDetail.ComponentLineID;
                        cmd.Parameters.Add("@EntryType", SqlDbType.VarChar, 10).Value = bOMComponentLineStageDetail.EntryType;
                        cmd.Parameters.Add("@StageID", SqlDbType.UniqueIdentifier).Value = bOMComponentLineStageDetail.StageID;
                        cmd.Parameters.Add("@PartType", SqlDbType.VarChar, 10).Value = bOMComponentLineStageDetail.PartType;
                        cmd.Parameters.Add("@PartID", SqlDbType.UniqueIdentifier).Value = bOMComponentLineStageDetail.PartID;
                        cmd.Parameters.Add("@Qty", SqlDbType.Decimal).Value = bOMComponentLineStageDetail.Qty;


                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 50).Value = bOMComponentLineStageDetail.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = bOMComponentLineStageDetail.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 50).Value = bOMComponentLineStageDetail.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = bOMComponentLineStageDetail.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(bOMComponentLineStageDetail.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        return new
                        {
                            ID = IDOut.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = bOMComponentLineStageDetail.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                ID = IDOut.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = bOMComponentLineStageDetail.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateBOMComponentLineStageDetail

        #region DeleteBOMComponentLineStageDetail
        public object DeleteBOMComponentLineStageDetail(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteBOMComponentLineStageDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
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
                Status = outputStatus.Value.ToString()
            };
        }
        #endregion DeleteBOMComponentLineStageDetail

        #region GetBOMComponentLineStageDetail
        public List<BOMComponentLineStageDetail> GetBOMComponentLineStageDetail(Guid id)
        {
            List<BOMComponentLineStageDetail> bOMComponentLineStageDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetBOMComponentLineStageDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ComponentLineID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                bOMComponentLineStageDetailList = new List<BOMComponentLineStageDetail>();
                                while (sdr.Read())
                                {
                                    BOMComponentLineStageDetail bOMComponentLineStageDetail = new BOMComponentLineStageDetail();
                                    {
                                        bOMComponentLineStageDetail.Stage = new Stage();
                                        bOMComponentLineStageDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : bOMComponentLineStageDetail.ID);
                                        bOMComponentLineStageDetail.ComponentLineID = (sdr["ComponentLineID"].ToString() != "" ? Guid.Parse(sdr["ComponentLineID"].ToString()) : bOMComponentLineStageDetail.ComponentLineID);
                                        bOMComponentLineStageDetail.Stage.ID = bOMComponentLineStageDetail.StageID = (sdr["StageID"].ToString() != "" ? Guid.Parse(sdr["StageID"].ToString()) : bOMComponentLineStageDetail.StageID);
                                        bOMComponentLineStageDetail.Stage.Description = (sdr["StageName"].ToString() != "" ? sdr["StageName"].ToString() : bOMComponentLineStageDetail.Stage.Description);
                                        bOMComponentLineStageDetail.EntryType = (sdr["EntryType"].ToString() != "" ? sdr["EntryType"].ToString() : bOMComponentLineStageDetail.EntryType);
                                        bOMComponentLineStageDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : bOMComponentLineStageDetail.Qty);
                                        bOMComponentLineStageDetail.PartType = (sdr["PartType"].ToString() != "" ? sdr["PartType"].ToString() : bOMComponentLineStageDetail.PartType);
                                        bOMComponentLineStageDetail.PartID = (sdr["PartID"].ToString() != "" ? Guid.Parse(sdr["PartID"].ToString()) : bOMComponentLineStageDetail.PartID);
                                        switch (bOMComponentLineStageDetail.PartType)
                                        {
                                            case "RAW":
                                                bOMComponentLineStageDetail.Material = new Material();
                                                bOMComponentLineStageDetail.Material.ID = bOMComponentLineStageDetail.PartID;
                                                bOMComponentLineStageDetail.Material.Description= (sdr["ItemName"].ToString() != "" ? sdr["ItemName"].ToString() : bOMComponentLineStageDetail.Material.Description);
                                                break;
                                            case "SUB":
                                                bOMComponentLineStageDetail.SubComponent = new SubComponent();
                                                bOMComponentLineStageDetail.SubComponent.ID = bOMComponentLineStageDetail.PartID;
                                                bOMComponentLineStageDetail.SubComponent.Description= (sdr["ItemName"].ToString() != "" ? sdr["ItemName"].ToString() : bOMComponentLineStageDetail.SubComponent.Description);
                                                break;
                                            case "COM":
                                                bOMComponentLineStageDetail.Product = new Product();
                                                bOMComponentLineStageDetail.Product.ID = bOMComponentLineStageDetail.PartID;
                                                bOMComponentLineStageDetail.Product.Name= (sdr["ItemName"].ToString() != "" ? sdr["ItemName"].ToString() : bOMComponentLineStageDetail.Product.Name);
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    bOMComponentLineStageDetailList.Add(bOMComponentLineStageDetail);
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
            return bOMComponentLineStageDetailList;
        }
    }
    #endregion GetBOMComponentLineStageDetail

}
