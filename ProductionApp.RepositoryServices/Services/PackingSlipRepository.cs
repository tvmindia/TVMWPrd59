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
                                        paySlip.SlipNo = (sdr["ReturnNo"].ToString() != "" ? sdr["ReturnNo"].ToString() : paySlip.SlipNo);
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

    }
}
