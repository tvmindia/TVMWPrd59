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
    public class DocumentApprovalRepository: IDocumentApprovalRepository
    {
        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public DocumentApprovalRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<ApprovalHistory> GetApprovalHistory(Guid DocumentID, string DocumentTypeCode)
        {
            List<ApprovalHistory> approvalHistoryList = null;
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
                        cmd.CommandText = "[AMC].[GetDocumentApprovalHistory]";
                        cmd.Parameters.Add("@DocumentID", SqlDbType.UniqueIdentifier).Value = DocumentID;
                        cmd.Parameters.Add("@DocumentTypeCode", SqlDbType.NVarChar,5).Value = DocumentTypeCode;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                approvalHistoryList = new List<ApprovalHistory>();
                                while (sdr.Read())
                                {
                                    ApprovalHistory approvalHistory = new ApprovalHistory();
                                    {
                                        approvalHistory.ApproverID = (sdr["ApproverID"].ToString() != "" ? Guid.Parse(sdr["ApproverID"].ToString()) : approvalHistory.ApproverID);
                                        approvalHistory.ApprovalDate = (sdr["ApprovalDate"].ToString() != "" ? DateTime.Parse(sdr["ApprovalDate"].ToString()).ToString(settings.DateFormat) : approvalHistory.ApprovalDate);
                                        approvalHistory.ApproverLevel = (sdr["ApproverLevel"].ToString() != "" ? sdr["ApproverLevel"].ToString() : approvalHistory.ApproverLevel);
                                        approvalHistory.ApproverName = (sdr["ApproverName"].ToString() != "" ? sdr["ApproverName"].ToString() : approvalHistory.ApproverName);
                                        approvalHistory.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : approvalHistory.ApprovalStatus);
                                    }
                                    approvalHistoryList.Add(approvalHistory);
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
            return approvalHistoryList;


        }
    }
}
