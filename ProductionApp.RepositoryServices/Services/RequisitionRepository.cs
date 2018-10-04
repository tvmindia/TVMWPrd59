using System;
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
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value =requisitionAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = requisitionAdvanceSearch.ToDate;
                        cmd.Parameters.Add("@ReqStatus", SqlDbType.VarChar).Value = requisitionAdvanceSearch.ReqStatus;
                        if (requisitionAdvanceSearch.EmployeeID != Guid.Empty)
                            cmd.Parameters.Add("@RequisitionBy", SqlDbType.UniqueIdentifier).Value = requisitionAdvanceSearch.EmployeeID;

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
                                        requisition.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : requisition.ApprovalStatus);
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
                        cmd.CommandText = "[AMC].[InsertUpdateRequisition]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = requisition.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = requisition.ID;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = requisition.hdnFileID;
                        cmd.Parameters.Add("@Title", SqlDbType.VarChar, 250).Value = requisition.Title;
                        if(requisition.EmployeeID != Guid.Empty)
                        cmd.Parameters.Add("@RequisitionBy", SqlDbType.UniqueIdentifier).Value = requisition.EmployeeID;
                        cmd.Parameters.Add("@ReqStatus", SqlDbType.VarChar, 250).Value = requisition.ReqStatus;
                        cmd.Parameters.Add("@ReqDate", SqlDbType.DateTime).Value = requisition.ReqDateFormatted;
                        cmd.Parameters.Add("@RequiredDate", SqlDbType.DateTime).Value = requisition.RequiredDateFormatted;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.VarChar, -1).Value = requisition.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = requisition.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = requisition.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = requisition.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = requisition.Common.UpdatedDate;
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
                        throw new Exception(requisition.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                      //  requisition.ID = Guid.Parse(IDOut.Value.ToString());
                        return new
                        {
                            ID = IDOut.Value.ToString(),
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
                Code = IDOut.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = requisition.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }

        #region GetAllrequisition For purchaseOrder
        public List<Requisition> GetAllRequisitionForPurchaseOrder()
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
                                        requisition.RequisitionBy = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : requisition.RequisitionBy);
                                        requisition.ApprovalDate = (sdr["ApprovalDate"].ToString() != "" ? DateTime.Parse(sdr["ApprovalDate"].ToString()) : requisition.ApprovalDate);
                                        requisition.ApprovalDateFormatted = (sdr["ApprovalDate"].ToString() != "" ? DateTime.Parse(sdr["ApprovalDate"].ToString()).ToString(settings.DateFormat) : requisition.ApprovalDateFormatted);
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

        #region GetRequisitionDetailsByID
        public List<RequisitionDetail> GetRequisitionDetailsByIDs(string IDs, Guid POID)
        {
            List<RequisitionDetail> requisitionList = null;
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
                        cmd.CommandText = "[AMC].[GetRequisitionDetailsByIDs]";
                        cmd.Parameters.Add("@IDs", SqlDbType.NVarChar, -1).Value = IDs;
                        if (POID != Guid.Empty)
                            cmd.Parameters.Add("@POID", SqlDbType.UniqueIdentifier).Value = POID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                requisitionList = new List<RequisitionDetail>();
                                while (sdr.Read())
                                {
                                    RequisitionDetail requisition = new RequisitionDetail();
                                    {
                                        requisition.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : requisition.ID);
                                        requisition.ReqID = (sdr["ReqID"].ToString() != "" ? Guid.Parse(sdr["ReqID"].ToString()) : requisition.ReqID);
                                        requisition.MaterialID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : requisition.MaterialID);
                                        requisition.ReqNo = (sdr["ReqNo"].ToString() != "" ? sdr["ReqNo"].ToString() : requisition.ReqNo);
                                        requisition.ApproximateRate = (sdr["Rate"].ToString() != "" ? sdr["Rate"].ToString() : requisition.ApproximateRate);
                                        requisition.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : requisition.Description);
                                        requisition.RequestedQty = (sdr["RequestedQty"].ToString() != "" ? sdr["RequestedQty"].ToString() : requisition.RequestedQty);
                                        requisition.OrderedQty = (sdr["OrderedQty"].ToString() != "" ? sdr["OrderedQty"].ToString() : requisition.OrderedQty);
                                        requisition.Material = new Material();
                                        requisition.Material.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : requisition.Material.MaterialCode);
                                        requisition.Material.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : requisition.Material.UnitCode);
                                        requisition.Discount = 0;
                                        //requisition.POQty = (decimal.Parse(requisition.RequestedQty) - decimal.Parse(requisition.OrderedQty)).ToString();
                                        if ((decimal.Parse(requisition.RequestedQty) - decimal.Parse(requisition.OrderedQty)) > 0)
                                            requisition.POQty = (decimal.Parse(requisition.RequestedQty) - decimal.Parse(requisition.OrderedQty)).ToString();
                                        else
                                            requisition.POQty = "0";
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


        #endregion GetRequisitionDetailsByID

        public Requisition GetRequisition(Guid ID)
        {
            Requisition requisition = new Requisition();
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
                        cmd.CommandText = "[AMC].[GetRequisition]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    requisition.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : requisition.ID);
                                    requisition.ReqNo = (sdr["ReqNo"].ToString() != "" ? sdr["ReqNo"].ToString() : requisition.ReqNo);
                                    requisition.ReqDate = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()) : requisition.ReqDate);
                                    requisition.ReqDateFormatted = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()).ToString(settings.DateFormat) : requisition.ReqDateFormatted);
                                    requisition.RequiredDate = (sdr["RequiredDate"].ToString() != "" ? DateTime.Parse(sdr["RequiredDate"].ToString()) : requisition.RequiredDate);
                                    requisition.RequiredDateFormatted = (sdr["RequiredDate"].ToString() != "" ? DateTime.Parse(sdr["RequiredDate"].ToString()).ToString(settings.DateFormat) : requisition.RequiredDateFormatted);
                                    requisition.Title = (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : requisition.Title);
                                    requisition.ReqStatus = (sdr["ReqStatus"].ToString() != "" ? sdr["ReqStatus"].ToString() : requisition.ReqStatus);
                                    requisition.EmployeeID = (sdr["EmployeeID"].ToString() != "" ? Guid.Parse(sdr["EmployeeID"].ToString()) : requisition.EmployeeID);
                                    requisition.RequisitionBy = (sdr["RequisitionBy"].ToString() != "" ? sdr["RequisitionBy"].ToString() : requisition.RequisitionBy);
                                    requisition.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : requisition.ApprovalStatus);
                                    requisition.LatestApprovalStatus = (sdr["LatestApprovalStatus"].ToString() != "" ? Int16.Parse(sdr["LatestApprovalStatus"].ToString()) : requisition.LatestApprovalStatus);
                                    requisition.LatestApprovalID = (sdr["LatestApprovalID"].ToString() != "" ? Guid.Parse(sdr["LatestApprovalID"].ToString() ): requisition.LatestApprovalID);
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
            return requisition;
        }

        public List<RequisitionDetail> GetRequisitionDetail(Guid ID)
        {
            List<RequisitionDetail> requisitionList = null;
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
                        cmd.CommandText = "[AMC].[GetRequisitionDetail]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value =ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                requisitionList = new List<RequisitionDetail>();
                                while (sdr.Read())
                                {
                                    RequisitionDetail requisition = new RequisitionDetail();
                                    {
                                        requisition.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : requisition.ID);
                                        requisition.ReqID = (sdr["ReqID"].ToString() != "" ? Guid.Parse(sdr["ReqID"].ToString()) : requisition.ReqID);
                                        requisition.MaterialID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : requisition.MaterialID);
                                        requisition.ApproximateRate = (sdr["Rate"].ToString() != "" ? sdr["Rate"].ToString() : requisition.ApproximateRate);
                                        requisition.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : requisition.Description);
                                        requisition.RequestedQty = (sdr["RequestedQty"].ToString() != "" ? sdr["RequestedQty"].ToString() : requisition.RequestedQty);
                                        requisition.Material = new Material();
                                        requisition.Material.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : requisition.Material.MaterialCode);
                                        requisition.Material.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : requisition.Material.CurrentStock);
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

        public object DeleteRequisitionDetail(Guid ID)
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
                        cmd.CommandText = "[AMC].[DeleteRequisitionDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
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

        public object DeleteRequisition(Guid ID)
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
                        cmd.CommandText = "[AMC].[DeleteRequisition]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
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

        #region GetRecentRequisition
        public List<Requisition> GetRecentRequisition()
        {
            List<Requisition> requisitionList = new List<Requisition>();
            Requisition requisition = null;
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
                        cmd.CommandText = "[AMC].[GetRecentRequisition]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    requisition = new Requisition();
                                    requisition.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);
                                    requisition.ReqNo = (sdr["ReqNo"].ToString() != "" ? sdr["ReqNo"].ToString() : requisition.ReqNo);
                                    requisition.ReqDateFormatted = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()).ToString(settings.DateFormat) : requisition.ReqDateFormatted);                                   
                                    requisition.ReqStatus = (sdr["ReqStatus"].ToString() != "" ? (sdr["ReqStatus"].ToString()) : requisition.ReqStatus);
                                    requisition.RequisitionBy = (sdr["RequisitionBy"].ToString() != "" ? (sdr["RequisitionBy"].ToString()) : requisition.RequisitionBy);
                                    requisition.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? (sdr["ApprovalStatus"].ToString()) : requisition.ApprovalStatus);
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
        #endregion GetRecentRequisition
    }
}
