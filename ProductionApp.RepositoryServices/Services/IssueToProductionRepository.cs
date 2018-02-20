using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;


namespace ProductionApp.RepositoryServices.Services
{
    public class IssueToProductionRepository : IIssueToProductionRepository
    {
        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();

        public IssueToProductionRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public object InsertUpdateIssueToProduction(MaterialIssue materialIssue)
        {
            SqlParameter outputStatus, outputCode = null;
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
                        cmd.CommandText ="[AMC].[InsertUpdateIssueToProduction]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpate", SqlDbType.Bit).Value = materialIssue.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = materialIssue.ID;
                        cmd.Parameters.Add("@IssueNo", SqlDbType.VarChar).Value = materialIssue.IssueNo;
                        cmd.Parameters.Add("@IssueTo", SqlDbType.VarChar).Value = materialIssue.IssueTo;
                        cmd.Parameters.Add("@IssuedBy", SqlDbType.VarChar).Value = materialIssue.IssuedBy;
                        cmd.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = materialIssue.IssueDate;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar).Value = materialIssue.GeneralNotes;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = materialIssue.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = materialIssue.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = materialIssue.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = materialIssue.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@IDOut", SqlDbType.VarChar, 5);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    switch (outputStatus.Value.ToString())
                    {
                        case "0":
                            throw new Exception(materialIssue.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                        case "1":
                            materialIssue.ID = Guid.Parse(outputCode.Value.ToString());
                            return new
                            {
                                Code = outputCode.Value.ToString(),
                                Status = outputStatus.Value.ToString(),
                                Message = materialIssue.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                            };
                    }
                }
                }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Code = "",//outputCode.Value.ToString(),
                Status = "1",//outputStatus.Value.ToString(),
                Message = materialIssue.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
    }
}
