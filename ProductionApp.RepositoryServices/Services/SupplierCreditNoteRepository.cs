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
    public class SupplierCreditNoteRepository: ISupplierCreditNoteRepository
    {


        private IDatabaseFactory _databaseFactory;

        Settings settings = new Settings();
        AppConst _appConst = new AppConst();

        public SupplierCreditNoteRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }




        public object DeleteSupplierCreditNote(Guid ID, string userName)
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
                        cmd.CommandText = "[AMC].[DeleteSupplierCreditNote]";
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

        public List<SupplierCreditNote> GetAllSupplierCreditNote(SupplierCreditNoteAdvanceSearch supplierCreditNoteAdvanceSearch)
        {
            List<SupplierCreditNote> SupplierCreditNoteList = null;
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
                        cmd.CommandText = "[AMC].[GetAllSupplierCreditNote]";

                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(supplierCreditNoteAdvanceSearch.SearchTerm) ? "" : supplierCreditNoteAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = supplierCreditNoteAdvanceSearch.DataTablePaging.Start;
                        if (supplierCreditNoteAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = supplierCreditNoteAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = supplierCreditNoteAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = supplierCreditNoteAdvanceSearch.ToDate;
                        if (supplierCreditNoteAdvanceSearch.SupplierID != Guid.Empty)
                            cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = supplierCreditNoteAdvanceSearch.SupplierID;

                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                SupplierCreditNoteList = new List<SupplierCreditNote>();
                                while (sdr.Read())
                                {
                                    SupplierCreditNote SupplierCreditNote = new SupplierCreditNote();
                                    {
                                        SupplierCreditNote.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : SupplierCreditNote.ID);
                                        SupplierCreditNote.SupplierName = (sdr["SupplierName"].ToString() != "" ? sdr["SupplierName"].ToString() : SupplierCreditNote.SupplierName);
                                        SupplierCreditNote.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : SupplierCreditNote.SupplierID);
                                        SupplierCreditNote.CreditNoteNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : SupplierCreditNote.CreditNoteNo);
                                        SupplierCreditNote.CreditNoteDateFormatted = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()).ToString(settings.DateFormat) : SupplierCreditNote.CreditNoteDateFormatted);
                                        SupplierCreditNote.CreditAmount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : SupplierCreditNote.CreditAmount);
                                        // SupplierCreditNote.AvailableCredit = (sdr["AvailableCredit"].ToString() != "" ? decimal.Parse(sdr["AvailableCredit"].ToString()) : SupplierCreditNote.AvailableCredit);
                                        // _SupplierCreditNotesObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : _SupplierCreditNotesObj.Type);
                                        // _SupplierCreditNotesObj.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : _SupplierCreditNotesObj.GeneralNotes);
                                    }
                                    SupplierCreditNoteList.Add(SupplierCreditNote);
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

            return SupplierCreditNoteList;
        }

        public SupplierCreditNote GetSupplierCreditNote(Guid ID)
        {
            SupplierCreditNote SupplierCreditNote = new SupplierCreditNote();
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
                        cmd.CommandText = "[AMC].[GetSupplierCreditNote]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = ID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                                if (sdr.Read())
                                {
                                    // SupplierCreditNote.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : SupplierCreditNote.ID);
                                    SupplierCreditNote.SupplierName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : SupplierCreditNote.SupplierName);
                                    SupplierCreditNote.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : SupplierCreditNote.SupplierID);
                                    SupplierCreditNote.CreditNoteNo = (sdr["CRNRefNo"].ToString() != "" ? sdr["CRNRefNo"].ToString() : SupplierCreditNote.CreditNoteNo);
                                    SupplierCreditNote.CreditNoteDateFormatted = (sdr["CRNDate"].ToString() != "" ? DateTime.Parse(sdr["CRNDate"].ToString()).ToString(settings.DateFormat) : SupplierCreditNote.CreditNoteDateFormatted);
                                    SupplierCreditNote.CreditAmount = (sdr["Amount"].ToString() != "" ? decimal.Parse(sdr["Amount"].ToString()) : SupplierCreditNote.CreditAmount);
                                    //_SupplierCreditNoteObj.Type = (sdr["Type"].ToString() != "" ? sdr["Type"].ToString() : _SupplierCreditNoteObj.Type);
                                    //    SupplierCreditNote.adjustedAmount = (sdr["AdjAmount"].ToString() != "" ? decimal.Parse(sdr["AdjAmount"].ToString()) : SupplierCreditNote.adjustedAmount);
                                    SupplierCreditNote.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : SupplierCreditNote.GeneralNotes);

                                }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return SupplierCreditNote;

        }

        public object InsertUpdateSupplierCreditNote(SupplierCreditNote SupplierCreditNote)
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
                        cmd.CommandText = "[AMC].[InsertUpdateSupplierCreditNote]";

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = SupplierCreditNote.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = SupplierCreditNote.ID;
                        if (SupplierCreditNote.SupplierID != Guid.Empty)
                            cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = SupplierCreditNote.SupplierID;
                        //  cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = SupplierCreditNote.hdnFileID;
                        //cmd.Parameters.Add("@Type", SqlDbType.VarChar, 2).Value = SupplierCreditNote.Type;
                        cmd.Parameters.Add("@CreditNoteDate", SqlDbType.DateTime).Value = SupplierCreditNote.CreditNoteDateFormatted;
                        cmd.Parameters.Add("@CreditAmount", SqlDbType.Decimal).Value = SupplierCreditNote.CreditAmount;

                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar, -1).Value = SupplierCreditNote.GeneralNotes;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = SupplierCreditNote.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = SupplierCreditNote.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = SupplierCreditNote.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = SupplierCreditNote.Common.UpdatedDate;
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
                        throw new Exception(SupplierCreditNote.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        return new
                        {
                            ID = IDOut.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = SupplierCreditNote.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = SupplierCreditNote.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
    }
}
