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
    }
}
