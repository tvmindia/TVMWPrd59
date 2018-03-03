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
    public class SalesInvoiceRepository: ISalesInvoiceRepository
    {

        #region Constructor Injection
        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();
        public SalesInvoiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection


        #region GetSalesSummary
        public List<SalesSummary> GetSalesSummary()
        {
            List<SalesSummary> SalesInvoiceSummaryList = new List<SalesSummary>();
            SalesSummary SalesInvoiceSummary = null;
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
                        cmd.CommandText = "[AMC].[GetSalesSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    SalesInvoiceSummary = new SalesSummary();   
                                    SalesInvoiceSummary.Month = (sdr["Month"].ToString() != "" ? sdr["Month"].ToString() : SalesInvoiceSummary.Month);
                                    SalesInvoiceSummary.MonthCode = (sdr["MonthCode"].ToString() != "" ? int.Parse(sdr["MonthCode"].ToString()) : SalesInvoiceSummary.MonthCode);
                                    SalesInvoiceSummary.Year = (sdr["Year"].ToString() != "" ? int.Parse(sdr["Year"].ToString()) : SalesInvoiceSummary.Year);
                                    SalesInvoiceSummary.Sales = (sdr["Sales"].ToString() != "" ? decimal.Parse(sdr["Sales"].ToString()) : SalesInvoiceSummary.Sales);
                                    SalesInvoiceSummaryList.Add(SalesInvoiceSummary);
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
            return SalesInvoiceSummaryList;
        }
        #endregion GetSalesSummary
    }
}
