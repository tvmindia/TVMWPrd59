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
    }
}
