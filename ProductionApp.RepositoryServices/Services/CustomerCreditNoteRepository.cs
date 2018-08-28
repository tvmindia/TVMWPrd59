using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductionApp.DataAccessObject.DTO;
using System.Data.SqlClient;
using System.Data;

namespace ProductionApp.RepositoryServices.Services
{
    public class CustomerCreditNoteRepository : ICustomerCreditNoteRepository
    {
        private IDatabaseFactory _databaseFactory;

        Settings settings = new Settings();
        AppConst _appConst = new AppConst();

        public CustomerCreditNoteRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }


        public object DeleteCustomerCreditNote(Guid ID, string userName)
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
                        cmd.CommandText = "[AMC].[DeleteCustomerCreditNote]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        // cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = userName;
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

        public List<CustomerCreditNote> GetAllCustomerCreditNote(CustomerCreditNoteAdvanceSearch customerCreditNoteAdvanceSearch)
        {
            List<CustomerCreditNote> customerCreditNoteList = null;
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
                        cmd.CommandText = "[AMC].[GetAllCustomerCreditNote]";

                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(customerCreditNoteAdvanceSearch.SearchTerm) ? "" : customerCreditNoteAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = customerCreditNoteAdvanceSearch.DataTablePaging.Start;
                        if (customerCreditNoteAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = customerCreditNoteAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = customerCreditNoteAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = customerCreditNoteAdvanceSearch.ToDate;
                        if (customerCreditNoteAdvanceSearch.CustomerID != Guid.Empty)
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = customerCreditNoteAdvanceSearch.CustomerID;


                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                customerCreditNoteList = new List<CustomerCreditNote>();
                                while (sdr.Read())
                                {
                                    CustomerCreditNote customerCreditNote = new CustomerCreditNote();
                                    {
                                        customerCreditNote.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customerCreditNote.ID);
                                        customerCreditNote.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : customerCreditNote.CustomerName);
                                        customerCreditNote.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : customerCreditNote.CustomerID);
                                        customerCreditNote.CreditNoteNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : customerCreditNote.CreditNoteNo);
                                        customerCreditNote.CreditNoteDateFormatted = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()).ToString(settings.DateFormat) : customerCreditNote.CreditNoteDateFormatted);
                                        customerCreditNote.CreditAmount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : customerCreditNote.CreditAmount);
                                      //  customerCreditNote.AvailableCredit = (sdr["AvailableCredit"].ToString() != "" ? decimal.Parse(sdr["AvailableCredit"].ToString()) : customerCreditNote.AvailableCredit);
                                        //  _customerCreditNotesObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : _customerCreditNotesObj.Type);
                                        //  _customerCreditNotesObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _customerCreditNotesObj.GeneralNotes);
                                    }
                                    customerCreditNoteList.Add(customerCreditNote);
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

            return customerCreditNoteList;
        }

        public CustomerCreditNote GetCustomerCreditNote(Guid ID)
        {
               CustomerCreditNote customerCreditNote = new CustomerCreditNote();
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
                        cmd.CommandText = "[AMC].[GetCustomerCreditNote]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                   // customerCreditNote.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : customerCreditNote.ID);
                                    customerCreditNote.CustomerName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : customerCreditNote.CustomerName);
                                    customerCreditNote.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : customerCreditNote.CustomerID);
                                    customerCreditNote.CreditNoteNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : customerCreditNote.CreditNoteNo);
                                    customerCreditNote.CreditNoteDateFormatted = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()).ToString(settings.DateFormat) : customerCreditNote.CreditNoteDateFormatted);
                                    customerCreditNote.CreditAmount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : customerCreditNote.CreditAmount);
                                    //_customerCreditNoteObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : _customerCreditNoteObj.Type);
                                //    customerCreditNote.adjustedAmount = (sdr["AdjAmount"].ToString() != "" ? decimal.Parse(sdr["AdjAmount"].ToString()) : customerCreditNote.adjustedAmount);
                                    customerCreditNote.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : customerCreditNote.GeneralNotes);

                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return customerCreditNote;
        
        }

        public object InsertUpdateCustomerCreditNote(CustomerCreditNote customerCreditNote)
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
                        cmd.CommandText = "[AMC].[InsertUpdateCustomerCreditNote]";

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = customerCreditNote.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = customerCreditNote.ID;
                        if (customerCreditNote.CustomerID != Guid.Empty)
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = customerCreditNote.CustomerID;
                      //  cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = customerCreditNote.hdnFileID;
                        //cmd.Parameters.Add("@Type", SqlDbType.VarChar, 2).Value = customerCreditNote.Type;
                        cmd.Parameters.Add("@CreditNoteDate", SqlDbType.DateTime).Value = customerCreditNote.CreditNoteDateFormatted;
                        cmd.Parameters.Add("@CreditAmount", SqlDbType.Decimal).Value = customerCreditNote.CreditAmount;

                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar, -1).Value = customerCreditNote.GeneralNotes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = customerCreditNote.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = customerCreditNote.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = customerCreditNote.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = customerCreditNote.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }

                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(customerCreditNote.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        return new
                        {
                            ID = IDOut.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = customerCreditNote.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
                        };
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
                Message = customerCreditNote.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
    }
}
