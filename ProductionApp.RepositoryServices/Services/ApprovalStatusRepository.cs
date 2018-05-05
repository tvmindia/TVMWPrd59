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
    public class ApprovalStatusRepository : IApprovalStatusRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public ApprovalStatusRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection
        #region GetApprovalStatusForSelectList
        /// <summary>
        /// To Get List of All Approval Status for Select List
        /// </summary>
        /// <returns>List</returns>
        public List<ApprovalStatus> GetApprovalStatusForSelectList()
        {
            List<ApprovalStatus> ApprovalStatusList = null;
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
                        cmd.CommandText = "[AMC].[GetApprovalStatusForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                ApprovalStatusList = new List<ApprovalStatus>();
                                while (sdr.Read())
                                {
                                    ApprovalStatus approvalStatus = new ApprovalStatus();
                                    {
                                        approvalStatus.ID = (sdr["ID"].ToString() != "" ? int.Parse(sdr["ID"].ToString()) : approvalStatus.ID);
                                        approvalStatus.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : approvalStatus.Description);
                                    }
                                    ApprovalStatusList.Add(approvalStatus);
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
            return ApprovalStatusList;
        }
        #endregion GetApprovalStatusForSelectList
    }
}
