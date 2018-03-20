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
                                        documentApproval.DocumentTypeCode = (sdr["DocumentTypeCode"].ToString() != "" ? sdr["DocumentTypeCode"].ToString() : documentApproval.DocumentTypeCode);
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
                                        approvalHistory.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : approvalHistory.Remarks);
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

        public object ApproveDocument(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode)
        {
            SqlParameter outputStatus= null;
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
                        cmd.CommandText = "[AMC].[ApproveDocument]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ApprovalLogID", SqlDbType.UniqueIdentifier).Value = ApprovalLogID;
                        cmd.Parameters.Add("@DocumentID", SqlDbType.UniqueIdentifier).Value = DocumentID;
                        cmd.Parameters.Add("@DocumentTypeCode", SqlDbType.VarChar, 250).Value = DocumentTypeCode;

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.ApprovalFailure);
                    case "1":
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.ApprovalSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConst.ApprovalSuccess
            };
        }

        public DataTable GetDocumentSummary(Guid DocumentID, string DocumentTypeCode) {
            try
            {
                DataTable result = new DataTable();
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[GetDocumentSummary]";
                        cmd.Parameters.Add("@DocumentID", SqlDbType.UniqueIdentifier).Value = DocumentID;
                        cmd.Parameters.Add("@DocumentTypeCode", SqlDbType.NVarChar, 5).Value = DocumentTypeCode;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            { 
                                    result.Load( sdr);
                                 
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public object RejectDocument(Guid ApprovalLogID, Guid DocumentID, string DocumentTypeCode, string Remarks)
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
                        cmd.CommandText = "[AMC].[RejectDocument]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ApprovalLogID", SqlDbType.UniqueIdentifier).Value = ApprovalLogID;
                        cmd.Parameters.Add("@DocumentID", SqlDbType.UniqueIdentifier).Value = DocumentID;
                        cmd.Parameters.Add("@DocumentTypeCode", SqlDbType.VarChar, 250).Value = DocumentTypeCode;
                        cmd.Parameters.Add("@Remarks", SqlDbType.VarChar,-1).Value = Remarks;

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.RejectFailure);
                    case "1":
                        return new
                        {
                            Status = outputStatus.Value.ToString(),
                            Message = _appConst.RejectSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConst.RejectSuccess
            };
        }

        public object ValidateDocumentsApprovalPermission(string LoginName, Guid DocumentID, string DocumentTypeCode)
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
                        cmd.CommandText = "[AMC].[ValidateDocumentsApprovalPermission]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@LoginName", SqlDbType.VarChar, 250).Value = LoginName;
                        cmd.Parameters.Add("@DocumentID", SqlDbType.UniqueIdentifier).Value = DocumentID;
                        cmd.Parameters.Add("@DocumentTypeCode", SqlDbType.VarChar, 5).Value = DocumentTypeCode;

                        outputStatus = cmd.Parameters.Add("@isValid", SqlDbType.Bit);
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

        public List<DocumentApprover> GetApproversByDocType(string docType)
        {
            List<DocumentApprover> sendForApprovalList = null;
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
                        cmd.CommandText = "[AMC].[GetApproversByDocType]";
                        cmd.Parameters.Add("@DocumentTypeCode", SqlDbType.NVarChar, 10).Value = docType;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                sendForApprovalList = new List<DocumentApprover>();
                                while (sdr.Read())
                                {
                                    DocumentApprover sendForApproval = new DocumentApprover();
                                    {
                                        sendForApproval.ApproverID = (sdr["ApproverID"].ToString() != "" ? Guid.Parse(sdr["ApproverID"].ToString()) : sendForApproval.ApproverID);
                                        sendForApproval.UserID = (sdr["UserID"].ToString() != "" ? Guid.Parse(sdr["UserID"].ToString()) : sendForApproval.UserID);
                                        sendForApproval.UserName = (sdr["UserName"].ToString() != "" ? sdr["UserName"].ToString() : sendForApproval.UserName);
                                        sendForApproval.EmailID = (sdr["EmailID"].ToString() != "" ? sdr["EmailID"].ToString() : sendForApproval.EmailID);
                                        sendForApproval.ApproverLevel = (sdr["ApproverLevel"].ToString() != "" ? Int32.Parse(sdr["ApproverLevel"].ToString()) : sendForApproval.ApproverLevel);
                                    }
                                    sendForApprovalList.Add(sendForApproval);
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

            return sendForApprovalList;
        }

        public object SendDocForApproval(Guid documentID, string documentTypeCode, string approvers, string createdBy, DateTime createdDate)
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
                        cmd.CommandText = "[AMC].[SendDocForApproval]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DocumentID", SqlDbType.UniqueIdentifier).Value = documentID;
                        cmd.Parameters.Add("@DocumentTypeCode", SqlDbType.VarChar, 5).Value = documentTypeCode;
                        cmd.Parameters.Add("@Approvers", SqlDbType.VarChar, -1).Value = approvers;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 250).Value = createdBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = createdDate;

                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.Bit);
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
                Message = _appConst.SendForApproval
            };

        }

        public object ReSendDocForApproval(Guid documentID, string documentTypeCode, Guid latestApprovalID, string createdBy, DateTime createdDate)
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
                        cmd.CommandText = "[AMC].[ReSendDocForApproval]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DocumentID", SqlDbType.UniqueIdentifier).Value = documentID;
                        cmd.Parameters.Add("@DocumentTypeCode", SqlDbType.VarChar, 5).Value = documentTypeCode;
                        cmd.Parameters.Add("@LatestApprovalID", SqlDbType.UniqueIdentifier).Value = latestApprovalID;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 250).Value = createdBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = createdDate;

                        outputStatus = cmd.Parameters.Add("@SPStatus", SqlDbType.Bit);
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
                Message = _appConst.SendForApproval
            };

        }




        public DocumentApprovalMailDetail GetApprovalMailDetails(Guid DocumentID, string DocumentTypeCode)
        {
            DocumentApprovalMailDetail documentMailDetail = new DocumentApprovalMailDetail();

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
                        cmd.CommandText = "[AMC].[GetApprovalMailDetails]";
                        cmd.Parameters.Add("@DocumentID", SqlDbType.UniqueIdentifier).Value = DocumentID;
                        cmd.Parameters.Add("@DocumentTypeCode", SqlDbType.NVarChar, 5).Value = DocumentTypeCode;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                               
                                if (sdr.Read())
                                {
                                    
                                    {
                                        documentMailDetail.NextApprover = (sdr["NextApprover"].ToString() != "" ? (sdr["NextApprover"].ToString()) : documentMailDetail.NextApprover);
                                        documentMailDetail.NextApproverEmail = (sdr["NextApproverEmail"].ToString() != "" ? (sdr["NextApproverEmail"].ToString()) : documentMailDetail.NextApproverEmail);
                                        documentMailDetail.DocumentNo = (sdr["DocumentNo"].ToString() != "" ? (sdr["DocumentNo"].ToString()) : documentMailDetail.DocumentNo);
                                        documentMailDetail.DocumentType = (sdr["DocumentType"].ToString() != "" ? (sdr["DocumentType"].ToString()) : documentMailDetail.DocumentType);
                                        documentMailDetail.DocumentOwner = (sdr["DocumentOwner"].ToString() != "" ? (sdr["DocumentOwner"].ToString()) : documentMailDetail.DocumentOwner);
                                        documentMailDetail.DocumnetOwnerMail = (sdr["DocumentOwnerEmail"].ToString() != "" ? (sdr["DocumentOwnerEmail"].ToString()) : documentMailDetail.DocumnetOwnerMail);
                                        documentMailDetail.Status = (sdr["DocumentStatus"].ToString() != "" ? (sdr["DocumentStatus"].ToString()) : documentMailDetail.Status);
                                        documentMailDetail.Remarks = (sdr["Remarks"].ToString() != "" ? (sdr["Remarks"].ToString()) : documentMailDetail.Remarks);
                                        documentMailDetail.ApprovalID = (sdr["ApprovalID"].ToString() != "" ? Guid.Parse(sdr["ApprovalID"].ToString()) : documentMailDetail.ApprovalID);

                                    }
                                    
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
            return documentMailDetail;


        }

        #region GetStockAdjustmentApprovalSummary
        public List<DocumentApproval> GetStockAdjApprovalSummary()
        {
            List<DocumentApproval> documentApprovalList = new List<DocumentApproval>();
            DocumentApproval documentApproval = null;
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
                        cmd.CommandText = "[AMC].[GetStockAdjApprovalSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    documentApproval = new DocumentApproval();
                                    documentApproval.DocumentID = (sdr["DocumentID"].ToString() != "" ? Guid.Parse(sdr["DocumentID"].ToString()) : Guid.Empty);
                                    documentApproval.DocumentType = (sdr["DocumentType"].ToString() != "" ? sdr["DocumentType"].ToString() : documentApproval.DocumentType);
                                    documentApproval.DocumentNo = (sdr["DocumentNo"].ToString() != "" ? sdr["DocumentNo"].ToString() : documentApproval.DocumentNo);
                                    documentApproval.DocumentDateFormatted = (sdr["DocumentDate"].ToString() != "" ? DateTime.Parse(sdr["DocumentDate"].ToString()).ToString(settings.DateFormat) : documentApproval.DocumentDateFormatted);
                                    documentApproval.DocumentStatus = (sdr["DocumentStatus"].ToString() != "" ? sdr["DocumentStatus"].ToString() : documentApproval.DocumentStatus);
                                    documentApproval.ApprovalLogID = (sdr["ApprovalLogID"].ToString() !=""? Guid.Parse(sdr["ApprovalLogID"].ToString()) : Guid.Empty);
                                    documentApproval.DocumentTypeCode = (sdr["DocumentTypeCode"].ToString() != "" ? sdr["DocumentTypeCode"].ToString() : documentApproval.DocumentTypeCode);
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

        #endregion
    }




}
