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
    public class MaterialReturnRepository: IMaterialReturnRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public MaterialReturnRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllReturnToSupplier
        public List<MaterialReturn> GetAllReturnToSupplier(MaterialReturnAdvanceSearch materialReturnAdvanceSearch)
        {
            List<MaterialReturn> materialReturnList = null;
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
                        cmd.CommandText = "[AMC].[GetAllReturnToSupplier]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(materialReturnAdvanceSearch.SearchTerm) ? "" : materialReturnAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = materialReturnAdvanceSearch.DataTablePaging.Start;
                        if (materialReturnAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = materialReturnAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = materialReturnAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = materialReturnAdvanceSearch.ToDate;
                        if (materialReturnAdvanceSearch.ReturnBy != Guid.Empty)
                            cmd.Parameters.Add("@ReturnBy", SqlDbType.UniqueIdentifier).Value = materialReturnAdvanceSearch.ReturnBy;
                        if (materialReturnAdvanceSearch.SupplierID != Guid.Empty)
                            cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = materialReturnAdvanceSearch.SupplierID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialReturnList = new List<MaterialReturn>();
                                while (sdr.Read())
                                {
                                    MaterialReturn materiaReturn = new MaterialReturn();
                                    {
                                        materiaReturn.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materiaReturn.ID);
                                        materiaReturn.ReturnSlipNo = (sdr["ReturnSlipNo"].ToString() != "" ? sdr["ReturnSlipNo"].ToString() : materiaReturn.ReturnSlipNo);
                                        materiaReturn.ReturnDate = (sdr["ReturnDate"].ToString() != "" ? DateTime.Parse(sdr["ReturnDate"].ToString()) : materiaReturn.ReturnDate);
                                        materiaReturn.ReturnDateFormatted = (sdr["ReturnDate"].ToString() != "" ? DateTime.Parse(sdr["ReturnDate"].ToString()).ToString(settings.DateFormat) : materiaReturn.ReturnDateFormatted);
                                        materiaReturn.ReturnByEmployeeName = (sdr["ReturnByEmployeeName"].ToString() != "" ? sdr["ReturnByEmployeeName"].ToString() : materiaReturn.ReturnByEmployeeName);
                                        materiaReturn.SupplierName = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : materiaReturn.SupplierName);
                                        materiaReturn.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : materiaReturn.TotalCount);
                                        materiaReturn.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : materiaReturn.FilteredCount);
                                    }
                                    materialReturnList.Add(materiaReturn);
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
            return materialReturnList;
        }

        #endregion GetAllReturnToSupplier

        #region InsertUpdateMaterialReturn
        public object InsertUpdateMaterialReturn(MaterialReturn materialReturn)
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
                        cmd.CommandText = "[AMC].[InsertUpdateMaterialReturn]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = materialReturn.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = materialReturn.ID;
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = materialReturn.SupplierID;
                        cmd.Parameters.Add("@ReturnBy", SqlDbType.UniqueIdentifier).Value = materialReturn.ReturnBy;
                        cmd.Parameters.Add("@ReturnDate", SqlDbType.DateTime).Value = materialReturn.ReturnDateFormatted;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar).Value = materialReturn.GeneralNotes;
                        cmd.Parameters.Add("@BillAddress", SqlDbType.VarChar).Value = materialReturn.BillAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.VarChar).Value = materialReturn.ShippingAddress;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = materialReturn.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = materialReturn.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = materialReturn.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = materialReturn.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = materialReturn.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier, 5);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    switch (outputStatus.Value.ToString())
                    {
                        case "0":
                            throw new Exception(materialReturn.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                        case "1":
                            return new
                            {
                                ID = IDOut.Value.ToString(),
                                Status = outputStatus.Value.ToString(),
                                Message = materialReturn.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                            };
                    }
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
                Message = materialReturn.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateMaterialReturn

        #region GetMaterialReturn
        public MaterialReturn GetMaterialReturn(Guid id)
        {
            MaterialReturn material = new MaterialReturn();
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
                        cmd.CommandText = "[AMC].[GetMaterialReturn]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    material.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : material.ID);
                                    material.ReturnSlipNo = (sdr["ReturnSlipNo"].ToString() != "" ? sdr["ReturnSlipNo"].ToString() : material.ReturnSlipNo);
                                    material.ReturnDate = (sdr["ReturnDate"].ToString() != "" ? DateTime.Parse(sdr["ReturnDate"].ToString()) : material.ReturnDate);
                                    material.ReturnDateFormatted = (sdr["ReturnDate"].ToString() != "" ? DateTime.Parse(sdr["ReturnDate"].ToString()).ToString(settings.DateFormat) : material.ReturnDateFormatted);
                                    material.ReturnBy = (sdr["ReturnBy"].ToString() != "" ? Guid.Parse(sdr["ReturnBy"].ToString()) : material.ReturnBy);
                                    material.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : material.SupplierID);
                                    material.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : material.GeneralNotes);
                                    material.BillAddress = (sdr["BillAddress"].ToString() != "" ? sdr["BillAddress"].ToString() : material.BillAddress);
                                    material.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : material.ShippingAddress);
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
            return material;
        }
        #endregion GetMaterialReturn

        #region GetMaterialReturnDetail
        public List<MaterialReturnDetail> GetMaterialReturnDetail(Guid id)
        {
            List<MaterialReturnDetail> materialReturnDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetMaterialReturnDetail]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialReturnDetailList = new List<MaterialReturnDetail>();
                                while (sdr.Read())
                                {
                                    MaterialReturnDetail materialReturn = new MaterialReturnDetail();
                                    {
                                        materialReturn.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materialReturn.ID);
                                        materialReturn.MaterialReturnID = (sdr["MaterialReturnID"].ToString() != "" ? Guid.Parse(sdr["MaterialReturnID"].ToString()) : materialReturn.MaterialReturnID);
                                        materialReturn.MaterialID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : materialReturn.MaterialID);
                                        materialReturn.MaterialDesc = (sdr["MaterialDesc"].ToString() != "" ? sdr["MaterialDesc"].ToString() : materialReturn.MaterialDesc);
                                        materialReturn.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : materialReturn.ToString());
                                        materialReturn.Qty = (sdr["Qty"].ToString() != "" ? Decimal.Parse(sdr["Qty"].ToString()) : materialReturn.Qty);
                                        materialReturn.Rate = (sdr["Rate"].ToString() != "" ? Decimal.Parse(sdr["Rate"].ToString()) : materialReturn.Rate);
                                        materialReturn.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : materialReturn.TaxTypeCode);
                                        materialReturn.TaxTypeDescription = (sdr["TaxTypeDescription"].ToString() != "" ? sdr["TaxTypeDescription"].ToString() : materialReturn.TaxTypeDescription);
                                        materialReturn.CGSTPerc = (sdr["CGSTPerc"].ToString() != "" ? Decimal.Parse(sdr["CGSTPerc"].ToString()) : materialReturn.CGSTPerc);
                                        materialReturn.SGSTPerc = (sdr["SGSTPerc"].ToString() != "" ? Decimal.Parse(sdr["SGSTPerc"].ToString()) : materialReturn.SGSTPerc);
                                        materialReturn.IGSTPerc = (sdr["IGSTPerc"].ToString() != "" ? Decimal.Parse(sdr["IGSTPerc"].ToString()) : materialReturn.IGSTPerc);
                                        materialReturn.Material = new Material();
                                        materialReturn.Material.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : materialReturn.Material.MaterialCode);
                                    }
                                    materialReturnDetailList.Add(materialReturn);
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
            return materialReturnDetailList;
        }
        #endregion

        #region DeleteMaterialReturnDetail
        public object DeleteMaterialReturnDetail(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteMaterialReturnDetail]";
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
        #endregion

        #region DeleteReturnToSupplier
        public object DeleteMaterialReturn(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteMaterialReturn]";
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
        #endregion

    }
}
