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
    public class BillOfMaterialRepository: IBillOfMaterialRepository
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
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(billOfMaterialAdvanceSearch.SearchTerm)?"": billOfMaterialAdvanceSearch.SearchTerm;
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
            catch(Exception ex)
            {
                throw ex;
            }
            return billOfMaterialList;
        }
        #endregion GetAllBillOfMaterial

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
    }
}
