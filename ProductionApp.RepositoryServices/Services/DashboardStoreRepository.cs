using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.RepositoryServices.Contracts;
using ProductionApp.DataAccessObject.DTO;
using System.Data.SqlClient;
using System.Data;

namespace ProductionApp.RepositoryServices.Services
{
    public class DashboardStoreRepository: IDashboardStoreRepository
    {
        Settings settings = new Settings();
        private IDatabaseFactory _databaseFactory;

        public DashboardStoreRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetRecentIssueSummary
        public List<MaterialIssue> GetRecentIssueSummary()
        {
            List<MaterialIssue> materialIssueList = new List<MaterialIssue>();
            MaterialIssue materialIssueHeader = null;
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
                        cmd.CommandText = "[AMC].[GetRecentIssueSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    materialIssueHeader = new MaterialIssue();
                                    materialIssueHeader.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);
                                    materialIssueHeader.IssueNo= (sdr["IssueNo"].ToString() != "" ? sdr["IssueNo"].ToString() : materialIssueHeader.IssueNo);
                                    materialIssueHeader.IssueTo= (sdr["IssueTo"].ToString() != "" ? Guid.Parse(sdr["IssueTo"].ToString()) : materialIssueHeader.IssueTo);
                                    materialIssueHeader.IssuedBy= (sdr["IssuedBy"].ToString() != "" ? Guid.Parse(sdr["IssuedBy"].ToString()) : materialIssueHeader.IssuedBy);
                                    materialIssueHeader.IssueDate = (sdr["IssueDate"].ToString() != "" ? DateTime.Parse(sdr["IssueDate"].ToString()).ToString(settings.DateFormat) : materialIssueHeader.IssueDate);

                                    materialIssueList.Add(materialIssueHeader);
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
            return materialIssueList;
        }
        #endregion GetRecentIssueSummary
    }
}
