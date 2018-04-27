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

       
    }
}
