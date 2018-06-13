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
    public class ReportRepository : IReportRepository
    {
        Settings settings = new Settings();
        private IDatabaseFactory _databaseFactory;
        public ReportRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetAllReport
        /// <summary>
        /// To Get All Reports
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public List<AMCSysReport>GetAllReport(string searchTerm)
        {
            List<AMCSysReport> AMCSysReportList = null;
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
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(searchTerm) ? "" : searchTerm;
                        cmd.CommandText = "[AMC].[GetAllSysReports]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                AMCSysReportList = new List<AMCSysReport>();
                                while (sdr.Read())
                                {
                                    AMCSysReport amcSysReport = new AMCSysReport();
                                    {
                                        amcSysReport.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : amcSysReport.ID);
                                        amcSysReport.ReportName = (sdr["ReportName"].ToString() != "" ? (sdr["ReportName"].ToString()) : amcSysReport.ReportName);
                                        amcSysReport.ReportDescription = (sdr["ReportDescription"].ToString() != "" ? (sdr["ReportDescription"].ToString()) : amcSysReport.ReportDescription);
                                        amcSysReport.Controller = (sdr["Controller"].ToString() != "" ? sdr["Controller"].ToString() : amcSysReport.Controller);
                                        amcSysReport.Action = (sdr["Action"].ToString() != "" ? sdr["Action"].ToString() : amcSysReport.Action);
                                        amcSysReport.SPName = (sdr["SPName"].ToString() != "" ? sdr["SPName"].ToString() : amcSysReport.SPName);
                                        amcSysReport.SQL = (sdr["SQL"].ToString() != "" ? sdr["SQL"].ToString() : amcSysReport.SQL);
                                        amcSysReport.ReportOrder = (sdr["ReportOrder"].ToString() != "" ? int.Parse(sdr["ReportOrder"].ToString()) : amcSysReport.ReportOrder);
                                        amcSysReport.ReportGroup = (sdr["ReportGroup"].ToString() != "" ? sdr["ReportGroup"].ToString() : amcSysReport.ReportGroup);
                                        amcSysReport.GroupOrder = (sdr["GroupOrder"].ToString() != "" ? int.Parse(sdr["GroupOrder"].ToString()) : amcSysReport.GroupOrder);
                                    }
                                    AMCSysReportList.Add(amcSysReport);
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
            return AMCSysReportList;
        }
        #endregion GetAllReport

        #region GetRequisitionSummaryReport
        /// <summary>
        /// To Get Requisition Summary Report
        /// </summary>
        /// <param name="requisitionSummaryReport"></param>
        /// <returns></returns>
        public List<Requisition> GetRequisitionSummaryReport(RequisitionSummaryReport requisitionSummaryReport)
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
                        if (requisitionSummaryReport.DataTablePaging.OrderColumn == "")
                        {
                            requisitionSummaryReport.DataTablePaging.OrderColumn = "REQNO";
                            requisitionSummaryReport.DataTablePaging.OrderDir = "ASC";
                        }
                        cmd.CommandText = "[AMC].[GetRequisitionSummaryReport]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(requisitionSummaryReport.SearchTerm) ? "" : requisitionSummaryReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = requisitionSummaryReport.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = requisitionSummaryReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = requisitionSummaryReport.DataTablePaging.OrderDir;
                        cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar).Value = requisitionSummaryReport.DataTablePaging.OrderColumn;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = requisitionSummaryReport.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = requisitionSummaryReport.ToDate;
                        cmd.Parameters.Add("@ReqStatus", SqlDbType.VarChar).Value = requisitionSummaryReport.ReqStatus;
                        if (requisitionSummaryReport.EmployeeID != Guid.Empty)
                            cmd.Parameters.Add("@ReqBy", SqlDbType.UniqueIdentifier).Value = requisitionSummaryReport.EmployeeID;

                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                requisitionList = new List<Requisition>();
                                while (sdr.Read())
                                {
                                    Requisition requisitionObj = new Requisition();
                                    {
                                        requisitionObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : requisitionObj.ID);
                                        requisitionObj.ReqNo = (sdr["RequisitionNo"].ToString() != "" ? sdr["RequisitionNo"].ToString() : requisitionObj.ReqNo);
                                        requisitionObj.ReqDate = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()) : requisitionObj.ReqDate);
                                        requisitionObj.ReqDateFormatted = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()).ToString(settings.DateFormat) : requisitionObj.ReqDateFormatted);
                                        requisitionObj.Title = (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : requisitionObj.Title);
                                        requisitionObj.ReqStatus = (sdr["ReqStatus"].ToString() != "" ? sdr["ReqStatus"].ToString() : requisitionObj.ReqStatus);
                                        requisitionObj.RequisitionBy = (sdr["RequisitionBy"].ToString() != "" ? sdr["RequisitionBy"].ToString() : requisitionObj.RequisitionBy);
                                        requisitionObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : requisitionObj.TotalCount);
                                        requisitionObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : requisitionObj.FilteredCount);
                                        requisitionObj.ReqAmount= (sdr["ReqAmount"].ToString() != "" ? sdr["ReqAmount"].ToString() : requisitionObj.ReqAmount);
                                        requisitionObj.RequiredDate = (sdr["RequiredDate"].ToString() != "" ? DateTime.Parse(sdr["RequiredDate"].ToString()) : requisitionObj.RequiredDate);
                                        requisitionObj.RequiredDateFormatted = (sdr["RequiredDate"].ToString() != "" ? DateTime.Parse(sdr["RequiredDate"].ToString()).ToString(settings.DateFormat) : requisitionObj.RequiredDateFormatted);
                                    }
                                    requisitionList.Add(requisitionObj);
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
        #endregion GetRequisitionSummaryReport




        #region GetRequisitionDetailReport
        /// <summary>
        /// To Get Requisition Detail Report
        /// </summary>
        /// <param name="requisitionDetailReport"></param>
        /// <returns></returns>
        public List<RequisitionDetailReport> GetRequisitionDetailReport(RequisitionDetailReport requisitionDetailReport)
        {

            List<RequisitionDetailReport> requisitionDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetRequisitionDetailReport]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(requisitionDetailReport.SearchTerm) ? "" : requisitionDetailReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = requisitionDetailReport.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = requisitionDetailReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = requisitionDetailReport.DataTablePaging.OrderDir;
                        cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar).Value = requisitionDetailReport.DataTablePaging.OrderColumn;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = requisitionDetailReport.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = requisitionDetailReport.ToDate;
                        cmd.Parameters.Add("@ReqStatus", SqlDbType.VarChar).Value = requisitionDetailReport.ReqStatus;
                        if (requisitionDetailReport.EmployeeID != Guid.Empty)
                            cmd.Parameters.Add("@ReqBy", SqlDbType.UniqueIdentifier).Value = requisitionDetailReport.EmployeeID;
                        if(requisitionDetailReport.MaterialID != Guid.Empty)
                            cmd.Parameters.Add("@materialID", SqlDbType.UniqueIdentifier).Value = requisitionDetailReport.MaterialID;
                        cmd.Parameters.Add("@DeliveryStatus", SqlDbType.VarChar).Value = requisitionDetailReport.DeliveryStatus;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                requisitionDetailList = new List<RequisitionDetailReport>();
                                while (sdr.Read())
                                {
                                    RequisitionDetailReport requisitionObj = new RequisitionDetailReport();
                                    {
                                        requisitionObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : requisitionObj.ID);
                                        requisitionObj.ReqNo = (sdr["RequisitionNo"].ToString() != "" ? sdr["RequisitionNo"].ToString() : requisitionObj.ReqNo);
                                        requisitionObj.ReqDate = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()) : requisitionObj.ReqDate);
                                        requisitionObj.ReqDateFormatted = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()).ToString(settings.DateFormat) : requisitionObj.ReqDateFormatted);
                                        requisitionObj.Title = (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : requisitionObj.Title);
                                        requisitionObj.ReqStatus = (sdr["ReqStatus"].ToString() != "" ? sdr["ReqStatus"].ToString() : requisitionObj.ReqStatus);
                                        requisitionObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : requisitionObj.TotalCount);
                                        requisitionObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : requisitionObj.FilteredCount);
                                        requisitionObj.Material = new Material();
                                        requisitionObj.Material.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : requisitionObj.Material.UnitCode);
                                        requisitionObj.Material.Description = (sdr["ITEM"].ToString() != "" ? sdr["ITEM"].ToString() : requisitionObj.Material.Description);
                                        requisitionObj.RequestedQty = (sdr["RequestedQty"].ToString() != "" ? sdr["RequestedQty"].ToString() : requisitionObj.RequestedQty);
                                        requisitionObj.OrderedQty= (sdr["OrderQty"].ToString() != "" ? sdr["OrderQty"].ToString() : requisitionObj.OrderedQty);
                                        requisitionObj.ReceivedQty = (sdr["ReceivedQty"].ToString() != "" ? sdr["ReceivedQty"].ToString() : requisitionObj.ReceivedQty);
                                        requisitionObj.DeliveryStatus= (sdr["DeliveryStatus"].ToString() != "" ? sdr["DeliveryStatus"].ToString() : requisitionObj.DeliveryStatus);
                                    }
                                    requisitionDetailList.Add(requisitionObj);
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

            return requisitionDetailList;
        }
        #endregion GetRequisitionDetailReport

        #region GetPurchaseSummaryReport
        /// <summary>
        /// To Get Purchase Summary Report
        /// </summary>
        /// <param name="purchaseSummaryReport"></param>
        /// <returns></returns>
        public List<PurchaseOrder> GetPurchaseSummaryReport(PurchaseSummaryReport purchaseSummaryReport)
        {

            List<PurchaseOrder> purchaseOrderList = null;
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
                        if (purchaseSummaryReport.DataTablePaging.OrderColumn == "")
                        {
                            purchaseSummaryReport.DataTablePaging.OrderColumn = "PURCHASEORDERNO";
                            purchaseSummaryReport.DataTablePaging.OrderDir = "ASC";
                        }
                        cmd.CommandText = "[AMC].[GetPurchaseSummaryReport]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(purchaseSummaryReport.SearchTerm) ? "" : purchaseSummaryReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = purchaseSummaryReport.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = purchaseSummaryReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = purchaseSummaryReport.DataTablePaging.OrderDir;
                        cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar).Value = purchaseSummaryReport.DataTablePaging.OrderColumn;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = purchaseSummaryReport.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = purchaseSummaryReport.ToDate;
                        cmd.Parameters.Add("@POStatus", SqlDbType.VarChar).Value = purchaseSummaryReport.Status;
                        if (purchaseSummaryReport.SupplierID != Guid.Empty)
                            cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = purchaseSummaryReport.SupplierID;
                        cmd.Parameters.Add("@EmailSentYN", SqlDbType.NVarChar).Value = purchaseSummaryReport.EmailedYN;


                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                purchaseOrderList = new List<PurchaseOrder>();
                                while (sdr.Read())
                                {
                                    PurchaseOrder purchaseOrderObj = new PurchaseOrder();
                                    {
                                        purchaseOrderObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : purchaseOrderObj.ID);
                                        purchaseOrderObj.PurchaseOrderNo = (sdr["PONo"].ToString() != "" ? sdr["PONo"].ToString() : purchaseOrderObj.PurchaseOrderNo);
                                        purchaseOrderObj.PurchaseOrderDate = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()) : purchaseOrderObj.PurchaseOrderDate);
                                        purchaseOrderObj.PurchaseOrderDateFormatted = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()).ToString(settings.DateFormat) : purchaseOrderObj.PurchaseOrderDateFormatted);
                                        purchaseOrderObj.PurchaseOrderTitle = (sdr["PurchaseOrderTitle"].ToString() != "" ? sdr["PurchaseOrderTitle"].ToString() : purchaseOrderObj.PurchaseOrderTitle);
                                        purchaseOrderObj.Supplier = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : purchaseOrderObj.Supplier);
                                        purchaseOrderObj.PurchaseOrderStatus = (sdr["PurchaseOrderStatus"].ToString() != "" ? sdr["PurchaseOrderStatus"].ToString() : purchaseOrderObj.PurchaseOrderStatus);
                                        purchaseOrderObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : purchaseOrderObj.TotalCount);
                                        purchaseOrderObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : purchaseOrderObj.FilteredCount);
                                        purchaseOrderObj.EmailSentYN= (sdr["EmailSentYN"].ToString() != "" ? sdr["EmailSentYN"].ToString() : purchaseOrderObj.EmailSentYN);
                                        purchaseOrderObj.GrossAmount = (sdr["PurchaseAmount"].ToString() != "" ? decimal.Parse(sdr["PurchaseAmount"].ToString()) : purchaseOrderObj.GrossAmount);
                                        purchaseOrderObj.PurchaseOrderIssuedDate = (sdr["PurchaseOrderIssuedDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderIssuedDate"].ToString()) : purchaseOrderObj.PurchaseOrderIssuedDate);
                                        purchaseOrderObj.PurchaseOrderIssuedDateFormatted = (sdr["PurchaseOrderIssuedDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderIssuedDate"].ToString()).ToString(settings.DateFormat) : purchaseOrderObj.PurchaseOrderIssuedDateFormatted);
                                    }
                                    purchaseOrderList.Add(purchaseOrderObj);
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

            return purchaseOrderList;
        }
        #endregion GetPurchaseSummaryReport

        #region GetPurchaseDetailReport
        /// <summary>
        /// To get Purchase Detail Report
        /// </summary>
        /// <param name="purchaseDetailReport"></param>
        /// <returns></returns>
        public List<PurchaseDetailReport> GetPurchaseDetailReport(PurchaseDetailReport purchaseDetailReport)
        {

            List<PurchaseDetailReport> purchaseDetailList = null;
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
                        if (purchaseDetailReport.DataTablePaging.OrderColumn == "")
                        {
                            purchaseDetailReport.DataTablePaging.OrderColumn = "PURCHASEORDERNO";
                            purchaseDetailReport.DataTablePaging.OrderDir = "ASC";
                        }
                        cmd.CommandText = "[AMC].[GetPurcaseDetailReport]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(purchaseDetailReport.SearchTerm) ? "" : purchaseDetailReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = purchaseDetailReport.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = purchaseDetailReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = purchaseDetailReport.DataTablePaging.OrderDir;
                        cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar).Value = purchaseDetailReport.DataTablePaging.OrderColumn;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = purchaseDetailReport.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = purchaseDetailReport.ToDate;
                        cmd.Parameters.Add("@POStatus", SqlDbType.VarChar).Value = purchaseDetailReport.Status;
                        if (purchaseDetailReport.SupplierID != Guid.Empty)
                            cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = purchaseDetailReport.SupplierID;
                        if (purchaseDetailReport.MaterialID != Guid.Empty)
                            cmd.Parameters.Add("@MaterialID", SqlDbType.UniqueIdentifier).Value = purchaseDetailReport.MaterialID;
                        cmd.Parameters.Add("@ApprovalStatus", SqlDbType.NVarChar).Value = purchaseDetailReport.ApprovalStatus;
                        cmd.Parameters.Add("@DeliveryStatus", SqlDbType.VarChar).Value = purchaseDetailReport.DeliveryStatus;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                purchaseDetailList = new List<PurchaseDetailReport>();
                                while (sdr.Read())
                                {
                                    PurchaseDetailReport purchaseOrderObj = new PurchaseDetailReport();
                                    {
                                        purchaseOrderObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : purchaseOrderObj.ID);
                                        purchaseOrderObj.PurchaseOrderNo = (sdr["PONo"].ToString() != "" ? sdr["PONo"].ToString() : purchaseOrderObj.PurchaseOrderNo);
                                        purchaseOrderObj.PurchaseOrderDate = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()) : purchaseOrderObj.PurchaseOrderDate);
                                        purchaseOrderObj.PurchaseOrderDateFormatted = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()).ToString(settings.DateFormat) : purchaseOrderObj.PurchaseOrderDateFormatted);
                                        purchaseOrderObj.Status = (sdr["PurchaseOrderStatus"].ToString() != "" ? sdr["PurchaseOrderStatus"].ToString() : purchaseOrderObj.Status);
                                        purchaseOrderObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : purchaseOrderObj.TotalCount);
                                        purchaseOrderObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : purchaseOrderObj.FilteredCount);
                                        purchaseOrderObj.Material = new Material();
                                        purchaseOrderObj.Material.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : purchaseOrderObj.Material.UnitCode);
                                        purchaseOrderObj.Material.Description = (sdr["ITEM"].ToString() != "" ? sdr["ITEM"].ToString() : purchaseOrderObj.Material.Description);
                                        purchaseOrderObj.Supplier = new Supplier();
                                        purchaseOrderObj.Supplier.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : purchaseOrderObj.Supplier.CompanyName);
                                        purchaseOrderObj.POQty = (sdr["OrderQty"].ToString() != "" ?decimal.Parse( sdr["OrderQty"].ToString()) : purchaseOrderObj.POQty);
                                        purchaseOrderObj.PrevRcvQty = (sdr["ReceivedQty"].ToString() != "" ? decimal.Parse(sdr["ReceivedQty"].ToString()) : purchaseOrderObj.PrevRcvQty);
                                        purchaseOrderObj.ApprovalStatus= (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : purchaseOrderObj.ApprovalStatus);
                                        purchaseOrderObj.DeliveryStatus = (sdr["DeliveryStatus"].ToString() != "" ? sdr["DeliveryStatus"].ToString() : purchaseOrderObj.DeliveryStatus);
                                    }
                                    purchaseDetailList.Add(purchaseOrderObj);
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

            return purchaseDetailList;
        }
        #endregion GetPurchaseDetailReport

        #region GetPurchaseRegisterReport
        /// <summary>
        /// To Get Purchase Register Report
        /// </summary>
        /// <param name="purchaseRegisterReport"></param>
        /// <returns></returns>
        public List<PurchaseRegisterReport> GetPurchaseRegisterReport(PurchaseRegisterReport purchaseRegisterReport)
        {

            List<PurchaseRegisterReport> purchaseRegisterList = null;
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
                        if (purchaseRegisterReport.DataTablePaging.OrderColumn == "")
                        {
                            purchaseRegisterReport.DataTablePaging.OrderColumn = "PURCHASEORDERNO";
                            purchaseRegisterReport.DataTablePaging.OrderDir = "ASC";
                        }
                        cmd.CommandText = "[AMC].[GetPurchaseRegisterReport]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(purchaseRegisterReport.SearchTerm) ? "" : purchaseRegisterReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = purchaseRegisterReport.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = purchaseRegisterReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = purchaseRegisterReport.DataTablePaging.OrderDir;
                        cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar).Value = purchaseRegisterReport.DataTablePaging.OrderColumn;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = purchaseRegisterReport.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = purchaseRegisterReport.ToDate;
                        cmd.Parameters.Add("@POStatus", SqlDbType.VarChar).Value = purchaseRegisterReport.Status;
                        cmd.Parameters.Add("@PaymentStatus", SqlDbType.VarChar).Value = purchaseRegisterReport.PaymentStatus;
                        if (purchaseRegisterReport.SupplierID != Guid.Empty)
                            cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = purchaseRegisterReport.SupplierID;

                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                purchaseRegisterList = new List<PurchaseRegisterReport>();
                                while (sdr.Read())
                                {
                                    PurchaseRegisterReport purchaseRegisterObj = new PurchaseRegisterReport();
                                    {
                                        purchaseRegisterObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : purchaseRegisterObj.ID);
                                        purchaseRegisterObj.PurchaseOrderNo = (sdr["PONo"].ToString() != "" ? sdr["PONo"].ToString() : purchaseRegisterObj.PurchaseOrderNo);
                                        purchaseRegisterObj.PurchaseOrderDate = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()) : purchaseRegisterObj.PurchaseOrderDate);
                                        purchaseRegisterObj.PurchaseOrderDateFormatted = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()).ToString(settings.DateFormat) : purchaseRegisterObj.PurchaseOrderDateFormatted);
                                        purchaseRegisterObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : purchaseRegisterObj.TotalCount);
                                        purchaseRegisterObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : purchaseRegisterObj.FilteredCount);
                                        purchaseRegisterObj.Supplier = new Supplier();
                                        purchaseRegisterObj.Supplier.CompanyName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : purchaseRegisterObj.Supplier.CompanyName);
                                        purchaseRegisterObj.GrossAmount= (sdr["GrossAmount"].ToString() != "" ? decimal.Parse(sdr["GrossAmount"].ToString()) : purchaseRegisterObj.GrossAmount);
                                        purchaseRegisterObj.Discount= (sdr["Discount"].ToString() != "" ? decimal.Parse(sdr["Discount"].ToString()) : purchaseRegisterObj.Discount);
                                        purchaseRegisterObj.TaxableAmount = (sdr["TaxableAmount"].ToString() != "" ? decimal.Parse(sdr["TaxableAmount"].ToString()) : purchaseRegisterObj.TaxableAmount);
                                        purchaseRegisterObj.GSTPerc = (sdr["GSTPercentage"].ToString() != "" ? decimal.Parse(sdr["GSTPercentage"].ToString()) : purchaseRegisterObj.GSTPerc);
                                        purchaseRegisterObj.GSTAmt = (sdr["GSTAMOUNT"].ToString() != "" ? decimal.Parse(sdr["GSTAMOUNT"].ToString()) : purchaseRegisterObj.GSTAmt);
                                        purchaseRegisterObj.NetAmount = (sdr["NetAmt"].ToString() != "" ? decimal.Parse(sdr["NetAmt"].ToString()) : purchaseRegisterObj.NetAmount);
                                        purchaseRegisterObj.InvoicedAmount = (sdr["InvcdAmount"].ToString() != "" ? decimal.Parse(sdr["InvcdAmount"].ToString()) : purchaseRegisterObj.InvoicedAmount);
                                        purchaseRegisterObj.PaidAmount = (sdr["PaidAmunt"].ToString() != "" ? decimal.Parse(sdr["PaidAmunt"].ToString()) : purchaseRegisterObj.PaidAmount);

                                    }
                                    purchaseRegisterList.Add(purchaseRegisterObj);
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

            return purchaseRegisterList;
        }
        #endregion GetPurchaseRegisterReport


        #region GetInventoryReorderStatusReport
        /// <summary>
        /// To Get Inventory Reorder Status Report
        /// </summary>
        /// <param name="inventoryReOrderStatusReport"></param>
        /// <returns></returns>
        public List<InventoryReorderStatusReport> GetInventoryReorderStatusReport(InventoryReorderStatusReport inventoryReOrderStatusReport)
        {

            List<InventoryReorderStatusReport> inventoryReOrderList = null;
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
                        //if (inventoryReOrderStatusReport.DataTablePaging.OrderColumn == "")
                        //{
                        //    inventoryReOrderStatusReport.DataTablePaging.OrderColumn = "PURCHASEORDERNO";
                        //    inventoryReOrderStatusReport.DataTablePaging.OrderDir = "ASC";
                        //}
                        cmd.CommandText = "[AMC].[GetInventoryReOrderStatusReport]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(inventoryReOrderStatusReport.SearchTerm) ? "" : inventoryReOrderStatusReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = inventoryReOrderStatusReport.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = inventoryReOrderStatusReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = inventoryReOrderStatusReport.DataTablePaging.OrderDir;
                        cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar).Value = inventoryReOrderStatusReport.DataTablePaging.OrderColumn;
                        cmd.Parameters.Add("@ItemStatus", SqlDbType.Int).Value = inventoryReOrderStatusReport.ItemStatus;
                        if (inventoryReOrderStatusReport.MaterialID != Guid.Empty)
                            cmd.Parameters.Add("@MaterialID", SqlDbType.UniqueIdentifier).Value = inventoryReOrderStatusReport.MaterialID;
                        cmd.Parameters.Add("@MaterialType", SqlDbType.NVarChar).Value = inventoryReOrderStatusReport.Code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                inventoryReOrderList = new List<InventoryReorderStatusReport>();
                                while (sdr.Read())
                                {
                                    InventoryReorderStatusReport inventoryReOrderObj = new InventoryReorderStatusReport();
                                    {

                                        inventoryReOrderObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : inventoryReOrderObj.ID);
                                        inventoryReOrderObj.Description = (sdr["Item"].ToString() != "" ? (sdr["Item"].ToString()) : inventoryReOrderObj.Description);
                                        inventoryReOrderObj.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : inventoryReOrderObj.CurrentStock);
                                        inventoryReOrderObj.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? decimal.Parse(sdr["ReorderQty"].ToString()) : inventoryReOrderObj.ReorderQty);
                                        inventoryReOrderObj.PODueQty = (sdr["PODueQty"].ToString() != "" ? decimal.Parse(sdr["PODueQty"].ToString()) : inventoryReOrderObj.PODueQty);
                                        inventoryReOrderObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : inventoryReOrderObj.TotalCount);
                                        inventoryReOrderObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : inventoryReOrderObj.FilteredCount);
                                        inventoryReOrderObj.NetAvailableQty = (sdr["NetAvailableQty"].ToString() != "" ? decimal.Parse(sdr["NetAvailableQty"].ToString()) : inventoryReOrderObj.NetAvailableQty);
                                        inventoryReOrderObj.ShortFall = (sdr["ShortFall"].ToString() != "" ? decimal.Parse(sdr["ShortFall"].ToString()) : inventoryReOrderObj.ShortFall);
                                    }
                                    inventoryReOrderList.Add(inventoryReOrderObj);
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

            return inventoryReOrderList;
        }

        #endregion GetInventoryReorderStatusReport

        #region GetStockRegisterReport
        /// <summary>
        /// To Get Stock Register Report
        /// </summary>
        /// <param name="stockRegisterReport"></param>
        /// <returns></returns>
        public List<StockRegisterReport> GetStockRegisterReport(StockRegisterReport stockRegisterReport)
        {

            List<StockRegisterReport> stockRegisterReportList = null;
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
                        cmd.CommandText = "[AMC].[GetStockRegisterReport]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(stockRegisterReport.SearchTerm) ? "" : stockRegisterReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = stockRegisterReport.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = stockRegisterReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = stockRegisterReport.DataTablePaging.OrderDir;
                        cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar).Value = stockRegisterReport.DataTablePaging.OrderColumn;
                        if (stockRegisterReport.MaterialID != Guid.Empty)
                            cmd.Parameters.Add("@MaterialID", SqlDbType.UniqueIdentifier).Value = stockRegisterReport.MaterialID;
                        cmd.Parameters.Add("@materialTypeCode", SqlDbType.NVarChar).Value = stockRegisterReport.MaterialTypeCode;
                        cmd.Parameters.Add("@Location", SqlDbType.VarChar).Value = stockRegisterReport.Location;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                stockRegisterReportList = new List<StockRegisterReport>();
                                while (sdr.Read())
                                {
                                    StockRegisterReport stockRegisterObj = new StockRegisterReport();
                                    {

                                        stockRegisterObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : stockRegisterObj.ID);
                                        stockRegisterObj.TypeDescription = (sdr["MaterialType"].ToString() != "" ? (sdr["MaterialType"].ToString()) : stockRegisterObj.TypeDescription);
                                        stockRegisterObj.Description = (sdr["Item"].ToString() != "" ? (sdr["Item"].ToString()) : stockRegisterObj.Description);
                                        stockRegisterObj.UnitCode = (sdr["UnitCode"].ToString() != "" ? (sdr["UnitCode"].ToString()) : stockRegisterObj.UnitCode);
                                        stockRegisterObj.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : stockRegisterObj.CurrentStock);
                                        stockRegisterObj.CostPrice = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : stockRegisterObj.CostPrice);
                                        stockRegisterObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : stockRegisterObj.TotalCount);
                                        stockRegisterObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : stockRegisterObj.FilteredCount);
                                        stockRegisterObj.StockValue = (sdr["StockValue"].ToString() != "" ? decimal.Parse(sdr["StockValue"].ToString()) : stockRegisterObj.StockValue);
                                    }
                                    stockRegisterReportList.Add(stockRegisterObj);
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

            return stockRegisterReportList;
        }

        #endregion GetStockRegisterReport

        #region GetStockLedgerReport
        /// <summary>
        /// To Get Stock Ledger Report
        /// </summary>
        /// <param name="stockLedgerReport"></param>
        /// <returns></returns>
        public List<StockLedgerReport> GetStockLedgerReport(StockLedgerReport stockLedgerReport)
        {

            List<StockLedgerReport> stockLedgerReportList = null;
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
                        cmd.CommandText = "[AMC].[GetStockLedgerReport]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(stockLedgerReport.SearchTerm) ? "" : stockLedgerReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = stockLedgerReport.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = stockLedgerReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = stockLedgerReport.DataTablePaging.OrderDir;
                        cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar).Value = stockLedgerReport.DataTablePaging.OrderColumn;
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = stockLedgerReport.FromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = stockLedgerReport.ToDate;
                        cmd.Parameters.Add("@TransactionType", SqlDbType.VarChar).Value = stockLedgerReport.TransactionType;
                        cmd.Parameters.Add("@materialTypeCode", SqlDbType.NVarChar).Value = stockLedgerReport.MaterialTypeCode;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                stockLedgerReportList = new List<StockLedgerReport>();
                                while (sdr.Read())
                                {
                                    StockLedgerReport stockLedgerObj = new StockLedgerReport();
                                    {

                                        stockLedgerObj.MaterialID = (sdr["materialID"].ToString() != "" ? Guid.Parse(sdr["materialID"].ToString()) : stockLedgerObj.MaterialID);
                                        stockLedgerObj.Description = (sdr["Item"].ToString() != "" ? (sdr["Item"].ToString()) : stockLedgerObj.Description);
                                        stockLedgerObj.UnitCode = (sdr["UnitCode"].ToString() != "" ? (sdr["UnitCode"].ToString()) : stockLedgerObj.UnitCode);
                                        stockLedgerObj.OpeningStock = (sdr["OpeningStock"].ToString() != "" ? decimal.Parse(sdr["OpeningStock"].ToString()) : stockLedgerObj.OpeningStock);
                                        stockLedgerObj.ClosingStock = (sdr["ClosingStock"].ToString() != "" ? decimal.Parse(sdr["ClosingStock"].ToString()) : stockLedgerObj.ClosingStock);
                                        stockLedgerObj.TransactionType = (sdr["TransactionType"].ToString() != "" ? (sdr["TransactionType"].ToString()) : stockLedgerObj.TransactionType);
                                        stockLedgerObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : stockLedgerObj.TotalCount);
                                        stockLedgerObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : stockLedgerObj.FilteredCount);
                                        stockLedgerObj.DocumentNo= (sdr["DocumentNo"].ToString() != "" ? (sdr["DocumentNo"].ToString()) : stockLedgerObj.DocumentNo);
                                        stockLedgerObj.TransactionDate = (sdr["TrDate"].ToString() != "" ? DateTime.Parse(sdr["TrDate"].ToString()) : stockLedgerObj.TransactionDate);
                                        stockLedgerObj.TransactionDateFormatted = (sdr["TrDate"].ToString() != "" ? DateTime.Parse(sdr["TrDate"].ToString()).ToString(settings.DateFormat) : stockLedgerObj.TransactionDateFormatted);
                                        stockLedgerObj.StockIn = (sdr["StockIn"].ToString() != "" ? decimal.Parse(sdr["StockIn"].ToString()) : stockLedgerObj.StockIn);
                                        stockLedgerObj.StockOut = (sdr["StockOut"].ToString() != "" ? decimal.Parse(sdr["StockOut"].ToString()) : stockLedgerObj.StockOut);

                                    }
                                    stockLedgerReportList.Add(stockLedgerObj);
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

            return stockLedgerReportList;
        }
        #endregion GetStockLedgerReport


        #region GetInventoryReOrderStatusFGReport
        /// <summary>
        /// To Get Inventory ReOrder Status FG Report
        /// </summary>
        /// <param name="inventoryReOrderStatusFGReport"></param>
        /// <returns></returns>
        public List<InventoryReOrderStatusFGReport> GetInventoryReOrderStatusFGReport(InventoryReOrderStatusFGReport inventoryReOrderStatusFGReport)
        {
            List<InventoryReOrderStatusFGReport> inventoryReOrderFGList = null;
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
                        cmd.CommandText = "[AMC].[GetInventoryReOrderStatusReport_FG]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(inventoryReOrderStatusFGReport.SearchTerm) ? "" : inventoryReOrderStatusFGReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = inventoryReOrderStatusFGReport.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = inventoryReOrderStatusFGReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = inventoryReOrderStatusFGReport.DataTablePaging.OrderDir;
                        cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar).Value = inventoryReOrderStatusFGReport.DataTablePaging.OrderColumn;
                        cmd.Parameters.Add("@ItemStatus", SqlDbType.Int).Value = inventoryReOrderStatusFGReport.ItemStatus;                       
                        cmd.Parameters.Add("@ProductType", SqlDbType.VarChar).Value = inventoryReOrderStatusFGReport.ProductType;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                inventoryReOrderFGList = new List<InventoryReOrderStatusFGReport>();
                                while (sdr.Read())
                                {
                                    InventoryReOrderStatusFGReport inventoryReOrderFGObj = new InventoryReOrderStatusFGReport();
                                    {
                                        inventoryReOrderFGObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : inventoryReOrderFGObj.ID);
                                        inventoryReOrderFGObj.Description = (sdr["Item"].ToString() != "" ? (sdr["Item"].ToString()) : inventoryReOrderFGObj.Description);
                                        inventoryReOrderFGObj.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : inventoryReOrderFGObj.CurrentStock);
                                        inventoryReOrderFGObj.ReorderQty = (sdr["ReorderQty"].ToString() != "" ? decimal.Parse(sdr["ReorderQty"].ToString()) : inventoryReOrderFGObj.ReorderQty);
                                        inventoryReOrderFGObj.SalesOrderDueQty = (sdr["SalesOrderDueQty"].ToString() != "" ? decimal.Parse(sdr["SalesOrderDueQty"].ToString()) : inventoryReOrderFGObj.SalesOrderDueQty);
                                        inventoryReOrderFGObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : inventoryReOrderFGObj.TotalCount);
                                        inventoryReOrderFGObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : inventoryReOrderFGObj.FilteredCount);
                                        inventoryReOrderFGObj.NetAvailableQty = (sdr["NetAvailableQty"].ToString() != "" ? decimal.Parse(sdr["NetAvailableQty"].ToString()) : inventoryReOrderFGObj.NetAvailableQty);
                                        inventoryReOrderFGObj.ShortFall = (sdr["ShortFall"].ToString() != "" ? decimal.Parse(sdr["ShortFall"].ToString()) : inventoryReOrderFGObj.ShortFall);
                                        inventoryReOrderFGObj.ProductType= (sdr["ProductType"].ToString() != "" ? (sdr["ProductType"].ToString()) : inventoryReOrderFGObj.ProductType);
                                    }
                                    inventoryReOrderFGList.Add(inventoryReOrderFGObj);
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
            return inventoryReOrderFGList;
        }
        #endregion GetInventoryReOrderStatusFGReport

        #region GetStockRegisterFGReport
        /// <summary>
        /// To Get Stock Register FG Report
        /// </summary>
        /// <param name="stockRegisterFGReport"></param>
        /// <returns></returns>
        public List<StockRegisterFGReport> GetStockRegisterFGReport(StockRegisterFGReport stockRegisterFGReport)
        {

            List<StockRegisterFGReport> stockRegisterFGReportList = null;
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
                        cmd.CommandText = "[AMC].[GetStockRegisterReport_FG]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(stockRegisterFGReport.SearchTerm) ? "" : stockRegisterFGReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = stockRegisterFGReport.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = stockRegisterFGReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = stockRegisterFGReport.DataTablePaging.OrderDir;
                        cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar).Value = stockRegisterFGReport.DataTablePaging.OrderColumn;                      
                        cmd.Parameters.Add("@ProductType", SqlDbType.VarChar).Value = stockRegisterFGReport.ProductType;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                stockRegisterFGReportList = new List<StockRegisterFGReport>();
                                while (sdr.Read())
                                {
                                    StockRegisterFGReport stockRegisterFGObj = new StockRegisterFGReport();
                                    {

                                        stockRegisterFGObj.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : stockRegisterFGObj.ID);
                                        stockRegisterFGObj.ProductType = (sdr["ProductType"].ToString() != "" ? (sdr["ProductType"].ToString()) : stockRegisterFGObj.ProductType);
                                        stockRegisterFGObj.Description = (sdr["Item"].ToString() != "" ? (sdr["Item"].ToString()) : stockRegisterFGObj.Description);
                                        stockRegisterFGObj.UnitCode = (sdr["UnitCode"].ToString() != "" ? (sdr["UnitCode"].ToString()) : stockRegisterFGObj.UnitCode);
                                        stockRegisterFGObj.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : stockRegisterFGObj.CurrentStock);
                                        stockRegisterFGObj.CostPrice = (sdr["CostPrice"].ToString() != "" ? decimal.Parse(sdr["CostPrice"].ToString()) : stockRegisterFGObj.CostPrice);
                                        stockRegisterFGObj.CostAmount = (sdr["CostAmount"].ToString() != "" ? decimal.Parse(sdr["CostAmount"].ToString()) : stockRegisterFGObj.CostAmount);
                                        stockRegisterFGObj.SellingRate = (sdr["SellingRate"].ToString() != "" ? decimal.Parse(sdr["SellingRate"].ToString()) : stockRegisterFGObj.SellingRate);
                                        stockRegisterFGObj.SellingAmount = (sdr["SellingAmount"].ToString() != "" ? decimal.Parse(sdr["SellingAmount"].ToString()) : stockRegisterFGObj.SellingAmount);
                                        stockRegisterFGObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : stockRegisterFGObj.TotalCount);
                                        stockRegisterFGObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : stockRegisterFGObj.FilteredCount);                                       
                                        stockRegisterFGObj.StockCostAmount = (sdr["TotalCostAmount"].ToString() != "" ? (sdr["TotalCostAmount"].ToString()) : stockRegisterFGObj.StockCostAmount);
                                        stockRegisterFGObj.StockSellingAmount = (sdr["TotalSellingAmount"].ToString() != "" ? (sdr["TotalSellingAmount"].ToString()) : stockRegisterFGObj.StockSellingAmount);

                                    }
                                    stockRegisterFGReportList.Add(stockRegisterFGObj);
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

            return stockRegisterFGReportList;
        }
        #endregion GetStockRegisterFGReport

        #region GetStockLedgerFGReport
        /// <summary>
        /// To Get Stock Ledger FG Report
        /// </summary>
        /// <param name="stockLedgerFGReport"></param>
        /// <returns></returns>
        public List<StockLedgerFGReport> GetStockLedgerFGReport(StockLedgerFGReport stockLedgerFGReport)
        {

            List<StockLedgerFGReport> stockLedgerFGReportList = null;
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
                        cmd.CommandText = "[AMC].[GetStockLedgerReport_FG]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(stockLedgerFGReport.SearchTerm) ? "" : stockLedgerFGReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = stockLedgerFGReport.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = stockLedgerFGReport.DataTablePaging.Length;
                        cmd.Parameters.Add("@OrderDir", SqlDbType.VarChar).Value = stockLedgerFGReport.DataTablePaging.OrderDir;
                        cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar).Value = stockLedgerFGReport.DataTablePaging.OrderColumn;
                        cmd.Parameters.Add("@DateFrom", SqlDbType.DateTime).Value = stockLedgerFGReport.FromDate;
                        cmd.Parameters.Add("@DateTo", SqlDbType.DateTime).Value = stockLedgerFGReport.ToDate;
                        cmd.Parameters.Add("@TransactionType", SqlDbType.VarChar).Value = stockLedgerFGReport.TransactionType;
                        cmd.Parameters.Add("@ProductType", SqlDbType.NVarChar).Value = stockLedgerFGReport.ProductType;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                stockLedgerFGReportList = new List<StockLedgerFGReport>();
                                while (sdr.Read())
                                {
                                    StockLedgerFGReport stockLedgerFGObj = new StockLedgerFGReport();
                                    {

                                        stockLedgerFGObj.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : stockLedgerFGObj.ProductID);
                                        stockLedgerFGObj.Description = (sdr["Item"].ToString() != "" ? (sdr["Item"].ToString()) : stockLedgerFGObj.Description);
                                        stockLedgerFGObj.UnitCode = (sdr["UnitCode"].ToString() != "" ? (sdr["UnitCode"].ToString()) : stockLedgerFGObj.UnitCode);
                                        stockLedgerFGObj.OpeningStock = (sdr["OpeningStock"].ToString() != "" ? decimal.Parse(sdr["OpeningStock"].ToString()) : stockLedgerFGObj.OpeningStock);
                                        stockLedgerFGObj.ClosingStock = (sdr["ClosingStock"].ToString() != "" ? decimal.Parse(sdr["ClosingStock"].ToString()) : stockLedgerFGObj.ClosingStock);
                                        stockLedgerFGObj.TransactionType = (sdr["TransactionType"].ToString() != "" ? (sdr["TransactionType"].ToString()) : stockLedgerFGObj.TransactionType);
                                        stockLedgerFGObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : stockLedgerFGObj.TotalCount);
                                        stockLedgerFGObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : stockLedgerFGObj.FilteredCount);
                                        stockLedgerFGObj.DocumentNo = (sdr["DocumentNo"].ToString() != "" ? (sdr["DocumentNo"].ToString()) : stockLedgerFGObj.DocumentNo);
                                        stockLedgerFGObj.TransactionDate = (sdr["TrDate"].ToString() != "" ? DateTime.Parse(sdr["TrDate"].ToString()) : stockLedgerFGObj.TransactionDate);
                                        stockLedgerFGObj.TransactionDateFormatted = (sdr["TrDate"].ToString() != "" ? DateTime.Parse(sdr["TrDate"].ToString()).ToString(settings.DateFormat) : stockLedgerFGObj.TransactionDateFormatted);
                                        stockLedgerFGObj.StockIn = (sdr["StockIn"].ToString() != "" ? decimal.Parse(sdr["StockIn"].ToString()) : stockLedgerFGObj.StockIn);
                                        stockLedgerFGObj.StockOut = (sdr["StockOut"].ToString() != "" ? decimal.Parse(sdr["StockOut"].ToString()) : stockLedgerFGObj.StockOut);

                                    }
                                    stockLedgerFGReportList.Add(stockLedgerFGObj);
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

            return stockLedgerFGReportList;
        }
        #endregion GetStockLedgerFGReport

        #region GetProductStageWiseStockReport
        /// <summary>
        /// To Get Product Stage Wise Stock Report
        /// </summary>
        /// <param name="productStagewiseReport"></param>
        /// <returns></returns>
        public List<ProductStageWiseStockReport> GetProductStageWiseStockReport(ProductStageWiseStockReport productStagewiseReport)
        {

            List<ProductStageWiseStockReport> productStagewiseReportList = null;
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
                        cmd.CommandText = "[AMC].[GetProductStageWiseReport]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(productStagewiseReport.SearchTerm) ? "" : productStagewiseReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = productStagewiseReport.DataTablePaging.Start;
                        cmd.Parameters.Add("@Length", SqlDbType.Int).Value = productStagewiseReport.DataTablePaging.Length;
                        if (productStagewiseReport.ProductID != Guid.Empty)
                        {
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value = productStagewiseReport.ProductID;




                            cmd.CommandType = CommandType.StoredProcedure;
                            using (SqlDataReader sdr = cmd.ExecuteReader())
                            {
                                if ((sdr != null) && (sdr.HasRows))
                                {
                                    productStagewiseReportList = new List<ProductStageWiseStockReport>();
                                    while (sdr.Read())
                                    {
                                        ProductStageWiseStockReport productStagewiseStockObj = new ProductStageWiseStockReport();
                                        {

                                            productStagewiseStockObj.ProductID = (sdr["PartID"].ToString() != "" ? Guid.Parse(sdr["PartID"].ToString()) : productStagewiseStockObj.ProductID);
                                            productStagewiseStockObj.Stage = (sdr["Stage"].ToString() != "" ? (sdr["Stage"].ToString()) : productStagewiseStockObj.Stage);
                                            productStagewiseStockObj.Description = (sdr["Name"].ToString() != "" ? (sdr["Name"].ToString()) : productStagewiseStockObj.Description);
                                            productStagewiseStockObj.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : productStagewiseStockObj.CurrentStock);
                                            productStagewiseStockObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : productStagewiseStockObj.TotalCount);
                                            productStagewiseStockObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : productStagewiseStockObj.FilteredCount);

                                        }
                                        productStagewiseReportList.Add(productStagewiseStockObj);
                                    }
                                }
                            }

                        }

                        else
                        {
                            cmd.Parameters.Add("@ProductID", SqlDbType.UniqueIdentifier).Value =Guid.Empty;
                    

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return productStagewiseReportList;
        }
        #endregion GetProductStageWiseStockReport


    }
}




