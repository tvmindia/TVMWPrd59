using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.RepositoryServices.Services
{
    public class OtherExpenseRepository : IOtherExpenseRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public OtherExpenseRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #region GetAllOtherExpense
        public List<OtherExpense> GetAllOtherExpense(OtherExpenseAdvanceSearch otherExpenseAdvanceSearch)
        {
            List<OtherExpense> otherExpenseList = null;
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
                        cmd.CommandText = "[AMC].[GetAllOtherExpense]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(otherExpenseAdvanceSearch.SearchTerm) ? "" : otherExpenseAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = otherExpenseAdvanceSearch.DataTablePaging.Start;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = otherExpenseAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = otherExpenseAdvanceSearch.ToDate;
                        if (otherExpenseAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = otherExpenseAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                otherExpenseList = new List<OtherExpense>();
                                while (sdr.Read())
                                {
                                    OtherExpense otherExpense = new OtherExpense();
                                    {
                                        otherExpense.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : otherExpense.ID);
                                        otherExpense.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : otherExpense.EntryNo);
                                        otherExpense.ReversalRef = (sdr["ReversalRef"].ToString() != "" ? sdr["ReversalRef"].ToString() : otherExpense.ReversalRef);
                                        otherExpense.ExpenseDate = (sdr["ExpenseDate"].ToString() != "" ? DateTime.Parse(sdr["ExpenseDate"].ToString()) : otherExpense.ExpenseDate);
                                        otherExpense.ExpenseDateFormatted = (sdr["ExpenseDate"].ToString() != "" ? DateTime.Parse(sdr["ExpenseDate"].ToString()).ToString(settings.DateFormat) : otherExpense.ExpenseDateFormatted);
                                        otherExpense.Account = (sdr["Account"].ToString() != "" ? sdr["Account"].ToString() : otherExpense.Account);
                                        otherExpense.PaymentMode = (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : otherExpense.PaymentMode);
                                        otherExpense.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : otherExpense.Amount);
                                        otherExpense.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : otherExpense.ApprovalStatus);
                                        otherExpense.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : otherExpense.TotalCount);
                                        otherExpense.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : otherExpense.FilteredCount);
                                    }
                                    otherExpenseList.Add(otherExpense);
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

            return otherExpenseList;
        }
        #endregion GetAllOtherExpense

        #region InsertUpdateOtherExpense
        public object InsertUpdateOtherExpense(OtherExpense otherExpense)
        {
            SqlParameter outputStatus, OutputID, OutputCode;
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
                        cmd.CommandText = "[AMC].[InsertUpdateOtherExpense]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = otherExpense.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = otherExpense.ID;
                        cmd.Parameters.Add("@AccountCode", SqlDbType.VarChar).Value = otherExpense.AccountCode;
                        cmd.Parameters.Add("@AccountSubHead", SqlDbType.VarChar).Value = otherExpense.AccountSubHead;
                        cmd.Parameters.Add("PaymentMode", SqlDbType.VarChar).Value = otherExpense.PaymentMode;
                        cmd.Parameters.Add("@ExpenseDate", SqlDbType.DateTime).Value = otherExpense.ExpenseDateFormatted;
                        cmd.Parameters.Add("@ChequeDate", SqlDbType.DateTime).Value = otherExpense.ChequeDateFormatted;
                        cmd.Parameters.Add("@ChequeClearDate", SqlDbType.DateTime).Value = otherExpense.ChequeClearDateFormatted;
                        if (otherExpense.DepositWithdrawalID != Guid.Empty)
                            cmd.Parameters.Add("@DepWithdID", SqlDbType.UniqueIdentifier).Value = otherExpense.DepositWithdrawalID;
                        cmd.Parameters.Add("@BankCode", SqlDbType.VarChar, 5).Value = otherExpense.BankCode;
                        cmd.Parameters.Add("@ExpneseRef", SqlDbType.VarChar, 20).Value = otherExpense.ExpneseRef;
                        cmd.Parameters.Add("@ReversalRef", SqlDbType.VarChar, 20).Value = otherExpense.ReversalRef;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.NVarChar).Value = otherExpense.Description;
                        cmd.Parameters.Add("@Amount", SqlDbType.Decimal).Value = otherExpense.Amount;
                        cmd.Parameters.Add("@IsReverse", SqlDbType.Bit).Value = otherExpense.IsReverse;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar).Value = otherExpense.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = otherExpense.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar).Value = otherExpense.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = otherExpense.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        OutputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        OutputID.Direction = ParameterDirection.Output;
                        OutputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 10);
                        OutputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(otherExpense.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        otherExpense.ID = Guid.Parse(OutputID.Value.ToString());
                        return new
                        {
                            ID = Guid.Parse(OutputID.Value.ToString()),
                            Status = outputStatus.Value.ToString(),
                            Code = OutputCode.Value.ToString(),
                            Message = otherExpense.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Code = OutputCode.Value.ToString(),
                Message = otherExpense.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateOtherExpense

        #region GetOtherExpense
        public OtherExpense GetOtherExpense(Guid id)
        {
            OtherExpense otherExpense = null;

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
                        cmd.CommandText = "[AMC].[GetOtherExpense]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    otherExpense = new OtherExpense();
                                    otherExpense.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : otherExpense.ID);
                                    otherExpense.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : otherExpense.EntryNo);
                                    otherExpense.ExpenseDateFormatted = (sdr["ExpenseDate"].ToString() != "" ? DateTime.Parse(sdr["ExpenseDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : otherExpense.ExpenseDateFormatted);
                                    otherExpense.ExpenseDate = (sdr["ExpenseDate"].ToString() != "" ? DateTime.Parse(sdr["ExpenseDate"].ToString()) : otherExpense.ExpenseDate);
                                    otherExpense.ChequeDateFormatted = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : otherExpense.ChequeDateFormatted);
                                    otherExpense.ChequeDate = (sdr["ChequeDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeDate"].ToString()) : otherExpense.ChequeDate);
                                    otherExpense.ChequeClearDateFormatted = (sdr["ChequeClearDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeClearDate"].ToString()).ToString("dd-MMM-yyyy").ToString() : otherExpense.ChequeClearDateFormatted);
                                    otherExpense.ChequeClearDate = (sdr["ChequeClearDate"].ToString() != "" ? DateTime.Parse(sdr["ChequeClearDate"].ToString()) : otherExpense.ChequeClearDate);
                                    otherExpense.Amount = (sdr["Amount"].ToString() != "" ? Decimal.Parse(sdr["Amount"].ToString()) : otherExpense.Amount);
                                    otherExpense.ReversableAmount = (sdr["BalanceReversibleAmount"].ToString() != "" ? Decimal.Parse(sdr["BalanceReversibleAmount"].ToString()) : otherExpense.ReversableAmount);
                                    otherExpense.DepositWithdrawalID = (sdr["DepositWithdrawalID"].ToString() != "" ? Guid.Parse(sdr["DepositWithdrawalID"].ToString()) : otherExpense.DepositWithdrawalID);
                                    otherExpense.PaymentMode= (sdr["PaymentMode"].ToString() != "" ? sdr["PaymentMode"].ToString() : otherExpense.PaymentMode);
                                    otherExpense.BankCode = (sdr["BankCode"].ToString() != "" ? sdr["BankCode"].ToString() : otherExpense.BankCode);
                                    otherExpense.AccountSubHead = (sdr["AccountSubHead"].ToString() != "" ? sdr["AccountSubHead"].ToString() : otherExpense.AccountSubHead);
                                    otherExpense.AccountCode = (sdr["AccountCode"].ToString() != "" ? sdr["AccountCode"].ToString() : otherExpense.AccountCode);
                                    otherExpense.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : otherExpense.ApprovalStatus);
                                    otherExpense.ExpneseRef = (sdr["ExpneseRef"].ToString() != "" ? sdr["ExpneseRef"].ToString() : otherExpense.ExpneseRef);
                                    otherExpense.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : otherExpense.Description);
                                    otherExpense.ReversalRef = (sdr["ReversalRef"].ToString() != "" ? sdr["ReversalRef"].ToString() : otherExpense.ReversalRef);
                                    otherExpense.RequiredApprovalLimit = (sdr["RequiredApprovalLimit"].ToString() != "" ? Decimal.Parse(sdr["RequiredApprovalLimit"].ToString()) : otherExpense.RequiredApprovalLimit);
                                    otherExpense.LatestApprovalStatus = (sdr["LatestApprovalStatus"].ToString() != "" ? Int16.Parse(sdr["LatestApprovalStatus"].ToString()) : otherExpense.LatestApprovalStatus);
                                    otherExpense.LatestApprovalID = (sdr["LatestApprovalID"].ToString() != "" ? Guid.Parse(sdr["LatestApprovalID"].ToString()) : otherExpense.LatestApprovalID);
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
            return otherExpense;
        }
        #endregion GetOtherExpense

        #region GetAccountSubHeadForSelectList
        public List<SelectListItem> GetAccountSubHeadForSelectList()
        {
            List<SelectListItem> subheadList = new List<SelectListItem>(); ;
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
                        cmd.CommandText = "[AMC].[GetOtherExpenseAccountSubHeadForSelectList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    //subheadList = (sdr["AccountSubHead"].ToString() != "" ? sdr["AccountSubHead"].ToString(): subheadList);
                                    subheadList.Add(new SelectListItem
                                    { Text = (sdr["AccountSubHead"].ToString()), Value = (sdr["AccountSubHead"].ToString()) });
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
            return subheadList;
        }
        #endregion GetAccountSubHeadForSelectList

        #region GetReversalReference
        public List<OtherExpense> GetReversalReference(string accountCode)
        {
            List<OtherExpense> otherExpenseList = null;
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
                        cmd.CommandText = "[AMC].[GetReversalReference]";
                        cmd.Parameters.Add("@AccountCode", SqlDbType.VarChar).Value = accountCode;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                otherExpenseList = new List<OtherExpense>();
                                while (sdr.Read())
                                {
                                    OtherExpense otherExpense = new OtherExpense();
                                    {
                                        otherExpense.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : otherExpense.ID);
                                        otherExpense.EntryNo = (sdr["EntryNo"].ToString() != "" ? sdr["EntryNo"].ToString() : otherExpense.EntryNo);
                                        otherExpense.ExpenseDate = (sdr["ExpenseDate"].ToString() != "" ? DateTime.Parse(sdr["ExpenseDate"].ToString()) : otherExpense.ExpenseDate);
                                        otherExpense.ExpenseDateFormatted = (sdr["ExpenseDate"].ToString() != "" ? DateTime.Parse(sdr["ExpenseDate"].ToString()).ToString(settings.DateFormat) : otherExpense.ExpenseDateFormatted);
                                        otherExpense.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : otherExpense.Description);
                                        otherExpense.ReversalRef = (sdr["ReversalRef"].ToString() != "" ? sdr["ReversalRef"].ToString() : otherExpense.ReversalRef);
                                        otherExpense.Amount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : otherExpense.Amount);
                                        otherExpense.ReversableAmount = (sdr["ReversableAmount"].ToString() != "" ? decimal.Parse(sdr["ReversableAmount"].ToString()) : otherExpense.ReversableAmount);
                                    }
                                    otherExpenseList.Add(otherExpense);
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

            return otherExpenseList;
        }
        #endregion GetReversalReference

        #region DeleteOtherExpense
        public object DeleteOtherExpense(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteOtherExpense]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
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
        #endregion DeleteOtherExpense
    }
}
