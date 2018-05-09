using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;

namespace ProductionApp.RepositoryServices.Services
{
    public class MaterialReceiptRepository : IMaterialReceiptRepository
    {
        #region Constructor Injection
        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();
        public MaterialReceiptRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetAllMaterialReceipt
        public List<MaterialReceipt> GetAllMaterialReceipt(MaterialReceiptAdvanceSearch materialReceiptAdvanceSearch)
        {
            try
            {
                List<MaterialReceipt> materialReceiptList = null;
                MaterialReceipt materialReceipt = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[GetAllMaterialReceipt]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = materialReceiptAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = materialReceiptAdvanceSearch.ToDate;
                        if (materialReceiptAdvanceSearch.SupplierID == Guid.Empty)
                        {
                            cmd.Parameters.AddWithValue("@SupplierID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = materialReceiptAdvanceSearch.SupplierID;
                        }
                        if (materialReceiptAdvanceSearch.PurchaseOrderID == Guid.Empty)
                        {
                            cmd.Parameters.AddWithValue("@PurchaseOrderID", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@PurchaseOrderID", SqlDbType.UniqueIdentifier).Value = materialReceiptAdvanceSearch.PurchaseOrderID;
                        }
                        if (string.IsNullOrEmpty(materialReceiptAdvanceSearch.SearchTerm))
                        {
                            cmd.Parameters.AddWithValue("@SearchValue", DBNull.Value);
                        }
                        else
                        {
                            cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = materialReceiptAdvanceSearch.SearchTerm;
                        }
                        if (materialReceiptAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = materialReceiptAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = materialReceiptAdvanceSearch.DataTablePaging.Start;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialReceiptList = new List<MaterialReceipt>();
                                while (sdr.Read())
                                {
                                    materialReceipt = new MaterialReceipt();
                                    materialReceipt.Supplier = new Supplier();
                                    materialReceipt.Common = new Common();
                                    materialReceipt.PurchaseOrderID = (sdr["PurchaseOrderID"].ToString() != "" ? Guid.Parse(sdr["PurchaseOrderID"].ToString()) : materialReceipt.PurchaseOrderID);
                                    materialReceipt.PurchaseOrderNo = (sdr["PurchaseOrderNo"].ToString() != "" ? sdr["PurchaseOrderNo"].ToString() : materialReceipt.PurchaseOrderNo);
                                    materialReceipt.Supplier.ID = materialReceipt.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : materialReceipt.SupplierID);
                                    materialReceipt.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materialReceipt.ID);
                                    materialReceipt.ReceiptNo = (sdr["ReceiptNo"].ToString() != "" ? sdr["ReceiptNo"].ToString() : materialReceipt.ReceiptNo);
                                    materialReceipt.ReceiptDate = (sdr["ReceiptDate"].ToString() != "" ? DateTime.Parse(sdr["ReceiptDate"].ToString()) : materialReceipt.ReceiptDate);
                                    materialReceipt.ReceiptDateFormatted = (sdr["ReceiptDate"].ToString() != "" ? DateTime.Parse(sdr["ReceiptDate"].ToString()).ToString(settings.DateFormat) : materialReceipt.ReceiptDateFormatted);
                                    materialReceipt.Supplier.CompanyName = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : materialReceipt.Supplier.CompanyName);
                                    materialReceipt.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : materialReceipt.GeneralNotes);
                                    materialReceipt.Common.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : materialReceipt.Common.CreatedBy);
                                    materialReceipt.Common.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : materialReceipt.Common.UpdatedBy);
                                    materialReceipt.Common.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.DateFormat) : materialReceipt.Common.CreatedDateString);
                                    materialReceipt.Common.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()).ToString(settings.DateFormat) : materialReceipt.Common.UpdatedDateString);
                                    materialReceipt.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : materialReceipt.FilteredCount);
                                    materialReceipt.TotalCount= (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : materialReceipt.TotalCount);
                                    materialReceiptList.Add(materialReceipt);
                                }
                            }
                        }
                        con.Close();
                    }
                }
                return materialReceiptList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion GetAllMaterialReceipt    

        #region InsertUpdateMaterialReceipt
        public object InsertUpdateMaterialReceipt(MaterialReceipt materialReceipt)
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
                        cmd.CommandText = "[AMC].[InsertUpdateMaterialReceipt]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = materialReceipt.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = materialReceipt.ID;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = materialReceipt.SupplierID;
                        if(materialReceipt.PurchaseOrderID==Guid.Empty)
                            cmd.Parameters.AddWithValue("@PurchaseOrderID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@PurchaseOrderID", SqlDbType.UniqueIdentifier).Value = materialReceipt.PurchaseOrderID;
                        cmd.Parameters.Add("@PurchaseOrderNo", SqlDbType.NVarChar, 20).Value = materialReceipt.PurchaseOrderNo;
                        cmd.Parameters.Add("@ReceiptNo", SqlDbType.NVarChar, 50).Value = materialReceipt.ReceiptNo;
                        cmd.Parameters.Add("@ReceiptDate", SqlDbType.DateTime).Value = materialReceipt.ReceiptDateFormatted;
                        cmd.Parameters.Add("@ReceivedBy", SqlDbType.UniqueIdentifier).Value = materialReceipt.ReceivedBy;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar, -1).Value = materialReceipt.GeneralNotes;

                        cmd.Parameters.Add("@DetailXML", SqlDbType.VarChar, -1).Value = materialReceipt.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 50).Value = materialReceipt.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = materialReceipt.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 50).Value = materialReceipt.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = materialReceipt.Common.UpdatedDate;
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
                        throw new Exception(materialReceipt.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        return new
                        {
                            ID = IDOut.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = materialReceipt.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return new
            {
                Code = IDOut.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = materialReceipt.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateMaterialReceipt

        #region DeleteMaterialReceipt
        public object DeleteMaterialReceipt(MaterialReceipt materialReceipt)
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
                        cmd.CommandText = "[AMC].[DeleteMaterialReceipt]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = materialReceipt.ID;
                        cmd.Parameters.Add("@DeletedDate", SqlDbType.SmallDateTime).Value = materialReceipt.Common.CreatedDate;
                        cmd.Parameters.Add("@DeletedBy", SqlDbType.VarChar, 50).Value = materialReceipt.Common.CreatedBy;
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
        #endregion DeleteMaterialReceipt

        #region DeleteMaterialReceiptDetail
        public object DeleteMaterialReceiptDetail(MaterialReceipt materialReceipt)
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
                        cmd.CommandText = "[AMC].[DeleteMaterialReceiptDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = materialReceipt.ID;
                        cmd.Parameters.Add("@DeletedDate", SqlDbType.SmallDateTime).Value = materialReceipt.Common.CreatedDate;
                        cmd.Parameters.Add("@DeletedBy", SqlDbType.VarChar, 50).Value = materialReceipt.Common.CreatedBy;
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
        #endregion DeleteMaterialReceiptDetail

        #region GetMaterialReceipt
        public MaterialReceipt GetMaterialReceipt(Guid id)
        {
            MaterialReceipt materialReceipt = new MaterialReceipt();
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
                        cmd.CommandText = "[AMC].[GetMaterialReceiptByID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    materialReceipt.Supplier = new Supplier();
                                    materialReceipt.Common = new Common();
                                    materialReceipt.PurchaseOrderID = (sdr["PurchaseOrderID"].ToString() != "" ? Guid.Parse(sdr["PurchaseOrderID"].ToString()) : materialReceipt.PurchaseOrderID);
                                    materialReceipt.PurchaseOrderNo = (sdr["PurchaseOrderNo"].ToString() != "" ? sdr["PurchaseOrderNo"].ToString() : materialReceipt.PurchaseOrderNo);
                                    materialReceipt.Supplier.ID = materialReceipt.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : materialReceipt.SupplierID);
                                    materialReceipt.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materialReceipt.ID);
                                    materialReceipt.ReceiptNo = (sdr["ReceiptNo"].ToString() != "" ? sdr["ReceiptNo"].ToString() : materialReceipt.ReceiptNo);
                                    materialReceipt.ReceiptDate = (sdr["ReceiptDate"].ToString() != "" ? DateTime.Parse(sdr["ReceiptDate"].ToString()) : materialReceipt.ReceiptDate);
                                    materialReceipt.ReceiptDateFormatted = (sdr["ReceiptDate"].ToString() != "" ? DateTime.Parse(sdr["ReceiptDate"].ToString()).ToString(settings.DateFormat) : materialReceipt.ReceiptDateFormatted);
                                    materialReceipt.Supplier.CompanyName = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : materialReceipt.Supplier.CompanyName);
                                    materialReceipt.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : materialReceipt.GeneralNotes);
                                    materialReceipt.Common.CreatedBy = (sdr["CreatedBy"].ToString() != "" ? sdr["CreatedBy"].ToString() : materialReceipt.Common.CreatedBy);
                                    materialReceipt.Common.UpdatedBy = (sdr["UpdatedBy"].ToString() != "" ? sdr["UpdatedBy"].ToString() : materialReceipt.Common.UpdatedBy);
                                    materialReceipt.Common.CreatedDateString = (sdr["CreatedDate"].ToString() != "" ? DateTime.Parse(sdr["CreatedDate"].ToString()).ToString(settings.DateFormat) : materialReceipt.Common.CreatedDateString);
                                    materialReceipt.Common.UpdatedDateString = (sdr["UpdatedDate"].ToString() != "" ? DateTime.Parse(sdr["UpdatedDate"].ToString()).ToString(settings.DateFormat) : materialReceipt.Common.UpdatedDateString);
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
            return materialReceipt;
        }
        #endregion GetMaterialReceipt

        #region GetAllMaterialReceiptDetailByHeaderID
        public List<MaterialReceiptDetail> GetAllMaterialReceiptDetailByHeaderID(Guid id)
        {
            List<MaterialReceiptDetail> materialReceiptDetailList = new List<MaterialReceiptDetail>();
            MaterialReceiptDetail materialReceiptDetail = null;
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
                        cmd.CommandText = "[AMC].[GetAllMaterialReceiptDetailByHeaderID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    materialReceiptDetail = new MaterialReceiptDetail();
                                    materialReceiptDetail.Material = new Material();

                                    materialReceiptDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materialReceiptDetail.ID);
                                    materialReceiptDetail.MaterialReceiptID = (sdr["MaterialReceiptID"].ToString() != "" ? Guid.Parse(sdr["MaterialReceiptID"].ToString()) : materialReceiptDetail.MaterialReceiptID);
                                    materialReceiptDetail.MaterialID = materialReceiptDetail.Material.ID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : materialReceiptDetail.MaterialID);
                                    materialReceiptDetail.Material.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : materialReceiptDetail.Material.MaterialCode);
                                    materialReceiptDetail.Material.UnitCode= (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : materialReceiptDetail.Material.UnitCode);
                                    materialReceiptDetail.Material.WeightInKG = (sdr["WeightInKG"].ToString() != "" ? decimal.Parse(sdr["WeightInKG"].ToString()) : materialReceiptDetail.Material.WeightInKG);
                                    materialReceiptDetail.MaterialDesc= (sdr["MaterialDesc"].ToString() != "" ? sdr["MaterialDesc"].ToString() : materialReceiptDetail.MaterialDesc);
                                    materialReceiptDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : materialReceiptDetail.Qty);
                                    materialReceiptDetail.QtyInKG = (sdr["QtyInKG"].ToString() != "" ? decimal.Parse(sdr["QtyInKG"].ToString()) : materialReceiptDetail.QtyInKG);
                                    materialReceiptDetail.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : materialReceiptDetail.UnitCode);

                                    materialReceiptDetailList.Add(materialReceiptDetail);
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
            return materialReceiptDetailList;
        }
        #endregion GetAllMaterialReceiptDetailByHeaderID

        #region GetRecentMaterialReceiptSummary
        public List<MaterialReceipt> GetRecentMaterialReceiptSummary()
        {
            List<MaterialReceipt> materialReceiptList = new List<MaterialReceipt>();
            MaterialReceipt materialReceipt = null;
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
                        cmd.CommandText = "[AMC].[GetRecentMaterialReceiptSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    materialReceipt = new MaterialReceipt();
                                    materialReceipt.Supplier = new Supplier();
                                    materialReceipt.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);
                                    materialReceipt.ReceiptDateFormatted = (sdr["MRNDate"].ToString() != "" ? DateTime.Parse(sdr["MRNDate"].ToString()).ToString(settings.DateFormat) : materialReceipt.ReceiptDateFormatted);
                                    materialReceipt.ReceiptNo = (sdr["MRNNo"].ToString() != "" ? sdr["MRNNo"].ToString() : materialReceipt.ReceiptNo);
                                    materialReceipt.Supplier.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : materialReceipt.Supplier.CompanyName);
                                    materialReceipt.PurchaseOrderNo = (sdr["PurchaseOrderNo"].ToString() != "" ? sdr["PurchaseOrderNo"].ToString() : materialReceipt.PurchaseOrderNo);
                                    materialReceiptList.Add(materialReceipt);
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
            return materialReceiptList;
        }
        #endregion GetRecentMaterialReceiptSummary
    }
}
