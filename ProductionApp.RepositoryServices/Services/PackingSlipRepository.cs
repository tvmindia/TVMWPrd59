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
                                        paySlip.SalesOrder = new SalesOrder();
                                        paySlip.SalesOrder.OrderNo= (sdr["OrderNo"].ToString() != "" ? sdr["OrderNo"].ToString() : paySlip.SalesOrder.OrderNo);
                                        paySlip.SalesOrder.CustomerName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : paySlip.SalesOrder.CustomerName);
                                        //paySlip.SalesOrderDetils = (sdr["OrderNo"].ToString() != "" ? (sdr["OrderNo"].ToString()) : "") + '#' + ' ' +
                                        //   (sdr["CompanyName"].ToString() != "" ? ("&" + sdr["CompanyName"].ToString()) : "");
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
                        if (packingSlip.ID == Guid.Empty) { 
                            cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = false;
                        }
                        else { 
                            cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = true;
                            packingSlip.IsUpdate = true;
                        }
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = packingSlip.ID;
                        cmd.Parameters.Add("@SalesOrderID", SqlDbType.UniqueIdentifier).Value = packingSlip.SalesOrderID;
                        cmd.Parameters.Add("@PackedBy", SqlDbType.UniqueIdentifier).Value = packingSlip.PackedBy;
                        if (packingSlip.DispatchedBy == Guid.Empty)
                            cmd.Parameters.Add("@DispatchedBy", SqlDbType.UniqueIdentifier).Value = null;
                        else
                            cmd.Parameters.Add("@DispatchedBy", SqlDbType.UniqueIdentifier).Value = packingSlip.DispatchedBy;
                        cmd.Parameters.Add("@DispatchedDate", SqlDbType.DateTime).Value = packingSlip.DispatchedDate;
                        cmd.Parameters.Add("@IssueToDispatchDate", SqlDbType.DateTime).Value = packingSlip.IssueToDispatchDate;
                        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = packingSlip.Date;
                        cmd.Parameters.Add("@ReceivedDate", SqlDbType.DateTime).Value = packingSlip.ReceivedDate;
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

        #region GetPackingSlipByID
        public PackingSlip GetPackingSlip(Guid id)
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
                        cmd.CommandText = "[AMC].[GetPackingSlip]";
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
                                    packingSlip.SalesOrder = new SalesOrder();
                                    packingSlip.SalesOrder.OrderNo= (sdr["OrderNo"].ToString() != "" ? sdr["OrderNo"].ToString() : packingSlip.SalesOrder.OrderNo);
                                    packingSlip.SalesOrder.CustomerName= (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : packingSlip.SalesOrder.CustomerName);
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
        #endregion GetPackingSlipByID

        #region PackingSlipDetailByID
        public List<PackingSlipDetail> GetPackingSlipDetail(Guid id)
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
                        cmd.CommandText = "[AMC].[GetPackingSlipDetail]";
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
        #endregion PackingSlipDetailByID

        #region PackingSlipDetailByIDForEdit
        public List<PackingSlip> PackingSlipDetailByPackingSlipDetailID(Guid PkgSlipDetailID)
        {
            List<PackingSlip> pkgSlipList = null;
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
                        cmd.CommandText = "[AMC].[GetPackingSlipDetailByPackingSlipDetailID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@PkgSlipDetailID", SqlDbType.UniqueIdentifier).Value = PkgSlipDetailID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                pkgSlipList = new List<PackingSlip>();
                                while (sdr.Read())
                                {
                                    PackingSlip pkgSlip = new PackingSlip();
                                    {
                                        pkgSlip.SalesOrder = new SalesOrder();
                                        pkgSlip.SalesOrder.SalesOrderDetail = new SalesOrderDetail();
                                        pkgSlip.SalesOrderID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : pkgSlip.SalesOrderID);
                                        pkgSlip.ID = (sdr["PkgDetailID"].ToString() != "" ? Guid.Parse(sdr["PkgDetailID"].ToString()) : pkgSlip.ID);
                                        pkgSlip.SalesOrder.SalesOrderDetail.Product = new Product();
                                        pkgSlip.SalesOrder.SalesOrderDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : pkgSlip.SalesOrder.SalesOrderDetail.ProductID);
                                        pkgSlip.SalesOrder.SalesOrderDetail.Product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : pkgSlip.SalesOrder.SalesOrderDetail.Product.Name);
                                        pkgSlip.SalesOrder.SalesOrderDetail.Product.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : pkgSlip.SalesOrder.SalesOrderDetail.Product.CurrentStock);
                                        pkgSlip.SalesOrder.SalesOrderDetail.Quantity = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : pkgSlip.SalesOrder.SalesOrderDetail.Quantity);
                                        pkgSlip.SalesOrder.SalesOrderDetail.PrevPkgQty = (sdr["PackingQty"].ToString() != "" ? decimal.Parse(sdr["PackingQty"].ToString()) : pkgSlip.SalesOrder.SalesOrderDetail.PrevPkgQty);
                                        pkgSlip.SalesOrder.SalesOrderDetail.PkgWt = (sdr["PkgWt"].ToString() != "" ? decimal.Parse(sdr["PkgWt"].ToString()) : pkgSlip.SalesOrder.SalesOrderDetail.PkgWt);
                                        decimal Bal = pkgSlip.SalesOrder.SalesOrderDetail.Quantity - pkgSlip.SalesOrder.SalesOrderDetail.PrevPkgQty;
                                        //if (pkgSlip.SalesOrder.SalesOrderDetail.Product.CurrentStock >= Bal)
                                        //    pkgSlip.SalesOrder.SalesOrderDetail.CurrentPkgQty = Bal;
                                        //else
                                            pkgSlip.SalesOrder.SalesOrderDetail.CurrentPkgQty = (sdr["CurrentPkgQty"].ToString() != "" ? decimal.Parse(sdr["CurrentPkgQty"].ToString()) : pkgSlip.SalesOrder.SalesOrderDetail.CurrentPkgQty);
                                    }
                                    pkgSlipList.Add(pkgSlip);
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

            return pkgSlipList;
        }
        #endregion PackingSlipDetailByIDForEdit

        #region DeletePackingSlipDetail
        public object DeletePackingSlipDetail(Guid id)
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
                        cmd.CommandText = "[AMC].[DeletePkgSlipDetail]";
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
        #endregion DeletePackingSlipDetail

        #region DeletePackingSlip
        public object DeletePackingSlip(Guid id)
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
                        cmd.CommandText = "[AMC].[DeletePackingSlipDetail]";
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
        #endregion DeletePackingSlip

        #region GetPackingSlipForSelectList
        public List<PackingSlip> GetPackingSlipForSelectList()
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
                        cmd.CommandText = "[AMC].[GetPackingSlipForSelectList]"; 
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
                                        paySlip.SalesOrderID = (sdr["SalesOrderID"].ToString() != "" ? Guid.Parse(sdr["SalesOrderID"].ToString()) : paySlip.SalesOrderID);
                                        paySlip.SalesOrder = new SalesOrder();
                                        paySlip.SalesOrder.OrderNo = (sdr["OrderNo"].ToString() != "" ? sdr["OrderNo"].ToString() : paySlip.SalesOrder.OrderNo);
                                        paySlip.SalesOrder.CustomerName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : paySlip.SalesOrder.CustomerName);
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


        #endregion GetPackingSlipForSelectList

        public List<PackingSlipDetail> GetPackingSlipDetailForCustomerInvoice(Guid packingSlipID)
        {
            List<PackingSlipDetail> PackingSlipDetailList = null;
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
                        cmd.CommandText = "[AMC].[GetPackingSlipDetailForCustomerInvoice]";
                        cmd.Parameters.Add("@PackingSlipID", SqlDbType.UniqueIdentifier).Value = packingSlipID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                PackingSlipDetailList = new List<PackingSlipDetail>();
                                while (sdr.Read())
                                {
                                    PackingSlipDetail packingSlipDetail = new PackingSlipDetail();
                                    {
                                        packingSlipDetail.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : packingSlipDetail.ID);
                                        packingSlipDetail.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : packingSlipDetail.ProductID);
                                        packingSlipDetail.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : packingSlipDetail.Name);
                                        packingSlipDetail.Qty = (sdr["Qty"].ToString() != "" ? decimal.Parse(sdr["Qty"].ToString()) : packingSlipDetail.Qty);
                                        packingSlipDetail.Weight = (sdr["Weight"].ToString() != "" ? decimal.Parse(sdr["Weight"].ToString()) : packingSlipDetail.Weight);
                                    }
                                    PackingSlipDetailList.Add(packingSlipDetail);
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
            return PackingSlipDetailList;
        }

    }
}
