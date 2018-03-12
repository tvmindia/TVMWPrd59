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
    public class MaterialReturnFromProductionRepository : IMaterialReturnFromProductionRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public MaterialReturnFromProductionRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllReturnFromProduction
        public List<MaterialReturnFromProduction> GetAllReturnFromProduction(MaterialReturnFromProductionAdvanceSearch materialReturnAdvanceSearch)
        {
            List<MaterialReturnFromProduction> materialReturnList = null;
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
                        cmd.CommandText = "[AMC].[GetAllReturnFromProduction]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(materialReturnAdvanceSearch.SearchTerm) ? "" : materialReturnAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = materialReturnAdvanceSearch.DataTablePaging.Start;
                        if (materialReturnAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = materialReturnAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = materialReturnAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = materialReturnAdvanceSearch.ToDate;
                        if (materialReturnAdvanceSearch.ReturnBy != Guid.Empty)
                            cmd.Parameters.Add("@ReceivedBy", SqlDbType.UniqueIdentifier).Value = materialReturnAdvanceSearch.ReturnBy;
                        if (materialReturnAdvanceSearch.ReceivedBy != Guid.Empty)
                            cmd.Parameters.Add("@ReturnBy", SqlDbType.UniqueIdentifier).Value = materialReturnAdvanceSearch.ReceivedBy;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialReturnList = new List<MaterialReturnFromProduction>();
                                while (sdr.Read())
                                {
                                    MaterialReturnFromProduction materiaReturn = new MaterialReturnFromProduction();
                                    {
                                        materiaReturn.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materiaReturn.ID);
                                        materiaReturn.ReturnNo = (sdr["ReturnNo"].ToString() != "" ? sdr["ReturnNo"].ToString() : materiaReturn.ReturnNo);
                                        materiaReturn.ReturnDate = (sdr["ReturnDate"].ToString() != "" ? DateTime.Parse(sdr["ReturnDate"].ToString()) : materiaReturn.ReturnDate);
                                        materiaReturn.ReturnDateFormatted = (sdr["ReturnDate"].ToString() != "" ? DateTime.Parse(sdr["ReturnDate"].ToString()).ToString(settings.DateFormat) : materiaReturn.ReturnDateFormatted);
                                        materiaReturn.RecievedByEmployeeName = (sdr["ReceivedByEmployeeName"].ToString() != "" ? sdr["ReceivedByEmployeeName"].ToString() : materiaReturn.RecievedByEmployeeName);
                                        materiaReturn.ReturnToEmployeeName = (sdr["ReturnByEmployeeName"].ToString() != "" ? sdr["ReturnByEmployeeName"].ToString() : materiaReturn.ReturnToEmployeeName);
                                        materiaReturn.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : materiaReturn.TotalCount);
                                        materiaReturn.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : materiaReturn.FilteredCount);
                                    }
                                    materialReturnList.Add(materiaReturn);
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
            return materialReturnList;
        }

        #endregion GetAllReturnFromProduction

        #region InsertUpdateReturnFromProduction
        public object InsertUpdateReturnFromProduction(MaterialReturnFromProduction materialReturnFromProduction)
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
                        cmd.CommandText = "[AMC].[InsertUpdateReturnFromProduction]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = materialReturnFromProduction.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = materialReturnFromProduction.ID;
                        cmd.Parameters.Add("@ReceivedBy", SqlDbType.UniqueIdentifier).Value = materialReturnFromProduction.ReceivedBy;
                        cmd.Parameters.Add("@ReturnBy", SqlDbType.UniqueIdentifier).Value = materialReturnFromProduction.ReturnBy;
                        cmd.Parameters.Add("@ReturnDate", SqlDbType.DateTime).Value = materialReturnFromProduction.ReturnDateFormatted;
                        cmd.Parameters.Add("@GeneralNotes", SqlDbType.VarChar).Value = materialReturnFromProduction.GeneralNotes;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = materialReturnFromProduction.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = materialReturnFromProduction.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = materialReturnFromProduction.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = materialReturnFromProduction.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = materialReturnFromProduction.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier, 5);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    switch (outputStatus.Value.ToString())
                    {
                        case "0":
                            throw new Exception(materialReturnFromProduction.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                        case "1":
                            return new
                            {
                                ID = IDOut.Value.ToString(),
                                Status = outputStatus.Value.ToString(),
                                Message = materialReturnFromProduction.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = materialReturnFromProduction.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateReturnFromProduction

        #region GetReturnFromProduction
        public MaterialReturnFromProduction GetReturnFromProduction(Guid id)
        {
            MaterialReturnFromProduction materialReturn = new MaterialReturnFromProduction();
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
                        cmd.CommandText = "[AMC].[GetReturnFromProduction]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    materialReturn.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materialReturn.ID);
                                    materialReturn.ReturnNo = (sdr["ReturnNo"].ToString() != "" ? sdr["ReturnNo"].ToString() : materialReturn.ReturnNo);
                                    materialReturn.ReturnDate = (sdr["ReturnDate"].ToString() != "" ? DateTime.Parse(sdr["ReturnDate"].ToString()) : materialReturn.ReturnDate);
                                    materialReturn.ReturnDateFormatted = (sdr["ReturnDate"].ToString() != "" ? DateTime.Parse(sdr["ReturnDate"].ToString()).ToString(settings.DateFormat) : materialReturn.ReturnDateFormatted);
                                    materialReturn.ReturnBy = (sdr["ReturnBy"].ToString() != "" ? Guid.Parse(sdr["ReturnBy"].ToString()) : materialReturn.ReturnBy);
                                    materialReturn.ReceivedBy = (sdr["ReceivedBy"].ToString() != "" ? Guid.Parse(sdr["ReceivedBy"].ToString()) : materialReturn.ReceivedBy);
                                    materialReturn.GeneralNotes = (sdr["GeneralNotes"].ToString() != "" ? sdr["GeneralNotes"].ToString() : materialReturn.GeneralNotes);
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
            return materialReturn;
        }
        #endregion GetReturnFromProduction

        #region GetReturnFromProductionDetail
        public List<MaterialReturnFromProductionDetail> GetReturnFromProductionDetail(Guid id)
        {
            List<MaterialReturnFromProductionDetail> materialReturnDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetReturnFromProductionDetail]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                materialReturnDetailList = new List<MaterialReturnFromProductionDetail>();
                                while (sdr.Read())
                                {
                                    MaterialReturnFromProductionDetail materialReturn = new MaterialReturnFromProductionDetail();
                                    {
                                        materialReturn.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : materialReturn.ID);
                                        materialReturn.HeaderID = (sdr["HeaderID"].ToString() != "" ? Guid.Parse(sdr["HeaderID"].ToString()) : materialReturn.HeaderID);
                                        materialReturn.MaterialID = (sdr["MaterialID"].ToString() != "" ? Guid.Parse(sdr["MaterialID"].ToString()) : materialReturn.MaterialID);
                                        materialReturn.MaterialDesc = (sdr["MaterialDesc"].ToString() != "" ? sdr["MaterialDesc"].ToString() : materialReturn.MaterialDesc);
                                        materialReturn.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : materialReturn.ToString());
                                        materialReturn.Qty = (sdr["Qty"].ToString() != "" ? Decimal.Parse(sdr["Qty"].ToString()) : materialReturn.Qty);
                                        materialReturn.Material = new Material();
                                        materialReturn.Material.MaterialCode = (sdr["MaterialCode"].ToString() != "" ? sdr["MaterialCode"].ToString() : materialReturn.Material.MaterialCode);
                                    }
                                    materialReturnDetailList.Add(materialReturn);
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
            return materialReturnDetailList;
        }
        #endregion ReturnFromProduction

        #region DeleteReturnFromProduction
        public object DeleteReturnFromProduction(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteReturnFromProduction]";
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
        #endregion DeleteReturnFromProduction

        #region DeleteReturnFromProductionDetail
        public object DeleteReturnFromProductionDetail(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteReturnFromProductionDetail]";
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
        #endregion DeleteReturnFromProductionDetail
    }
}
