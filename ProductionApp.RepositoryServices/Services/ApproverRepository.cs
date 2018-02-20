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

        #region InsertUpdateApprover
        /// <summary>
        /// To Insert and update Approver
        /// </summary>
        /// <param name="approver"></param>
        /// <returns>object</returns>
        public object InsertUpdateApprover(Approver approver)
        {
            SqlParameter outputStatus, OutputID;
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
                        cmd.CommandText = "[AMC].[InsertUpdateApprover]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = approver.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = approver.ID;
                        cmd.Parameters.Add("@DocType", SqlDbType.VarChar).Value = approver.DocType;
                        cmd.Parameters.Add("@Level", SqlDbType.Decimal).Value = approver.Level;
                        cmd.Parameters.Add("@UserID", SqlDbType.VarChar).Value = approver.UserID;
                        cmd.Parameters.Add("@IsDefault", SqlDbType.VarChar).Value = approver.IsDefault;
                        cmd.Parameters.Add("@IsActive", SqlDbType.VarChar).Value = approver.IsActive;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = approver.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = approver.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = approver.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = approver.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        OutputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        OutputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(approver.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        approver.ID = Guid.Parse(OutputID.Value.ToString());
                        return new
                        {
                            ID = Guid.Parse(OutputID.Value.ToString()),
                            Status = outputStatus.Value.ToString(),
                            Message = approver.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                ID = Guid.Parse(OutputID.Value.ToString()),
                Status = outputStatus.Value.ToString(),
                Message = approver.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateApprover

        #region GetApprover
        /// <summary>
        /// To Get Approver Details corresponding to ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Approver</returns>
        public Approver GetApprover(Guid id)
        {
            Approver approver = null;

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
                        cmd.CommandText = "[AMC].[GetApprover]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    approver = new Approver();
                                    approver.ID = sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : approver.ID;
                                    approver.DocType = sdr["DocType"].ToString() != "" ? (sdr["DocType"].ToString()) : approver.DocType;
                                    approver.Level = sdr["Level"].ToString() != "" ? int.Parse(sdr["Level"].ToString()) : approver.Level;
                                    approver.UserID = sdr["UserID"].ToString() != "" ? Guid.Parse(sdr["UserID"].ToString()) : approver.UserID;
                                    //approver.IsDefault = new MaterialType();
                                    approver.IsDefault = sdr["IsDefault"].ToString() != "" ? bool.Parse(sdr["IsDefault"].ToString()) : approver.IsDefault;
                                    approver.IsActive = sdr["IsActive"].ToString() != "" ? bool.Parse(sdr["IsActive"].ToString()) : approver.IsActive;
                                   
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
            return approver;
        }
        #endregion GetApprover

    }
}
