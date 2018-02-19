﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;

namespace ProductionApp.RepositoryServices.Services
{
    public class RequisitionRepository: IRequisitionRepository
    {

        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public RequisitionRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<Requisition> GetAllRequisition(RequisitionAdvanceSearch requisitionAdvanceSearch)
        {

            List<Requisition> requisitionList = null;
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
                        cmd.CommandText = "[AMC].[GetAllRequisition]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(requisitionAdvanceSearch.SearchTerm) ? "" : requisitionAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = requisitionAdvanceSearch.DataTablePaging.Start;
                        if (requisitionAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = requisitionAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                requisitionList = new List<Requisition>();
                                while (sdr.Read())
                                {
                                    Requisition requisition = new Requisition();
                                    {
                                        requisition.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : requisition.ID);
                                        requisition.ReqNo = (sdr["ReqNo"].ToString() != "" ? sdr["ReqNo"].ToString() : requisition.ReqNo);
                                        requisition.ReqDate = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()) : requisition.ReqDate);
                                        requisition.ReqDateFormatted = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()).ToString(settings.DateFormat) : requisition.ReqDateFormatted);
                                        requisition.Title = (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : requisition.Title);
                                        requisition.ReqStatus = (sdr["ReqStatus"].ToString() != "" ? sdr["ReqStatus"].ToString() : requisition.ReqStatus);
                                        requisition.RequisitionBy = (sdr["RequisitionBy"].ToString() != "" ? sdr["RequisitionBy"].ToString() : requisition.RequisitionBy);
                                        requisition.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : requisition.TotalCount);
                                        requisition.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : requisition.FilteredCount);
                                    }
                                    requisitionList.Add(requisition);
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

            return requisitionList;
        }

        public object InsertUpdateRequisition(Requisition requisition)
        {

            SqlParameter outputStatus, outputCode = null;
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
                        cmd.CommandText = "[AMC].[InsertUpdateRequisition]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = requisition.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = requisition.ID;
                        cmd.Parameters.Add("@Title", SqlDbType.VarChar, 250).Value = requisition.Title;
                        cmd.Parameters.Add("@RequisitionBy", SqlDbType.VarChar, 250).Value = requisition.RequisitionBy;
                        cmd.Parameters.Add("@ReqStatus", SqlDbType.VarChar, 250).Value = requisition.ReqStatus;
                        cmd.Parameters.Add("@ReqDate", SqlDbType.DateTime).Value = requisition.ReqDateFormatted;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.VarChar, -1).Value = requisition.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = requisition.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = requisition.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = requisition.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = requisition.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(requisition.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        requisition.ID = Guid.Parse(outputCode.Value.ToString());
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = requisition.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Code = outputCode.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = requisition.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }

        #region GetAllrequisition For purchaseOrder
        public List<Requisition> GetAllRequisitionForPurchaseOrder(RequisitionAdvanceSearch requisitionAdvanceSearch)
        {
            List<Requisition> requisitionList = null;
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
                        cmd.CommandText = "[AMC].[GetAllRequsitionForPurchaseOrder]";
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = requisitionAdvanceSearch.DataTablePaging.Start;
                        if (requisitionAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = requisitionAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                requisitionList = new List<Requisition>();
                                while (sdr.Read())
                                {
                                    Requisition requisition = new Requisition();
                                    {
                                        requisition.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : requisition.ID);
                                        requisition.ReqNo = (sdr["ReqNo"].ToString() != "" ? sdr["ReqNo"].ToString() : requisition.ReqNo);
                                        requisition.ReqDate = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()) : requisition.ReqDate);
                                        requisition.ReqDateFormatted = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()).ToString(settings.DateFormat) : requisition.ReqDateFormatted);
                                        requisition.Title = (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : requisition.Title);
                                        requisition.ReqStatus = (sdr["ReqStatus"].ToString() != "" ? sdr["ReqStatus"].ToString() : requisition.ReqStatus);
                                        requisition.RequisitionBy = (sdr["RequisitionBy"].ToString() != "" ? sdr["RequisitionBy"].ToString() : requisition.RequisitionBy);
                                        requisition.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : requisition.TotalCount);
                                        requisition.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : requisition.FilteredCount);
                                    }
                                    requisitionList.Add(requisition);
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

            return requisitionList;
        }

        #endregion GetAllrequisition For purchaseOrder
    }
}
