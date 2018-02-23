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

        #region InsertUpdateIssueToProduction
        public object InsertUpdateIssueToProduction(MaterialIssue materialIssue)
        {
            SqlParameter outputStatus, IDOut = null;
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
                        cmd.CommandText = "[AMC].[InsertUpdateIssueToProduction]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = materialIssue.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = materialIssue.ID;                       
                        cmd.Parameters.Add("@IssueTo", SqlDbType.UniqueIdentifier).Value = materialIssue.IssueTo;
                        cmd.Parameters.Add("@IssuedBy", SqlDbType.UniqueIdentifier).Value = materialIssue.IssuedBy;
                        cmd.Parameters.Add("@IssueDate", SqlDbType.DateTime).Value = materialIssue.IssueDateFormatted;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar).Value = materialIssue.GeneralNotes;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = materialIssue.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = materialIssue.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = materialIssue.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = materialIssue.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = materialIssue.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier, 5);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    switch (outputStatus.Value.ToString())
                    {
                        case "0":
                            throw new Exception(materialIssue.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                        case "1":
                           // materialIssue.ID = Guid.Parse(outputCode.Value.ToString());
                            return new
                            {
                                ID = IDOut.Value.ToString(),
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
                Code = IDOut.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = materialIssue.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion

        #region GetIssueToProduction
        public MaterialIssue GetIssueToProduction(Guid ID)
        {
            MaterialIssue material = new MaterialIssue();
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
                        cmd.CommandText = "[AMC].[GetIssueToProduction]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    material.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : material.ID);
                                    material.IssueNo = (sdr["IssueNo"].ToString() != "" ? sdr["IssueNo"].ToString() : material.IssueNo);
                                    material.IssueDate = (sdr["IssueDate"].ToString() != "" ? DateTime.Parse(sdr["IssueDate"].ToString()) : material.IssueDate);
                                    material.IssueDateFormatted = (sdr["IssueDate"].ToString() != "" ? DateTime.Parse(sdr["IssueDate"].ToString()).ToString(settings.DateFormat) : material.IssueDateFormatted);
                                    material.IssuedBy = (sdr["IssuedBy"].ToString() != "" ? Guid.Parse(sdr["IssuedBy"].ToString()) : material.IssuedBy);
                                    material.IssueTo= (sdr["IssueTo"].ToString() != "" ? Guid.Parse(sdr["IssueTo"].ToString()) : material.IssueTo);
                                    material.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : material.GeneralNotes);
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
            return material;
        }
        #endregion

        #region GetIssueToPrductionDetail
        public List<MaterialIssueDetail> GetIssueToProductionDetail(Guid ID)
        {
            List<MaterialIssueDetail> materialIssueDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetIssueToProductionDetail]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialIssueDetailList = new List<MaterialIssueDetail>();
                                while(sdr.Read())
                                {
                                    MaterialIssueDetail materialIssue = new MaterialIssueDetail();
                                    {
                                        materialIssue.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materialIssue.ID);
                                        materialIssue.HeaderID= (sdr["HeaderID"].ToString() != "" ? Guid.Parse(sdr["HeaderID"].ToString()) : materialIssue.HeaderID);
                                        materialIssue.MaterialID= (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : materialIssue.MaterialID);
                                        materialIssue.MaterialDesc= (sdr["MaterialDesc"].ToString() != "" ? sdr["MaterialDesc"].ToString() : materialIssue.MaterialDesc);
                                        materialIssue.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : materialIssue.ToString());
                                        materialIssue.Qty= (sdr["Qty"].ToString() != "" ? Decimal.Parse(sdr["Qty"].ToString()) : materialIssue.Qty);
                                        materialIssue.Material = new Material();
                                        materialIssue.Material.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : materialIssue.Material.MaterialCode);
                                    }
                                    materialIssueDetailList.Add(materialIssue);
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return materialIssueDetailList;
            }
        #endregion

        #region DeleteIssueToProductionDetail
        public object DeleteIssueToProductionDetail(Guid ID)
        {
            SqlParameter outputStatus = null;
            try
            {
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if(con.State==ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[DeleteIssueToProductionDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value=ID;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return new
            {
                Status = outputStatus.Value.ToString(),
            };
        }
        #endregion

        #region DeleteIssueToProduction
        public object DeleteIssueToProduction(Guid ID)
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
                        cmd.CommandText = "[AMC].[DeleteIssueToProduction]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
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
        #endregion
    }

}

