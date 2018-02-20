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
    public class ApproverRepository:IApproverRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ApproverRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetAllApprover
        /// <summary>
        /// To Get List of All Approver
        /// </summary>
        /// <param name="approverAdvanceSearch"></param>
        /// <returns>List</returns>
        public List<Approver> GetAllApprover(ApproverAdvanceSearch approverAdvanceSearch)
        {
            List<Approver> approverList = null;
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
                        cmd.CommandText = "[AMC].[GetAllApprover]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(approverAdvanceSearch.SearchTerm) ? "" : approverAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = approverAdvanceSearch.DataTablePaging.Start;
                        if (approverAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = approverAdvanceSearch.DataTablePaging.Length;
                        //cmd.Parameters.Add("@OrderDir", SqlDbType.NVarChar, 5).Value = model.order[0].dir;
                        //cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar, -1).Value = model.order[0].column;
                        //
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                approverList = new List<Approver>();
                                while (sdr.Read())
                                {
                                    Approver approver = new Approver();
                                    {
                                        approver.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : approver.ID);
                                        approver.DocType = (sdr["DocType"].ToString() != "" ? sdr["DocType"].ToString() : approver.DocType);
                                        approver.Level = (sdr["Level"].ToString() != "" ? int.Parse(sdr["Level"].ToString()) : approver.Level);
                                        approver.UserID = (sdr["UserID"].ToString() != "" ? Guid.Parse(sdr["UserID"].ToString()) : approver.UserID);
                                        approver.User = new User();
                                        approver.User.LoginName = (sdr["LoginName"].ToString() != "" ? sdr["LoginName"].ToString() : approver.User.LoginName);
                                        approver.IsDefault = (sdr["IsDefault"].ToString() != "" ? bool.Parse(sdr["IsDefault"].ToString()) : approver.IsDefault);
                                        approver.IsActive = (sdr["IsActive"].ToString() != "" ? bool.Parse(sdr["IsActive"].ToString()) : approver.IsActive);
                                        approver.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : approver.FilteredCount);
                                        approver.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : approver.TotalCount);
                                    }
                                    approverList.Add(approver);
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
            return approverList;
        }
        #endregion GetAllApprover

    }
}
