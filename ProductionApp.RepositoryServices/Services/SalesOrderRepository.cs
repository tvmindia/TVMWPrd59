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
    public class SalesOrderRepository: ISalesOrderRepository
    {

        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();

        /// <summary>
        /// Constructor Injection:-Getting IDatabaseFactory implementing object
        /// </summary>
        /// <param name="databaseFactory"></param>
        public SalesOrderRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public List<SalesOrder> GetAllSalesOrder(SalesOrderAdvanceSearch salesOrderAdvanceSearch)
        {
            List<SalesOrder> salesOrderList = null;
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
                        cmd.CommandText = "[AMC].[GetAllSalesOrder]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(salesOrderAdvanceSearch.SearchTerm) ? "" : salesOrderAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = salesOrderAdvanceSearch.DataTablePaging.Start;
                        if (salesOrderAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = salesOrderAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = salesOrderAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = salesOrderAdvanceSearch.ToDate;
                        if (salesOrderAdvanceSearch.CustomerID != Guid.Empty)
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = salesOrderAdvanceSearch.CustomerID;
                        if (salesOrderAdvanceSearch.EmployeeID != Guid.Empty)
                            cmd.Parameters.Add("@EmployeeID", SqlDbType.UniqueIdentifier).Value = salesOrderAdvanceSearch.EmployeeID;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                salesOrderList = new List<SalesOrder>();
                                while (sdr.Read())
                                {
                                    SalesOrder salesOrder = new SalesOrder();
                                    {
                                        salesOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : salesOrder.ID);
                                        salesOrder.OrderNo = (sdr["OrderNo"].ToString() != "" ? sdr["OrderNo"].ToString() : salesOrder.OrderNo);
                                        salesOrder.OrderDate = (sdr["OrderDate"].ToString() != "" ? DateTime.Parse(sdr["OrderDate"].ToString()) : salesOrder.OrderDate);
                                        salesOrder.OrderDateFormatted = (sdr["OrderDate"].ToString() != "" ? DateTime.Parse(sdr["OrderDate"].ToString()).ToString(settings.DateFormat) : salesOrder.OrderDateFormatted);
                                        salesOrder.ExpectedDeliveryDate = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()) : salesOrder.ExpectedDeliveryDate);
                                        salesOrder.ExpectedDeliveryDateFormatted = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()).ToString(settings.DateFormat) : salesOrder.ExpectedDeliveryDateFormatted);
                                        salesOrder.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : salesOrder.CustomerName);
                                        salesOrder.OrderAmount = (sdr["NetAmount"].ToString() != "" ? decimal.Parse(sdr["NetAmount"].ToString()) : salesOrder.OrderAmount);
                                        salesOrder.OrderStatus = (sdr["SOStatus"].ToString() != "" ? sdr["SOStatus"].ToString() : salesOrder.OrderStatus);
                                        salesOrder.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : salesOrder.TotalCount);
                                        salesOrder.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : salesOrder.FilteredCount);
                                    }
                                    salesOrderList.Add(salesOrder);
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

            return salesOrderList;
        }

        public List<SalesOrder> GetAllSalesOrderDetail(SalesOrderAdvanceSearch salesOrderAdvanceSearch)
        {
            List<SalesOrder> salesOrderList = null;
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
                        cmd.CommandText = "[AMC].[GetAllSalesOrderDetail]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(salesOrderAdvanceSearch.SearchTerm) ? "" : salesOrderAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = salesOrderAdvanceSearch.DataTablePaging.Start;
                        if (salesOrderAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = salesOrderAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = salesOrderAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = salesOrderAdvanceSearch.ToDate;
                        if (salesOrderAdvanceSearch.CustomerID != Guid.Empty)
                            cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = salesOrderAdvanceSearch.CustomerID;
                        if (salesOrderAdvanceSearch.EmployeeID != Guid.Empty)
                            cmd.Parameters.Add("@EmployeeID", SqlDbType.UniqueIdentifier).Value = salesOrderAdvanceSearch.EmployeeID;

                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                salesOrderList = new List<SalesOrder>();
                                while (sdr.Read())
                                {
                                    SalesOrder salesOrder = new SalesOrder();
                                    {
                                        salesOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : salesOrder.ID);
                                        salesOrder.OrderNo = (sdr["OrderNo"].ToString() != "" ? sdr["OrderNo"].ToString() : salesOrder.OrderNo);
                                        salesOrder.DispatchedDates = (sdr["DispatchedDates"].ToString() != "" ? sdr["DispatchedDates"].ToString() : salesOrder.DispatchedDates);
                                        salesOrder.OrderDate = (sdr["OrderDate"].ToString() != "" ? DateTime.Parse(sdr["OrderDate"].ToString()) : salesOrder.OrderDate);
                                        salesOrder.OrderDateFormatted = (sdr["OrderDate"].ToString() != "" ? DateTime.Parse(sdr["OrderDate"].ToString()).ToString(settings.DateFormat) : salesOrder.OrderDateFormatted);
                                        salesOrder.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : salesOrder.CustomerName);
                                        salesOrder.SalesOrderDetail = new SalesOrderDetail();
                                        salesOrder.SalesOrderDetail.ID = (sdr["DetailID"].ToString() != "" ? Guid.Parse(sdr["DetailID"].ToString()) : salesOrder.SalesOrderDetail.ID);
                                        salesOrder.SalesOrderDetail.GroupName = (sdr["GroupName"].ToString() != "" ? sdr["GroupName"].ToString() : salesOrder.SalesOrderDetail.GroupName);
                                        salesOrder.SalesOrderDetail.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : salesOrder.SalesOrderDetail.Quantity);
                                        salesOrder.SalesOrderDetail.ExpectedDeliveryDate = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()) : salesOrder.SalesOrderDetail.ExpectedDeliveryDate);
                                        salesOrder.SalesOrderDetail.ExpectedDeliveryDateFormatted = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()).ToString(settings.DateFormat) : salesOrder.SalesOrderDetail.ExpectedDeliveryDateFormatted);
                                        salesOrder.SalesOrderDetail.Product = new Product();
                                        salesOrder.SalesOrderDetail.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : salesOrder.SalesOrderDetail.Product.Name);
                                        salesOrder.SalesOrderDetail.Product.HSNNo = (sdr["HSNNo"].ToString() != "" ? sdr["HSNNo"].ToString() : salesOrder.SalesOrderDetail.Product.HSNNo);
                                        salesOrder.SOStatus = (sdr["SOStatus"].ToString() != "" ? sdr["SOStatus"].ToString() : salesOrder.SOStatus);
                                        salesOrder.DispatchedDates = (sdr["DispatchedDates"].ToString() != "" ? sdr["DispatchedDates"].ToString() : salesOrder.DispatchedDates);
                                        salesOrder.DispatchedQty = (sdr["DISPATCHEDQTY"].ToString() != "" ? decimal.Parse(sdr["DISPATCHEDQTY"].ToString()) : salesOrder.DispatchedQty);
                                        salesOrder.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : salesOrder.TotalCount);
                                        salesOrder.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : salesOrder.FilteredCount);
                                        salesOrder.DispatchedQty = (sdr["DISPATCHEDQTY"].ToString() != "" ? decimal.Parse(sdr["DISPATCHEDQTY"].ToString()) : salesOrder.DispatchedQty);
                                        salesOrder.OrderStatus = (sdr["SOStatus"].ToString() != "" ? sdr["SOStatus"].ToString() : salesOrder.OrderStatus);
                                    }
                                    salesOrderList.Add(salesOrder);
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

            return salesOrderList;
        }
        public object InsertUpdateSalesOrder(SalesOrder salesOrder)
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
                        cmd.CommandText = "[AMC].[InsertUpdateSalesOrder_GroupingDemo]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = salesOrder.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = salesOrder.ID;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = salesOrder.hdnFileID;
                        cmd.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = salesOrder.OrderDateFormatted;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = salesOrder.CustomerID;
                        cmd.Parameters.Add("@ReferenceCustomer", SqlDbType.UniqueIdentifier).Value = salesOrder.ReferenceCustomer;
                        if(salesOrder.SalesPerson!=Guid.Empty)
                        cmd.Parameters.Add("@SalesPerson", SqlDbType.UniqueIdentifier).Value = salesOrder.SalesPerson;

                        cmd.Parameters.Add("@BillingAddress", SqlDbType.VarChar, -1).Value = salesOrder.BillingAddress;
                        cmd.Parameters.Add("@ShippingAddress", SqlDbType.VarChar, -1).Value = salesOrder.ShippingAddress;
                        cmd.Parameters.Add("@Remarks", SqlDbType.VarChar,-1).Value = salesOrder.Remarks;
                        cmd.Parameters.Add("@ExpectedDeliveryDate", SqlDbType.DateTime).Value = salesOrder.ExpectedDeliveryDateFormatted;
                        cmd.Parameters.Add("@DetailXML", SqlDbType.VarChar, -1).Value = salesOrder.DetailXML;

                        cmd.Parameters.Add("@CreatedBy", SqlDbType.NVarChar, 250).Value = salesOrder.Common.CreatedBy;
                        cmd.Parameters.Add("@CreatedDate", SqlDbType.DateTime).Value = salesOrder.Common.CreatedDate;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.NVarChar, 250).Value = salesOrder.Common.UpdatedBy;
                        cmd.Parameters.Add("@UpdatedDate", SqlDbType.DateTime).Value = salesOrder.Common.UpdatedDate;
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
                        throw new Exception(salesOrder.IsUpdate ? _appConst.UpdateFailure : _appConst.InsertFailure);
                    case "1":
                        //  requisition.ID = Guid.Parse(IDOut.Value.ToString());
                        return new
                        {
                            ID = IDOut.Value.ToString(),
                            Status = outputStatus.Value.ToString(),
                            Message = salesOrder.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
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
                Message = salesOrder.IsUpdate ? _appConst.UpdateSuccess : _appConst.InsertSuccess
            };
        }

        public List<SalesOrder> GetAllSalesOrderForSelectList()
        {
            List<SalesOrder> salesOrderList = null;
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
                        cmd.CommandText = "[AMC].[GetAllSalesOrderForPkgSlip]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                salesOrderList = new List<SalesOrder>();
                                while (sdr.Read())
                                {
                                    SalesOrder salesOrder = new SalesOrder();
                                    {
                                        salesOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : salesOrder.ID);
                                        salesOrder.OrderNo = (sdr["OrderNo"].ToString() != "" ? sdr["OrderNo"].ToString() : salesOrder.OrderNo);
                                    }
                                    salesOrderList.Add(salesOrder);
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

            return salesOrderList;
        }

        public List<SalesOrderDetail> GetSalesOrderProductList(Guid salesOrderId, Guid packingSlipID)
        {
            List<SalesOrderDetail> salesOrderList = null;
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
                        cmd.CommandText = "[AMC].[GetProductListForPackingSlip_Grouping]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SaleOrderID", SqlDbType.UniqueIdentifier).Value = salesOrderId;
                        cmd.Parameters.Add("@packingSlipID", SqlDbType.UniqueIdentifier).Value = packingSlipID;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                salesOrderList = new List<SalesOrderDetail>();
                                while (sdr.Read())
                                {
                                    SalesOrderDetail salesOrder = new SalesOrderDetail();
                                    {
                                        salesOrder.SalesOrderID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : salesOrder.ID);
                                        salesOrder.GroupID = (sdr["GroupID"].ToString() != "" ? Guid.Parse(sdr["GroupID"].ToString()) : salesOrder.GroupID);
                                        salesOrder.GroupName = (sdr["GroupName"].ToString() != "" ? sdr["GroupName"].ToString() : salesOrder.GroupName);
                                        salesOrder.Product = new Product();
                                        salesOrder.ProductID= (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : salesOrder.ProductID);
                                        salesOrder.Product.Name= (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : salesOrder.Product.Name);
                                        salesOrder.Product.CurrentStock= (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : salesOrder.Product.CurrentStock);
                                        salesOrder.Quantity = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : salesOrder.Quantity);
                                        salesOrder.PrevPkgQty = (sdr["PackingQty"].ToString() != "" ? decimal.Parse(sdr["PackingQty"].ToString()) : salesOrder.PrevPkgQty);
                                        salesOrder.PkgWt = (sdr["PkgWt"].ToString() != "" ? decimal.Parse(sdr["PkgWt"].ToString()) : salesOrder.PkgWt);
                                        salesOrder.ChildCount = (sdr["ChildCount"].ToString() != "" ? int.Parse(sdr["ChildCount"].ToString()) : salesOrder.ChildCount);
                                        decimal Bal = salesOrder.Quantity - salesOrder.PrevPkgQty;
                                        //if (salesOrder.Product.CurrentStock >= Bal)
                                            salesOrder.CurrentPkgQty = Bal;
                                        //else
                                           // salesOrder.CurrentPkgQty = salesOrder.Product.CurrentStock; 
                                        
                                    }
                                    salesOrderList.Add(salesOrder);
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

            return salesOrderList;
        }

        public SalesOrder GetCustomerOfSalesOrderForPackingSlip(Guid salesOrderId)
        {
            SalesOrder salesOrder = new SalesOrder();
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
                        cmd.CommandText = "[AMC].[SalesOrderCustomer]";
                        cmd.Parameters.Add("@salesOrderId", SqlDbType.UniqueIdentifier).Value = salesOrderId;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    salesOrder.ExpectedDeliveryDate = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()) : salesOrder.ExpectedDeliveryDate);
                                    salesOrder.ExpectedDeliveryDateFormatted = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()).ToString(settings.DateFormat) : salesOrder.ExpectedDeliveryDateFormatted);
                                    salesOrder.CustomerName = (sdr["CompanyName"].ToString() != "" ? sdr["CompanyName"].ToString() : salesOrder.CustomerName);
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
            return salesOrder;
        }
        public SalesOrder GetSalesOrder(Guid id)
        {
            SalesOrder salesOrder = new SalesOrder();
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
                        cmd.CommandText = "[AMC].[GetSalesOrder]";
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = id;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                if (sdr.Read())
                                {
                                    //salesOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : salesOrder.ID);
                                    salesOrder.OrderNo = (sdr["OrderNo"].ToString() != "" ? sdr["OrderNo"].ToString() : salesOrder.OrderNo);
                                    salesOrder.OrderDate = (sdr["OrderDate"].ToString() != "" ? DateTime.Parse(sdr["OrderDate"].ToString()) : salesOrder.OrderDate);
                                    salesOrder.OrderDateFormatted = (sdr["OrderDate"].ToString() != "" ? DateTime.Parse(sdr["OrderDate"].ToString()).ToString(settings.DateFormat) : salesOrder.OrderDateFormatted);
                                    salesOrder.ExpectedDeliveryDate = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()) : salesOrder.ExpectedDeliveryDate);
                                    salesOrder.ExpectedDeliveryDateFormatted = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()).ToString(settings.DateFormat) : salesOrder.ExpectedDeliveryDateFormatted);
                                    salesOrder.SalesPerson = (sdr["SalesPerson"].ToString() != "" ? Guid.Parse(sdr["SalesPerson"].ToString()) : salesOrder.SalesPerson);
                                    salesOrder.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : salesOrder.CustomerID);
                                    salesOrder.ReferenceCustomer = (sdr["ReferenceCustomer"].ToString() != "" ? Guid.Parse(sdr["ReferenceCustomer"].ToString()) : salesOrder.ReferenceCustomer);
                                    salesOrder.Remarks = (sdr["Remarks"].ToString() != "" ? sdr["Remarks"].ToString() : salesOrder.Remarks);
                                    salesOrder.BillingAddress = (sdr["BillingAddress"].ToString() != "" ? sdr["BillingAddress"].ToString() : salesOrder.BillingAddress);
                                    salesOrder.ShippingAddress = (sdr["ShippingAddress"].ToString() != "" ? sdr["ShippingAddress"].ToString() : salesOrder.ShippingAddress);

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
            return salesOrder;
        }

        public List<SalesOrderDetail> GetSalesOrderDetail(Guid salesOrderId)
        {
            List<SalesOrderDetail> salesOrderList = null;
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
                        cmd.CommandText = "[AMC].[GetSalesOrderDetail_GroupingDemo]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@salesOrderId", SqlDbType.UniqueIdentifier).Value = salesOrderId;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                salesOrderList = new List<SalesOrderDetail>();
                                while (sdr.Read())
                                {
                                    SalesOrderDetail salesOrder = new SalesOrderDetail();
                                    {
                                        salesOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : salesOrder.ID);
                                        salesOrder.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : salesOrder.ProductID);
                                        salesOrder.TaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : salesOrder.TaxTypeCode);
                                        salesOrder.TaxTypeDescription = (sdr["TaxTypeDescription"].ToString() != "" ? sdr["TaxTypeDescription"].ToString() : salesOrder.TaxTypeDescription);
                                        salesOrder.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : salesOrder.UnitCode);
                                        salesOrder.Product = new Product();
                                        salesOrder.Product.Name = (sdr["ProductName"].ToString() != "" ? sdr["ProductName"].ToString() : salesOrder.Product.Name);
                                        salesOrder.Product.HSNNo = (sdr["HSNNo"].ToString() != "" ? sdr["HSNNo"].ToString() : salesOrder.Product.HSNNo);
                                        salesOrder.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : salesOrder.Quantity);
                                        salesOrder.Rate = (sdr["Rate"].ToString() != "" ? decimal.Parse(sdr["Rate"].ToString()) : salesOrder.Rate);
                                        salesOrder.DiscountPercent = (sdr["DiscountPercent"].ToString() != "" ? decimal.Parse(sdr["DiscountPercent"].ToString()) : salesOrder.DiscountPercent);
                                        salesOrder.TradeDiscountAmount = (sdr["TradeDiscountAmount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountAmount"].ToString()) : salesOrder.TradeDiscountAmount);
                                        salesOrder.GrossAmount = (sdr["GrossAmount"].ToString() != "" ? decimal.Parse(sdr["GrossAmount"].ToString()) : salesOrder.GrossAmount);
                                        salesOrder.NetAmount = (sdr["NetAmount"].ToString() != "" ? decimal.Parse(sdr["NetAmount"].ToString()) : salesOrder.NetAmount);
                                        salesOrder.TaxAmount = (sdr["TaxAmount"].ToString() != "" ? decimal.Parse(sdr["TaxAmount"].ToString()) : salesOrder.TaxAmount);
                                        salesOrder.ExpectedDeliveryDateFormatted = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()).ToString(settings.DateFormat) : salesOrder.ExpectedDeliveryDateFormatted);
                                        salesOrder.GroupID = (sdr["GroupID"].ToString() != "" ? Guid.Parse(sdr["GroupID"].ToString()) : salesOrder.GroupID);
                                        salesOrder.PkgWt = (sdr["PkgWt"].ToString() != "" ? decimal.Parse(sdr["PkgWt"].ToString()) : salesOrder.PkgWt);
                                    }
                                    salesOrderList.Add(salesOrder);
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

            return salesOrderList;
        }

        public object DeleteSalesOrderDetail(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteSalesOrderDetail_GroupingDemo]";
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
        public object DeleteSalesOrder(Guid id)
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
                        cmd.CommandText = "[AMC].[DeleteSalesOrder]";
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

        #region GetRecentSalesOrder
        public List<SalesOrder> GetRecentSalesOrder()
        {
            List<SalesOrder> salesOrderList = new List<SalesOrder>();
            SalesOrder salesOrder = null;
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
                        cmd.CommandText = "[AMC].[GetRecentSalesOrder]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    salesOrder = new SalesOrder();
                                    salesOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : Guid.Empty);
                                    salesOrder.OrderNo = (sdr["OrderNo"].ToString() != "" ? sdr["OrderNo"].ToString() : salesOrder.OrderNo);
                                    salesOrder.CustomerName = (sdr["CustomerName"].ToString() != "" ? sdr["CustomerName"].ToString() : salesOrder.CustomerName);
                                    salesOrder.ReferenceCustomerName = (sdr["ReferenceCustomer"].ToString() != "" ? sdr["ReferenceCustomer"].ToString() : salesOrder.ReferenceCustomerName);
                                    salesOrder.ExpectedDeliveryDateFormatted = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()).ToString(settings.DateFormat) : salesOrder.ExpectedDeliveryDateFormatted);
                                    salesOrderList.Add(salesOrder);
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
            return salesOrderList;
        }
        #endregion GetRecentSalesOrder

        #region GetSaleOrderDetailByGroupId
        public List<SalesOrderDetail> GetSaleOrderDetaiByGroupId(Guid id)
        {
            List<SalesOrderDetail> salesOrderList = null;
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
                        cmd.CommandText = "[AMC].[GetSalesOrderDetailBYGroupID_GroupingDemo]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@GroupId", SqlDbType.UniqueIdentifier).Value = id;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                salesOrderList = new List<SalesOrderDetail>();
                                while (sdr.Read())
                                {
                                    SalesOrderDetail salesOrder = new SalesOrderDetail();
                                    {
                                        salesOrder.ID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : salesOrder.ID);
                                        salesOrder.SalesOrderID = (sdr["id"].ToString() != "" ? Guid.Parse(sdr["id"].ToString()) : salesOrder.SalesOrderID);
                                        salesOrder.UnitCode = (sdr["UnitCode"].ToString() != "" ? sdr["UnitCode"].ToString() : salesOrder.UnitCode);
                                        salesOrder.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : salesOrder.Name);
                                        salesOrder.Quantity = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : salesOrder.Quantity);
                                        salesOrder.NumOfSet = (sdr["Quantity"].ToString() != "" ? decimal.Parse(sdr["Quantity"].ToString()) : salesOrder.NumOfSet);
                                        salesOrder.CostPrice = (sdr["SellingPrice"].ToString() != "" ? decimal.Parse(sdr["SellingPrice"].ToString()) : salesOrder.CostPrice);
                                        salesOrder.GroupID = (sdr["GroupID"].ToString() != "" ? Guid.Parse(sdr["GroupID"].ToString()) : salesOrder.GroupID);
                                        salesOrder.GroupName = (sdr["GroupName"].ToString() != "" ? sdr["GroupName"].ToString() : salesOrder.GroupName);
                                        salesOrder.Product = new Product();
                                        salesOrder.Product.ProductCategoryCode= (sdr["ProductCategoryCode"].ToString() != "" ? sdr["ProductCategoryCode"].ToString() : salesOrder.Product.ProductCategoryCode);
                                        salesOrder.WeightInKG = (sdr["WeightInKG"].ToString() != "" ? decimal.Parse(sdr["WeightInKG"].ToString()) : salesOrder.WeightInKG);
                                        salesOrder.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : salesOrder.CurrentStock);
                                        salesOrder.GroupItemExpectedDeliveryDateFormatted = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()).ToString(settings.DateFormat) : salesOrder.GroupItemExpectedDeliveryDateFormatted);
                                        salesOrder.GroupTaxTypeCode = (sdr["TaxTypeCode"].ToString() != "" ? sdr["TaxTypeCode"].ToString() : salesOrder.GroupTaxTypeCode);
                                        salesOrder.GroupItemDiscountPercent = (sdr["DiscountPercent"].ToString() != "" ? decimal.Parse(sdr["DiscountPercent"].ToString()) : salesOrder.GroupItemDiscountPercent);
                                        salesOrder.GroupItemTradeDiscountAmount = (sdr["TradeDiscountAmount"].ToString() != "" ? decimal.Parse(sdr["TradeDiscountAmount"].ToString()) : salesOrder.GroupItemTradeDiscountAmount);
                                        salesOrder.IsInvoiceInKG = sdr["IsInvoiceInKG"].ToString() != "" ? bool.Parse(sdr["IsInvoiceInKG"].ToString()) : salesOrder.IsInvoiceInKG;
                                        salesOrder.OrderDue = (sdr["OrderDue"].ToString() != "" ? decimal.Parse(sdr["OrderDue"].ToString()) : salesOrder.OrderDue);
                                    }
                                    salesOrderList.Add(salesOrder);
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

            return salesOrderList;
        }
        #endregion GetSaleOrderDetaiByGroupId 
    
        public List<SalesOrderDetail> GetProductListForPackingSlipByGroupID(Guid salesOrderId, Guid packingSlipId, Guid groupId)
        {
            List<SalesOrderDetail> salesOrderList = null;
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
                        cmd.CommandText = "[AMC].[GetProductListForPackingSlip_ByGroupID]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SaleOrderID", SqlDbType.UniqueIdentifier).Value = salesOrderId;
                        if(packingSlipId!=Guid.Empty)
                        cmd.Parameters.Add("@packingSlipID", SqlDbType.UniqueIdentifier).Value = packingSlipId;
                        cmd.Parameters.Add("@GroupID", SqlDbType.UniqueIdentifier).Value = groupId;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                salesOrderList = new List<SalesOrderDetail>();
                                while (sdr.Read())
                                {
                                    SalesOrderDetail salesOrder = new SalesOrderDetail();
                                    {
                                        salesOrder.SalesOrderID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : salesOrder.ID);
                                        salesOrder.GroupID = (sdr["GroupID"].ToString() != "" ? Guid.Parse(sdr["GroupID"].ToString()) : salesOrder.GroupID);
                                        salesOrder.GroupName = (sdr["GroupName"].ToString() != "" ? sdr["GroupName"].ToString() : salesOrder.GroupName);
                                        salesOrder.Product = new Product();
                                        salesOrder.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : salesOrder.ProductID);
                                        salesOrder.Product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : salesOrder.Product.Name);
                                        salesOrder.Product.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : salesOrder.Product.CurrentStock);
                                        salesOrder.Quantity = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : salesOrder.Quantity);
                                        salesOrder.PrevPkgQty = (sdr["PackingQty"].ToString() != "" ? decimal.Parse(sdr["PackingQty"].ToString()) : salesOrder.PrevPkgQty);
                                        salesOrder.PkgWt = (sdr["PkgWt"].ToString() != "" ? decimal.Parse(sdr["PkgWt"].ToString()) : salesOrder.PkgWt);
                                        salesOrder.isExists = (sdr["isExists"].ToString() != "" ? Boolean.Parse(sdr["isExists"].ToString()) : salesOrder.isExists);
                                        decimal Bal = salesOrder.Quantity - salesOrder.PrevPkgQty;
                                        if (salesOrder.Product.CurrentStock >= Bal)
                                            salesOrder.CurrentPkgQty = Bal;
                                        else
                                            salesOrder.CurrentPkgQty = salesOrder.Product.CurrentStock;
                                    }
                                    salesOrderList.Add(salesOrder);
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

            return salesOrderList;
        }

    }
}
