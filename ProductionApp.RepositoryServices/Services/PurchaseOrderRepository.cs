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
    public class PurchaseOrderRepository: IPurchaseOrderRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public PurchaseOrderRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        public List<PurchaseOrder>GetAllPurchaseOrder(PurchaseOrderAdvanceSearch purchaseOrderAdvanceSearch)
        {
            List<PurchaseOrder> purchaseOrderList = null;
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
                        cmd.CommandText = "[AMC].[GetAllPurchaseOrder]";
                        cmd.Parameters.Add("@SearchValue", SqlDbType.NVarChar, -1).Value = string.IsNullOrEmpty(purchaseOrderAdvanceSearch.SearchTerm) ? "" : purchaseOrderAdvanceSearch.SearchTerm;
                        cmd.Parameters.Add("@RowStart", SqlDbType.Int).Value = purchaseOrderAdvanceSearch.DataTablePaging.Start;
                        cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = purchaseOrderAdvanceSearch.FromDate;
                        cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value = purchaseOrderAdvanceSearch.ToDate;
                        cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = purchaseOrderAdvanceSearch.Status;
                        if(purchaseOrderAdvanceSearch.Supplier.ID == Guid.Empty)
                            cmd.Parameters.AddWithValue("@SupplierID", DBNull.Value);
                        else
                        cmd.Parameters.Add("@SupplierID", SqlDbType.UniqueIdentifier).Value = purchaseOrderAdvanceSearch.Supplier.ID;
                        if (purchaseOrderAdvanceSearch.DataTablePaging.Length == -1)
                            cmd.Parameters.AddWithValue("@Length", DBNull.Value);
                        else
                            cmd.Parameters.Add("@Length", SqlDbType.Int).Value = purchaseOrderAdvanceSearch.DataTablePaging.Length;
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                purchaseOrderList = new List<PurchaseOrder>();
                                while (sdr.Read())
                                {
                                    PurchaseOrder purchaseOrder = new PurchaseOrder();
                                    {
                                        purchaseOrder.ID= (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : purchaseOrder.ID);
                                        purchaseOrder.PurchaseOrderNo = (sdr["PurchaseOrderNo"].ToString() != "" ? sdr["PurchaseOrderNo"].ToString() : purchaseOrder.PurchaseOrderNo);
                                        purchaseOrder.PurchaseOrderDate = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()): purchaseOrder.PurchaseOrderDate);
                                        purchaseOrder.PurchaseOrderDateFormatted = (sdr["PurchaseOrderDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderDate"].ToString()).ToString(settings.DateFormat) : purchaseOrder.PurchaseOrderDateFormatted);
                                        purchaseOrder.PurchaseOrderIssuedDate = (sdr["PurchaseOrderIssuedDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderIssuedDate"].ToString()) : purchaseOrder.PurchaseOrderIssuedDate);
                                        purchaseOrder.PurchaseOrderIssuedDateFormatted = (sdr["PurchaseOrderIssuedDate"].ToString() != "" ? DateTime.Parse(sdr["PurchaseOrderIssuedDate"].ToString()).ToString(settings.DateFormat) : purchaseOrder.PurchaseOrderIssuedDateFormatted);
                                        purchaseOrder.Supplier = (sdr["Supplier"].ToString() != "" ? sdr["Supplier"].ToString() : purchaseOrder.Supplier);
                                        purchaseOrder.SupplierID = (sdr["SupplierID"].ToString() != "" ? Guid.Parse(sdr["SupplierID"].ToString()) : purchaseOrder.SupplierID);
                                        purchaseOrder.PurchaseOrderStatus = (sdr["PurchaseOrderStatus"].ToString() != "" ? sdr["PurchaseOrderStatus"].ToString() : purchaseOrder.PurchaseOrderStatus);
                                        purchaseOrder.PurchaseOrderTitle = (sdr["PurchaseOrderTitle"].ToString() != "" ? sdr["PurchaseOrderTitle"].ToString() : purchaseOrder.PurchaseOrderTitle);
                                        purchaseOrder.TotalCount= (sdr["TotalCount"].ToString() != "" ? int.Parse(sdr["TotalCount"].ToString()) : purchaseOrder.TotalCount);
                                        purchaseOrder.FilteredCount = (sdr["FilteredCount"].ToString() != "" ? int.Parse(sdr["FilteredCount"].ToString()) : purchaseOrder.FilteredCount);
                                    }
                                    purchaseOrderList.Add(purchaseOrder);
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

            return purchaseOrderList;
        }
        #region PurchaseOrder Dropdown
        public List<PurchaseOrder> GetAllPurchaseOrderForSelectList()
        {
            List<PurchaseOrder> purchaseOrderList = null;
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
                        cmd.CommandText = "[AMC].[GetPurchaseOrderForSelectList]";
                        
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                purchaseOrderList = new List<PurchaseOrder>();
                                while (sdr.Read())
                                {
                                    PurchaseOrder purchaseOrder = new PurchaseOrder();
                                    {
                                        purchaseOrder.ID = (sdr["ID"].ToString() != "" ? Guid.Parse(sdr["ID"].ToString()) : purchaseOrder.ID);
                                        purchaseOrder.PurchaseOrderNo = (sdr["PurchaseOrderNo"].ToString() != "" ? sdr["PurchaseOrderNo"].ToString() : purchaseOrder.PurchaseOrderNo);
                                    }
                                    purchaseOrderList.Add(purchaseOrder);
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

            return purchaseOrderList;
        }
        #endregion PurchaseOrder Dropdown
    }
}
