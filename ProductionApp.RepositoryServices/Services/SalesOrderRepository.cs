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
                                        //salesOrder.ReqNo = (sdr["ReqNo"].ToString() != "" ? sdr["ReqNo"].ToString() : salesOrder.ReqNo);
                                        //salesOrder.ReqDate = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()) : salesOrder.ReqDate);
                                        //salesOrder.ReqDateFormatted = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()).ToString(settings.DateFormat) : salesOrder.ReqDateFormatted);
                                        //salesOrder.Title = (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : salesOrder.Title);
                                        //salesOrder.ReqStatus = (sdr["ReqStatus"].ToString() != "" ? sdr["ReqStatus"].ToString() : salesOrder.ReqStatus);
                                        //salesOrder.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : salesOrder.ApprovalStatus);
                                        //salesOrder.RequisitionBy = (sdr["RequisitionBy"].ToString() != "" ? sdr["RequisitionBy"].ToString() : salesOrder.RequisitionBy);
                                        //salesOrder.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : salesOrder.TotalCount);
                                        //salesOrder.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : salesOrder.FilteredCount);
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
                        cmd.CommandText = "[AMC].[GetAllSalesOrder]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(salesOrderAdvanceSearch.SearchTerm) ? "" : salesOrderAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = salesOrderAdvanceSearch.DataTablePaging.Start;
                        if (salesOrderAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = salesOrderAdvanceSearch.DataTablePaging.Length;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = salesOrderAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = salesOrderAdvanceSearch.ToDate;

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
                                        //salesOrder.ReqNo = (sdr["ReqNo"].ToString() != "" ? sdr["ReqNo"].ToString() : salesOrder.ReqNo);
                                        //salesOrder.ReqDate = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()) : salesOrder.ReqDate);
                                        //salesOrder.ReqDateFormatted = (sdr["ReqDate"].ToString() != "" ? DateTime.Parse(sdr["ReqDate"].ToString()).ToString(settings.DateFormat) : salesOrder.ReqDateFormatted);
                                        //salesOrder.Title = (sdr["Title"].ToString() != "" ? sdr["Title"].ToString() : salesOrder.Title);
                                        //salesOrder.ReqStatus = (sdr["ReqStatus"].ToString() != "" ? sdr["ReqStatus"].ToString() : salesOrder.ReqStatus);
                                        //salesOrder.ApprovalStatus = (sdr["ApprovalStatus"].ToString() != "" ? sdr["ApprovalStatus"].ToString() : salesOrder.ApprovalStatus);
                                        //salesOrder.RequisitionBy = (sdr["RequisitionBy"].ToString() != "" ? sdr["RequisitionBy"].ToString() : salesOrder.RequisitionBy);
                                        //salesOrder.TotalCount = (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : salesOrder.TotalCount);
                                        //salesOrder.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : salesOrder.FilteredCount);
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
                        cmd.CommandText = "[AMC].[InsertUpdateSalesOrder]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IsUpdate", SqlDbType.Bit).Value = salesOrder.IsUpdate;
                        cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = salesOrder.ID;
                        cmd.Parameters.Add("@FileDupID", SqlDbType.UniqueIdentifier).Value = salesOrder.hdnFileID;
                        cmd.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = salesOrder.OrderDateFormatted;
                        cmd.Parameters.Add("@CustomerID", SqlDbType.UniqueIdentifier).Value = salesOrder.CustomerID;
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
                        cmd.CommandText = "[AMC].[GetAllSalesOrder]";
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

        public List<SalesOrderDetail> GetSalesOrderProductList(Guid salesOrderId)
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
                        cmd.CommandText = "[AMC].[GetProductList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SaleOrderID", SqlDbType.UniqueIdentifier).Value = salesOrderId;
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
                                        salesOrder.Product = new Product();
                                        salesOrder.ProductID= (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : salesOrder.ProductID);
                                        salesOrder.Product.Name= (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : salesOrder.Product.Name);
                                        salesOrder.Product.CurrentStock= (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : salesOrder.Product.CurrentStock);
                                        salesOrder.Quantity = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : salesOrder.Quantity);
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
                                    salesOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : salesOrder.ID);
                                    salesOrder.OrderNo = (sdr["OrderNo"].ToString() != "" ? sdr["OrderNo"].ToString() : salesOrder.OrderNo);
                                    salesOrder.OrderDate = (sdr["OrderDate"].ToString() != "" ? DateTime.Parse(sdr["OrderDate"].ToString()) : salesOrder.OrderDate);
                                    salesOrder.OrderDateFormatted = (sdr["OrderDate"].ToString() != "" ? DateTime.Parse(sdr["OrderDate"].ToString()).ToString(settings.DateFormat) : salesOrder.OrderDateFormatted);
                                    salesOrder.ExpectedDeliveryDate = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()) : salesOrder.ExpectedDeliveryDate);
                                    salesOrder.ExpectedDeliveryDateFormatted = (sdr["ExpectedDeliveryDate"].ToString() != "" ? DateTime.Parse(sdr["ExpectedDeliveryDate"].ToString()).ToString(settings.DateFormat) : salesOrder.ExpectedDeliveryDateFormatted);
                                    salesOrder.SalesPerson = (sdr["SalesPerson"].ToString() != "" ? Guid.Parse(sdr["SalesPerson"].ToString()) : salesOrder.SalesPerson);
                                    salesOrder.CustomerID = (sdr["CustomerID"].ToString() != "" ? Guid.Parse(sdr["CustomerID"].ToString()) : salesOrder.CustomerID);
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
                        cmd.CommandText = "[AMC].[GetSalesOrderDetail]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@SaleOrderID", SqlDbType.UniqueIdentifier).Value = salesOrderId;
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
                                        //salesOrder.Product = new Product();
                                        //salesOrder.ProductID = (sdr["ProductID"].ToString() != "" ? Guid.Parse(sdr["ProductID"].ToString()) : salesOrder.ProductID);
                                        //salesOrder.Product.Name = (sdr["Name"].ToString() != "" ? sdr["Name"].ToString() : salesOrder.Product.Name);
                                        //salesOrder.Product.CurrentStock = (sdr["CurrentStock"].ToString() != "" ? decimal.Parse(sdr["CurrentStock"].ToString()) : salesOrder.Product.CurrentStock);
                                        //salesOrder.Quantity = (sdr["OrderQty"].ToString() != "" ? decimal.Parse(sdr["OrderQty"].ToString()) : salesOrder.Quantity);
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
