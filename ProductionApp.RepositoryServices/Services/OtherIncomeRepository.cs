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
    public class OtherIncomeRepository: IOtherIncomeRepository
    {
        #region Constructor Injection
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public OtherIncomeRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetAllOtherIncome
        public List<OtherIncome> GetAllOtherIncome(OtherIncomeAdvanceSearch otherIncomeAdvanceSearch)
        {
            List<OtherIncome> otherIncomeList = null;
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
                        cmd.CommandText = "[AMC].[GetAllOtherIncome]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(otherIncomeAdvanceSearch.SearchTerm) ? "" : otherIncomeAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = otherIncomeAdvanceSearch.DataTablePaging.Start;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = otherIncomeAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = otherIncomeAdvanceSearch.ToDate;
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = otherIncomeAdvanceSearch.PaymentMode;
                        cmd.Parameters.Add("@AccountCode", SqlDbType.VarChar).Value = otherIncomeAdvanceSearch.ChartOfAccount.Code;
                        if (otherIncomeAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = otherIncomeAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                otherIncomeList = new List<OtherIncome>();
                                while (sdr.Read())
                                {
                                    OtherIncome otherIncome = new OtherIncome();
                                    {
                                        otherIncome.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : otherIncome.ID);
                                        otherIncome.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : otherIncome.EntryNo);
                                        otherIncome.IncomeDate = (sdr["IncomeDate"].ToString() != "" ? DateTime.Parse(sdr["IncomeDate"].ToString()) : otherIncome.IncomeDate);
                                        otherIncome.IncomeDateFormatted = (sdr["IncomeDate"].ToString() != "" ? DateTime.Parse(sdr["IncomeDate"].ToString()).ToString(settings.DateFormat) : otherIncome.IncomeDateFormatted);
                                        otherIncome.AccountCode = (sdr["Account"].ToString() != "" ? sdr["Account"].ToString() : otherIncome.AccountCode);
                                        otherIncome.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : otherIncome.PaymentMode);
                                        otherIncome.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : otherIncome.Amount);
                                        otherIncome.AccountSubHead = (sdr["AccountSubHead"].ToString() != "" ? sdr["AccountSubHead"].ToString() : otherIncome.AccountSubHead);
                                        otherIncome.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : otherIncome.TotalCount);
                                        otherIncome.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : otherIncome.FilteredCount);
                                    }
                                    otherIncomeList.Add(otherIncome);
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

            return otherIncomeList;
        }
        #endregion GetAllOtherIncome

        #region InsertUpdateOtherIncome
        public object InsertUpdateOtherIncome(OtherIncome otherIncome)
        {
            SqlParameter outputStatus, IDOut, DepositWithdrawalIDOut, EntryNoOut = null;
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
                        cmd.CommandText = "[AMC].[InsertUpdateOtherIncome]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = otherIncome.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = otherIncome.ID;
                        cmd.Parameters.Add("@EntryNo", SqlDbType.NVarChar, 20).Value = otherIncome.EntryNo;
                        cmd.Parameters.Add("@IncomeDate", SqlDbType.DateTime).Value = otherIncome.IncomeDateFormatted;
                        cmd.Parameters.Add("@AccountCode", SqlDbType.VarChar, 10).Value = otherIncome.AccountCode;//
                        cmd.Parameters.Add("@AccountSubHead", SqlDbType.VarChar, 50).Value = otherIncome.AccountSubHead;//
                        cmd.Parameters.Add("@PaymentMode", SqlDbType.VarChar, 10).Value = otherIncome.PaymentMode;//
                        if (otherIncome.DepositWithdrawalID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@DepositWithdrawalID", DBNull.Value);
                        else
                            cmd.Parameters.Add("@DepositWithdrawalID", SqlDbType.UniqueIdentifier).Value = otherIncome.DepositWithdrawalID;
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 5).Value = otherIncome.BankCode;//
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = otherIncome.ChequeDateFormatted;
                        cmd.Parameters.Add("@ReferenceBank", SqlDbType.NVarChar, 50).Value = otherIncome.ReferenceBank;//
                        cmd.Parameters.Add("@PaymentRef", SqlDbType.NVarChar, 50).Value = otherIncome.PaymentRef;//
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = otherIncome.Amount;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = otherIncome.Description;//

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 50).Value = otherIncome.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.SmallDateTime).Value = otherIncome.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 50).Value = otherIncome.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.SmallDateTime).Value = otherIncome.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        IDOut.Direction = ParameterDirection.Output;
                        DepositWithdrawalIDOut = cmd.Parameters.Add("@DepositWithdrawalIDOut", SqlDbType.UniqueIdentifier);
                        DepositWithdrawalIDOut.Direction = ParameterDirection.Output;
                        EntryNoOut = cmd.Parameters.Add("@EntryNoOut", SqlDbType.NVarChar, 20);
                        EntryNoOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(otherIncome.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        return new
                        {
                            ID = IDOut.Value.ToString(),
                            DepositWithdrawalID= DepositWithdrawalIDOut.Value.ToString(),
                            EntryNo= EntryNoOut.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = otherIncome.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new
            {
                ID = IDOut.Value.ToString(),
                DepositWithdrawalID= DepositWithdrawalIDOut.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = otherIncome.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateOtherIncome

        #region GetOtherIncome
        public OtherIncome GetOtherIncome(Guid id)
        {
            try
            {
                OtherIncome otherIncome = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[GetOtherIncome]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    otherIncome = new OtherIncome();
                                    otherIncome.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : otherIncome.ID);
                                    otherIncome.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : otherIncome.EntryNo);
                                    otherIncome.IncomeDate = (sdr["IncomeDate"].ToString() != "" ? DateTime.Parse(sdr["IncomeDate"].ToString()) : otherIncome.IncomeDate);
                                    otherIncome.IncomeDateFormatted = (sdr["IncomeDate"].ToString() != "" ? DateTime.Parse(sdr["IncomeDate"].ToString()).ToString(settings.DateFormat) : otherIncome.IncomeDateFormatted);
                                    otherIncome.AccountCode = (sdr["Account"].ToString() != "" ? sdr["Account"].ToString() : otherIncome.AccountCode);
                                    otherIncome.AccountSubHead = (sdr["AccountSubHead"].ToString() != "" ? sdr["AccountSubHead"].ToString() : otherIncome.AccountSubHead);
                                    otherIncome.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : otherIncome.PaymentMode);
                                    otherIncome.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : otherIncome.Amount);
                                    otherIncome.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : otherIncome.Description);
                                    otherIncome.BankCode = (sdr["BankCode"].ToString() != "" ? sdr["BankCode"].ToString() : otherIncome.BankCode);
                                    otherIncome.PaymentRef = (sdr["PaymentRef"].ToString() != "" ? sdr["PaymentRef"].ToString() : otherIncome.PaymentRef);
                                    otherIncome.ReferenceBank = (sdr["ReferenceBank"].ToString() != "" ? sdr["ReferenceBank"].ToString() : otherIncome.ReferenceBank);
                                    otherIncome.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()) : otherIncome.ChequeDate);
                                    otherIncome.ChequeDateFormatted = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString(settings.DateFormat) : otherIncome.ChequeDateFormatted);
                                    otherIncome.DepositWithdrawalID = (sdr["DepositWithdrawalID"].ToString() != "" ? Guid.Parse(sdr["DepositWithdrawalID"].ToString()) : otherIncome.DepositWithdrawalID);
                                }
                            }
                        }
                    }
                }
                return otherIncome;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion GetOtherIncome

        #region GetAllAccountSubHeadForSelectList
        public List<string> GetAllAccountSubHeadForSelectList()
        {
            List<string> subHeadList = null;
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
                        cmd.CommandText = "[AMC].[GetAllAccountSubHeadForSelectList]";
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                subHeadList = new List<string>();
                                while(sdr.Read())
                                {
                                    subHeadList.Add(sdr["AccountSubHead"].ToString());
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
            return subHeadList;
        }
        #endregion GetAllAccountSubHeadForSelectList

        #region DeleteOtherIncome
        public object DeleteOtherIncome(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteOtherIncome]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(_appConst.DeleteFailure);
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new
            {
                Status = outputStatus.Value.ToString(),
                Message = _appConst.DeleteSuccess
            };
        }
        #endregion DeleteOtherIncome
    }
}
