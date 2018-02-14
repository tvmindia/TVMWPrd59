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
            MaterialIssue materialIssue = null;
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
                                    materialIssue = new MaterialIssue();
                                    materialIssue.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);
                                    materialIssue.IssueNo= (sdr["IssueNo"].ToString() != "" ? sdr["IssueNo"].ToString() : materialIssue.IssueNo);
                                    materialIssue.IssueTo= (sdr["IssueTo"].ToString() != "" ? Guid.Parse(sdr["IssueTo"].ToString()) : materialIssue.IssueTo);
                                    materialIssue.IssuedBy= (sdr["IssuedBy"].ToString() != "" ? Guid.Parse(sdr["IssuedBy"].ToString()) : materialIssue.IssuedBy);
                                    materialIssue.IssueDate = (sdr["IssueDate"].ToString() != "" ? DateTime.Parse(sdr["IssueDate"].ToString()).ToString(settings.DateFormat) : materialIssue.IssueDate);
                                    materialIssue.IssuedByEmployee = new Employee();
                                    materialIssue.IssueToEmployee = new Employee();
                                    materialIssue.IssuedByEmployee.Name= (sdr["IssuedByName"].ToString() != "" ? sdr["IssuedByName"].ToString() : materialIssue.IssuedByEmployee.Name);
                                    materialIssue.IssueToEmployee.Name = (sdr["IssueToName"].ToString() != "" ? sdr["IssueToName"].ToString() : materialIssue.IssueToEmployee.Name);
                                    materialIssueList.Add(materialIssue);
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
