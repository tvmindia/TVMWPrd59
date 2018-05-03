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
                        cmd.CommandText = "[AMC].[GetRequisitionSummaryReport]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(requisitionSummaryReport.SearchTerm) ? "" : requisitionSummaryReport.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = requisitionSummaryReport.DataTablePaging.Start;
                        //if (requisitionSummaryReport.DataTablePaging.Length == -1)
                        //    cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        //else
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
                                        requisitionObj.ReqNo = (sdr["ReqNo"].ToString() != "" ? sdr["ReqNo"].ToString() : requisitionObj.ReqNo);
                                        requisitionObj.ReqDate = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()) : requisitionObj.ReqDate);
                                        requisitionObj.ReqDateFormatted = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()).ToString(settings.DateFormat) : requisitionObj.ReqDateFormatted);
                                        requisitionObj.Title = (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : requisitionObj.Title);
                                        requisitionObj.ReqStatus = (sdr["ReqStatus"].ToString() != "" ? sdr["ReqStatus"].ToString() : requisitionObj.ReqStatus);
                                        //requisitionObj.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : requisitionObj.ApprovalStatus);
                                        requisitionObj.RequisitionBy = (sdr["RequisitionBy"].ToString() != "" ? sdr["RequisitionBy"].ToString() : requisitionObj.RequisitionBy);
                                        requisitionObj.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : requisitionObj.TotalCount);
                                        requisitionObj.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : requisitionObj.FilteredCount);
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
    }
}
