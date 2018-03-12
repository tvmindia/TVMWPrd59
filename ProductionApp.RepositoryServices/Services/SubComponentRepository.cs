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
    public class SubComponentRepository : ISubComponentRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();

        #region Constructor Injection
        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SubComponentRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetAllSubComponent
        /// <summary>
        /// To Get List of All SubComponent
        /// </summary>
        /// <param name="subComponentAdvanceSearch"></param>
        /// <returns>List</returns>
        public List<SubComponent> GetAllSubComponent(SubComponentAdvanceSearch subComponentAdvanceSearch)
        {
            List<SubComponent> subComponentList = null;
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
                        cmd.CommandText = "[AMC].[GetAllSubComponent]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(subComponentAdvanceSearch.SearchTerm) ? "" : subComponentAdvanceSearch.SearchTerm.Trim();
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = subComponentAdvanceSearch.DataTablePaging.Start;
                        if (subComponentAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = subComponentAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                subComponentList = new List<SubComponent>();
                                while (sdr.Read())
                                {
                                    SubComponent subComponent = new SubComponent();
                                    {
                                        subComponent.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : subComponent.ID);
                                        subComponent.Code = (sdr["Code"].ToString() != "" ? sdr["Code"].ToString() : subComponent.Code);
                                        subComponent.Description = (sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : subComponent.Description);
                                        subComponent.OpeningQty = (sdr["OpeningQty"].ToString() != "" ? decimal.Parse(sdr["OpeningQty"].ToString()) : subComponent.OpeningQty);
                                        subComponent.CurrentQty = (sdr["CurrentQty"].ToString() != "" ? decimal.Parse(sdr["CurrentQty"].ToString()) : subComponent.CurrentQty);
                                        subComponent.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : subComponent.UnitCode);
                                        subComponent.Unit = new Unit();
                                        subComponent.Unit.Description= (sdr["UnitDescription"].ToString() != "" ? sdr["UnitDescription"].ToString() : subComponent.Unit.Description);
                                        subComponent.WeightInKG = (sdr["WeightInKG"].ToString() != "" ? decimal.Parse(sdr["WeightInKG"].ToString()) : subComponent.WeightInKG);
                                        subComponent.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : subComponent.FilteredCount);
                                        subComponent.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : subComponent.TotalCount);
                                    }
                                    subComponentList.Add(subComponent);
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
            return subComponentList;
        }
        #endregion GetAllSubComponent

        //#region CheckSubComponentCodeExist
        ///// <summary>
        ///// To Check whether SubComponent Existing or not
        ///// </summary>
        ///// <param name="materialCode"></param>
        ///// <returns>bool</returns>
        //public bool CheckSubComponentCodeExist(string subComponentCode)
        //{
        //    try
        //    {
        //        using (SqlConnection con = _databaseFactory.GetDBConnection())
        //        {
        //            using (SqlCommand cmd = new SqlCommand())
        //            {
        //                if (con.State == ConnectionState.Closed)
        //                {
        //                    con.Open();
        //                }
        //                cmd.Connection = con;
        //                cmd.CommandText = "[AMC].[CheckSubComponentCodeExist]";
        //                cmd.Parameters.Add("@SubComponentCode", SqlDbType.VarChar).Value = subComponentCode;
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                Object res = cmd.ExecuteScalar();
        //                return (res.ToString() == "Exists" ? true : false);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //#endregion CheckSubComponentCodeExist

        #region InsertUpdateSubComponent
        /// <summary>
        /// To Insert and update SubComponent
        /// </summary>
        /// <param name="subComponent"></param>
        /// <returns>object</returns>
        public object InsertUpdateSubComponent(SubComponent subComponent)
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
                        cmd.CommandText = "[AMC].[InsertUpdateSubComponent]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = subComponent.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = subComponent.ID;
                        cmd.Parameters.Add("@Code", SqlDbType.VarChar, 10).Value = subComponent.Code;
                        cmd.Parameters.Add("@Description", SqlDbType.NVarChar,250).Value = subComponent.Description;
                        cmd.Parameters.Add("@OpeningQty", SqlDbType.Decimal).Value = subComponent.OpeningQty;
                        cmd.Parameters.Add("@CurrentQty", SqlDbType.Decimal).Value = subComponent.CurrentQty;
                        cmd.Parameters.Add("@UnitCode", SqlDbType.VarChar, 15).Value = subComponent.UnitCode;
                        cmd.Parameters.Add("@WeightInKG", SqlDbType.Decimal).Value = subComponent.WeightInKG;
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 50).Value = subComponent.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = subComponent.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.VarChar, 50).Value = subComponent.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = subComponent.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.Int);
                        outputStatus.Direction = ParameterDirection.Output;
                        OutputID = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier);
                        OutputID.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                }
                switch (outputStatus.Value.ToString())
                {
                    case "0":
                        throw new Exception(subComponent.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        subComponent.ID = Guid.Parse(OutputID.Value.ToString());
                        return new
                        {
                            ID = Guid.Parse(OutputID.Value.ToString()),
                            Status = outputStatus.Value.ToString(),
                            Message = subComponent.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = subComponent.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdateSubComponent

        #region GetSubComponent
        /// <summary>
        /// To Get SubComponent Details corresponding to ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Material</returns>
        public SubComponent GetSubComponent(Guid id)
        {
            SubComponent subComponent = null;

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
                        cmd.CommandText = "[AMC].[GetSubComponent]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    subComponent = new SubComponent();
                                    subComponent.ID = sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : subComponent.ID;
                                    subComponent.Code = sdr["Code"].ToString() != "" ? (sdr["Code"].ToString()) : subComponent.Code;
                                    subComponent.Description = sdr["Description"].ToString() != "" ? sdr["Description"].ToString() : subComponent.Description;
                                    subComponent.UnitCode = sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : subComponent.UnitCode;
                                    subComponent.Unit = new Unit();
                                    subComponent.Unit.Code= sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : subComponent.UnitCode;
                                    subComponent.OpeningQty = sdr["OpeningQty"].ToString() != "" ? decimal.Parse(sdr["OpeningQty"].ToString()) : subComponent.OpeningQty;
                                    subComponent.CurrentQty = sdr["CurrentQty"].ToString() != "" ? decimal.Parse(sdr["CurrentQty"].ToString()) : subComponent.CurrentQty;
                                    subComponent.WeightInKG = sdr["WeightInKG"].ToString() != "" ? decimal.Parse(sdr["WeightInKG"].ToString()) : subComponent.WeightInKG;
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
            return subComponent;
        }
        #endregion GetSubComponent

        #region DeleteSubComponent
        /// <summary>
        /// To Delete SubComponent
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public object DeleteSubComponent(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteSubComponent]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.Int);
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
        #endregion DeleteSubComponent

    }
}
