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
    public class BankRepository:IBankRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public BankRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<Bank> GetAllBank(BankAdvanceSearch bankAdvanceSearch)
        {
            List<Bank> bankList = null;
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
                        cmd.CommandText = "[AMC].[GetAllBank]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(bankAdvanceSearch.SearchTerm)?"": bankAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = bankAdvanceSearch.DataTablePaging.Start;
                        if (bankAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = bankAdvanceSearch.DataTablePaging.Length;
                        //cmd.Parameters.Add("@OrderDir", SqlDbType.NVarChar, 5).Value = model.order[0].dir;
                        //cmd.Parameters.Add("@OrderColumn", SqlDbType.NVarChar, -1).Value = model.order[0].column;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                bankList = new List<Bank>();
                                while (sdr.Read())
                                {
                                    Bank bank = new Bank();
                                    {
                                        bank.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : bank.Code);
                                        bank.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : bank.Name);
                                        bank.Opening= (sdr["Opening"].ToString() != "" ? decimal.Parse(sdr["Opening"].ToString()) : bank.Opening);
                                        bank.ActualODLimit = (sdr["ActualODLimit"].ToString() != "" ? decimal.Parse(sdr["ActualODLimit"].ToString()) : bank.ActualODLimit);
                                        bank.DisplayODLimit = (sdr["DisplayODLimit"].ToString() != "" ? decimal.Parse(sdr["DisplayODLimit"].ToString()) : bank.DisplayODLimit);
                                        bank.TotalCount= (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : bank.TotalCount);
                                        bank.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : bank.FilteredCount);
                                    }
                                    bankList.Add(bank);
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

            return bankList;
        }
        public Bank GetBank(string code)
        {
            Bank bank = null;
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
                        cmd.CommandText = "[AMC].[GetBank]";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar, 10).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    bank = new Bank();
                                    bank.Code = (sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : bank.Code);
                                    bank.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : bank.Name);
                                    bank.Opening= (sdr["Opening"].ToString() != "" ? decimal.Parse(sdr["Opening"].ToString()) : bank.Opening);
                                    bank.ActualODLimit = (sdr["ActualODLimit"].ToString() != "" ? decimal.Parse(sdr["ActualODLimit"].ToString()) : bank.ActualODLimit);
                                    bank.DisplayODLimit = (sdr["DisplayODLimit"].ToString() != "" ? decimal.Parse(sdr["DisplayODLimit"].ToString()) : bank.DisplayODLimit);
                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return bank;
        }
        public bool CheckCodeExist(string code)
        {
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
                        cmd.CommandText = "[AMC].[CheckCodeExist]";
                        cmd.Parameters.Add("@Code", SqlDbType.NVarChar).Value = code;
                        cmd.CommandType = CommandType.StoredProcedure;
                        Object res = cmd.ExecuteScalar();
                        return (res.ToString() == "Exists" ? true : false);
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region InsertUpdateBank
        public object InsertUpdateBank(Bank bank)
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
                        cmd.CommandText = "[AMC].[InsertUpdateBank]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = bank.IsUpdate;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 5).Value = bank.Code;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = bank.Name;
                        cmd.Parameters.Add("@ActualODLimit", SqlDbType.Decimal).Value = bank.ActualODLimit;
                        cmd.Parameters.Add("@DisplayODLimit", SqlDbType.Decimal).Value = bank.DisplayODLimit;
                        cmd.Parameters.Add("@Opening", SqlDbType.Decimal).Value = bank.Opening;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = bank.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = bank.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = bank.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = bank.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        outputCode = cmd.Parameters.Add("@CodeOut", SqlDbType.VarChar, 5);
                        outputCode.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":                       
                        throw new Exception(_appConst.InsertFailure);
                    case "1":
                        bank.Code = outputCode.Value.ToString();
                        return new
                        {
                            Code = outputCode.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = bank.IsUpdate?_appConst.UpdateSuccess:_appConst.InsertSuccess
                        };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return new
            {
                Code = outputCode.Value.ToString(),
                Status = outputStatus.Value.ToString(),
                Message = bank.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateBank    
        #region DeleteBank
        public object DeleteBank(string code)
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
                        cmd.CommandText = "[Accounts].[DeleteBank]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 5).Value = code;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
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
        #endregion DeleteBank
    }
}
