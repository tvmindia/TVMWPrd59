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
    public class PackingSlipRepository : IPackingSlipRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public PackingSlipRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetAllPackingSlip
        public List<PackingSlip> GetAllPackingSlip(PackingSlipAdvanceSearch paySlipAdvanceSearch)
        {
            List<PackingSlip> paySlipList = null;
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
                        cmd.CommandText = "[AMC].[GetAllPackingSlip]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(paySlipAdvanceSearch.SearchTerm) ? "" : paySlipAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = paySlipAdvanceSearch.DataTablePaging.Start;
                        if (paySlipAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = paySlipAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@PackingFromDate", SqlDbType.DateTime).Value = paySlipAdvanceSearch.PackingFromDate;
                        cmd.Parameters.Add("@PackingToDate", SqlDbType.DateTime).Value = paySlipAdvanceSearch.PackingToDate;
                        cmd.Parameters.Add("@DispatchedFromDate", SqlDbType.DateTime).Value = paySlipAdvanceSearch.DispatchedFromDate;
                        cmd.Parameters.Add("@DispatchedToDate", SqlDbType.DateTime).Value = paySlipAdvanceSearch.DispatchedToDate;
                        if (paySlipAdvanceSearch.PackedBy != Guid.Empty)
                            cmd.Parameters.Add("@PackedBy", SqlDbType.UniqueIdentifier).Value = paySlipAdvanceSearch.PackedBy;
                        if (paySlipAdvanceSearch.DispatchedBy != Guid.Empty)
                            cmd.Parameters.Add("@DispatchedBy", SqlDbType.UniqueIdentifier).Value = paySlipAdvanceSearch.DispatchedBy;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                paySlipList = new List<PackingSlip>();
                                while (sdr.Read())
                                {
                                    PackingSlip paySlip = new PackingSlip();
                                    {
                                        paySlip.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : paySlip.ID);
                                        paySlip.SlipNo = (sdr["SlipNo"].ToString() != "" ? sdr["SlipNo"].ToString() : paySlip.SlipNo);
                                        paySlip.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()) : paySlip.Date);
                                        paySlip.DateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.DateFormat) : paySlip.DateFormatted);
                                        paySlip.DispatchedDate = (sdr["DispatchedDate"].ToString() != "" ? DateTime.Parse(sdr["DispatchedDate"].ToString()) : paySlip.DispatchedDate);
                                        paySlip.DispatchedDateFormatted = (sdr["DispatchedDate"].ToString() != "" ? DateTime.Parse(sdr["DispatchedDate"].ToString()).ToString(settings.DateFormat) : paySlip.DispatchedDateFormatted);
                                        paySlip.DispatchedByEmployeeName = (sdr["DispatchedByEmployeeName"].ToString() != "" ? sdr["DispatchedByEmployeeName"].ToString() : paySlip.DispatchedByEmployeeName);
                                        paySlip.PackedByEmployeeName = (sdr["PackedByEmployeeName"].ToString() != "" ? sdr["PackedByEmployeeName"].ToString() : paySlip.PackedByEmployeeName);
                                        paySlip.DispatchRemarks = (sdr["DispatchRemarks"].ToString() != "" ? sdr["DispatchRemarks"].ToString() : paySlip.DispatchRemarks);
                                        paySlip.DriverName = (sdr["DriverName"].ToString() != "" ? sdr["DriverName"].ToString() : paySlip.DriverName);
                                        paySlip.PackingRemarks = (sdr["PackingRemarks"].ToString() != "" ? sdr["PackingRemarks"].ToString() : paySlip.PackingRemarks);
                                        paySlip.VehiclePlateNo = (sdr["VehiclePlateNo"].ToString() != "" ? sdr["VehiclePlateNo"].ToString() : paySlip.VehiclePlateNo);
                                        paySlip.ReceivedBy = (sdr["ReceivedBy"].ToString() != "" ? sdr["ReceivedBy"].ToString() : paySlip.ReceivedBy);
                                        paySlip.ReceivedDate = (sdr["ReceivedDate"].ToString() != "" ? DateTime.Parse(sdr["ReceivedDate"].ToString()) : paySlip.ReceivedDate);
                                        paySlip.SalesOrderID = (sdr["SalesOrderID"].ToString() != "" ? Guid.Parse(sdr["SalesOrderID"].ToString()) : paySlip.SalesOrderID);
                                        paySlip.CheckedPackageWeight = (sdr["CheckedPackageWeight"].ToString() != "" ? decimal.Parse(sdr["CheckedPackageWeight"].ToString()) : paySlip.CheckedPackageWeight);
                                        paySlip.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : paySlip.TotalCount);
                                        paySlip.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : paySlip.FilteredCount);
                                    }
                                    paySlipList.Add(paySlip);
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
            return paySlipList;
        }

        #endregion GetAllPackingSlip

        #region InsertUpdatePackingSlip
        public object InsertUpdatePackingSlip(PackingSlip packingSlip)
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
                        cmd.CommandText = "[AMC].[InsertUpdatePackingSlip]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = packingSlip.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = packingSlip.ID;
                        cmd.Parameters.Add("@SalesOrderID", SqlDbType.UniqueIdentifier).Value = packingSlip.SalesOrderID;
                        cmd.Parameters.Add("@PackedBy", SqlDbType.UniqueIdentifier).Value = packingSlip.PackedBy;
                        if (packingSlip.DispatchedBy == Guid.Empty)
                            cmd.Parameters.Add("@DispatchedBy", SqlDbType.UniqueIdentifier).Value = null;
                        else
                            cmd.Parameters.Add("@DispatchedBy", SqlDbType.UniqueIdentifier).Value = packingSlip.DispatchedBy;
                        cmd.Parameters.Add("@DispatchedDate", SqlDbType.DateTime).Value = packingSlip.DispatchedDateFormatted;
                        cmd.Parameters.Add("@IssueToDispatchDate", SqlDbType.DateTime).Value = packingSlip.IssueToDispatchDate;
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = packingSlip.Date;
                        cmd.Parameters.Add("@ReceivedDate", SqlDbType.DateTime).Value = packingSlip.ReceivedDateFormatted;
                        cmd.Parameters.Add("@ReceivedBy", SqlDbType.VarChar).Value = packingSlip.ReceivedBy;
                        cmd.Parameters.Add("@TotalPackageWeight", SqlDbType.Decimal).Value = packingSlip.TotalPackageWeight;
                        cmd.Parameters.Add("@CheckedPackageWeight", SqlDbType.Decimal).Value = packingSlip.CheckedPackageWeight;
                        cmd.Parameters.Add("@VehiclePlateNo", SqlDbType.VarChar).Value = packingSlip.VehiclePlateNo;
                        cmd.Parameters.Add("@DriverName", SqlDbType.VarChar).Value = packingSlip.DriverName;
                        cmd.Parameters.Add("@DispatchRemarks", SqlDbType.VarChar).Value = packingSlip.DispatchRemarks;
                        cmd.Parameters.Add("@PackingRemarks", SqlDbType.VarChar).Value = packingSlip.PackingRemarks;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.Xml).Value = packingSlip.DetailXML;
                        
                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = packingSlip.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = packingSlip.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = packingSlip.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = packingSlip.Common.UpdatedDate;
                        outputStatus = cmd.Parameters.Add("@Status", SqlDbType.SmallInt);
                        outputStatus.Direction = ParameterDirection.Output;
                        IDOut = cmd.Parameters.Add("@IDOut", SqlDbType.UniqueIdentifier, 5);
                        IDOut.Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                    }
                    switch (outputStatus.Value.ToString())
                    {
                        case "0":
                            throw new Exception(packingSlip.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                        case "1":
                            return new
                            {
                                ID = IDOut.Value.ToString(),
                                Status = outputStatus.Value.ToString(),
                                Message = packingSlip.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = packingSlip.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }
        #endregion InsertUpdatePackingSlip

        #region GetPkgSlipByID
        public PackingSlip GetPkgSlipByID(Guid id)
        {
            PackingSlip packingSlip = new PackingSlip();
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
                        cmd.CommandText = "[AMC].[GetPkgSlipByID]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    packingSlip.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : packingSlip.ID);
                                    packingSlip.SalesOrderID = (sdr["SalesOrderID"].ToString() != "" ? Guid.Parse(sdr["SalesOrderID"].ToString()) : packingSlip.SalesOrderID);
                                    packingSlip.SlipNo = (sdr["SlipNo"].ToString() != "" ? sdr["SlipNo"].ToString() : packingSlip.SlipNo);
                                    packingSlip.Date = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()) : packingSlip.Date);
                                    packingSlip.DateFormatted = (sdr["Date"].ToString() != "" ? DateTime.Parse(sdr["Date"].ToString()).ToString(settings.DateFormat) : packingSlip.DateFormatted);
                                    packingSlip.IssueToDispatchDate = (sdr["IssueToDispatchDate"].ToString() != "" ? DateTime.Parse(sdr["IssueToDispatchDate"].ToString()) : packingSlip.IssueToDispatchDate);
                                    packingSlip.IssueToDispatchDateFormatted = (sdr["IssueToDispatchDate"].ToString() != "" ? DateTime.Parse(sdr["IssueToDispatchDate"].ToString()).ToString(settings.DateFormat) : packingSlip.IssueToDispatchDateFormatted);
                                    packingSlip.DispatchedDate = (sdr["DispatchedDate"].ToString() != "" ? DateTime.Parse(sdr["DispatchedDate"].ToString()) : packingSlip.DispatchedDate);
                                    packingSlip.DispatchedDateFormatted = (sdr["DispatchedDate"].ToString() != "" ? DateTime.Parse(sdr["DispatchedDate"].ToString()).ToString(settings.DateFormat) : packingSlip.DispatchedDateFormatted);
                                    packingSlip.ReceivedDate = (sdr["ReceivedDate"].ToString() != "" ? DateTime.Parse(sdr["ReceivedDate"].ToString()) : packingSlip.ReceivedDate);
                                    packingSlip.ReceivedDateFormatted = (sdr["ReceivedDate"].ToString() != "" ? DateTime.Parse(sdr["ReceivedDate"].ToString()).ToString(settings.DateFormat) : packingSlip.ReceivedDateFormatted);
                                    packingSlip.PackedBy = (sdr["PackedBy"].ToString() != "" ? Guid.Parse(sdr["PackedBy"].ToString()) : packingSlip.PackedBy);
                                    packingSlip.DispatchedBy = (sdr["DispatchedBy"].ToString() != "" ? Guid.Parse(sdr["DispatchedBy"].ToString()) : packingSlip.DispatchedBy);
                                    packingSlip.PackingRemarks = (sdr["PackingRemarks"].ToString() != "" ? sdr["PackingRemarks"].ToString() : packingSlip.PackingRemarks);
                                    packingSlip.TotalPackageWeight = (sdr["TotalPackageWeight"].ToString() != "" ? Decimal.Parse(sdr["TotalPackageWeight"].ToString()) : packingSlip.TotalPackageWeight);
                                    packingSlip.CheckedPackageWeight = (sdr["CheckedPackageWeight"].ToString() != "" ? Decimal.Parse(sdr["CheckedPackageWeight"].ToString()) : packingSlip.CheckedPackageWeight);
                                    packingSlip.DispatchRemarks = (sdr["DispatchRemarks"].ToString() != "" ? sdr["DispatchRemarks"].ToString() : packingSlip.DispatchRemarks);
                                    packingSlip.VehiclePlateNo = (sdr["VehiclePlateNo"].ToString() != "" ? sdr["VehiclePlateNo"].ToString() : packingSlip.VehiclePlateNo);
                                    packingSlip.DriverName = (sdr["DriverName"].ToString() != "" ? sdr["DriverName"].ToString() : packingSlip.DriverName);
                                    packingSlip.ReceivedBy = (sdr["ReceivedBy"].ToString() != "" ? sdr["ReceivedBy"].ToString() : packingSlip.ReceivedBy);
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
            return packingSlip;
        }
        #endregion GetPkgSlipByID

        #region PkgSlipDetailByID
        public List<PackingSlipDetail> GetPkgSlipDetailByID(Guid id)
        {
            List<PackingSlipDetail> PkgSlipDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetPkgSlipDetailByID]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                PkgSlipDetailList = new List<PackingSlipDetail>();
                                while (sdr.Read())
                                {
                                    PackingSlipDetail pkgSlipDetail = new PackingSlipDetail();
                                    {
                                        pkgSlipDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : pkgSlipDetail.ID);
                                        pkgSlipDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : pkgSlipDetail.ProductID);
                                        pkgSlipDetail.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : pkgSlipDetail.Name);
                                        pkgSlipDetail.Qty = (sdr["Qty"].ToString() != "" ? Decimal.Parse(sdr["Qty"].ToString()) : pkgSlipDetail.Qty);
                                        pkgSlipDetail.Weight = (sdr["Weight"].ToString() != "" ? Decimal.Parse(sdr["Weight"].ToString()) : pkgSlipDetail.Weight);
                                    }
                                    PkgSlipDetailList.Add(pkgSlipDetail);
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
            return PkgSlipDetailList;
        }
        #endregion PkgSlipDetailByID
    }
}
