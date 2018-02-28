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

        public List<DocumentApproval> GetAllDocumentsPendingForApprovals(DocumentApprovalAdvanceSearch documentApprovalAdvanceSearch)
        {
            List<DocumentApproval> documentApprovalList = null;
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
                        cmd.CommandText = "[AMC].[GetAllDocumentsPendingForApprovals]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(documentApprovalAdvanceSearch.SearchTerm) ? "" : documentApprovalAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = documentApprovalAdvanceSearch.DataTablePaging.Start;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = documentApprovalAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = documentApprovalAdvanceSearch.ToDate;
                        cmd.Parameters.Add("@LoginName", SqlDbType.NVarChar,250).Value = documentApprovalAdvanceSearch.LoginName;
                        cmd.Parameters.Add("@ShowAll", SqlDbType.Bit).Value = documentApprovalAdvanceSearch.ShowAll;

                        if (documentApprovalAdvanceSearch.DocumentType.Code == "ALL")
                            cmd.Parameters.AddWithValue("@DocumentTypeCode", DBNull.Value);
                        else
                            cmd.Parameters.Add("@DocumentTypeCode", SqlDbType.VarChar,5).Value = documentApprovalAdvanceSearch.DocumentType.Code;

                        if (documentApprovalAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = documentApprovalAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                documentApprovalList = new List<DocumentApproval>();
                                while (sdr.Read())
                                {
                                    DocumentApproval documentApproval = new DocumentApproval();
                                    {
                                        //-----------
                                        documentApproval.ApprovalLogID = (sdr["ApprovalLogID"].ToString() != "" ? Guid.Parse(sdr["ApprovalLogID"].ToString()) : documentApproval.ApprovalLogID);
                                        documentApproval.ApproverID = (sdr["ApproverID"].ToString() != "" ? Guid.Parse(sdr["ApproverID"].ToString()) : documentApproval.ApproverID);
                                        documentApproval.UserID = (sdr["UserID"].ToString() != "" ? Guid.Parse(sdr["UserID"].ToString()) : documentApproval.UserID);
                                        documentApproval.LastApprovedUserID = (sdr["LastApprovedUserID"].ToString() != "" ? Guid.Parse(sdr["LastApprovedUserID"].ToString()) : documentApproval.LastApprovedUserID);
                                        documentApproval.DocumentID = (sdr["DocumentID"].ToString() != "" ? Guid.Parse(sdr["DocumentID"].ToString()) : documentApproval.DocumentID);

                                        documentApproval.DocumentType = (sdr["DocumentType"].ToString() != "" ? sdr["DocumentType"].ToString() : documentApproval.DocumentType);
                                        documentApproval.DocumentNo = (sdr["DocumentNo"].ToString() != "" ? sdr["DocumentNo"].ToString() : documentApproval.DocumentNo);

                                        documentApproval.DocumentDate = (sdr["DocumentDate"].ToString() != "" ? DateTime.Parse(sdr["DocumentDate"].ToString()) : documentApproval.DocumentDate);
                                        documentApproval.DocumentDateFormatted = (sdr["DocumentDate"].ToString() != "" ? DateTime.Parse(sdr["DocumentDate"].ToString()).ToString(settings.DateFormat) : documentApproval.DocumentDateFormatted);

                                        //-------------------
                                        documentApproval.StatusCode = (sdr["StatusCode"].ToString() != "" ? int.Parse(sdr["StatusCode"].ToString()) : documentApproval.StatusCode);
                                        documentApproval.DocumentStatus = (sdr["DocumentStatus"].ToString() != "" ? sdr["DocumentStatus"].ToString() : documentApproval.DocumentStatus);
                                        documentApproval.Approver = (sdr["Approver"].ToString() != "" ? sdr["Approver"].ToString() : documentApproval.Approver);
                                        documentApproval.ApproverLevel = (sdr["ApproverLevel"].ToString() != "" ? int.Parse(sdr["ApproverLevel"].ToString()) : documentApproval.ApproverLevel);
                                        documentApproval.DocumentCreatedBy = (sdr["DocumentCreatedBy"].ToString() != "" ? sdr["DocumentCreatedBy"].ToString() : documentApproval.DocumentCreatedBy);
                                        documentApproval.LatestDocumentStatus = (sdr["LatestDocumentStatus"].ToString() != "" ? (sdr["LatestDocumentStatus"].ToString()) : documentApproval.LatestDocumentStatus);                                        
                                       // documentApproval.IsNextApprover = (sdr["isNextApprover"].ToString() != "" ? bool.Parse(sdr["isNextApprover"].ToString()) : documentApproval.IsNextApprover);
                                        documentApproval.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : documentApproval.TotalCount);
                                        documentApproval.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : documentApproval.FilteredCount);

                                    }
                                    documentApprovalList.Add(documentApproval);
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

            return documentApprovalList;
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
