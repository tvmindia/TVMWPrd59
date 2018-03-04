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
    public class PurchaseInvoiceRepository: IPurchaseInvoiceRepository
    {


        #region Constructor Injection
        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();
        public PurchaseInvoiceRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection


        #region GetPurchaseSummary
        public List<PurchaseSummary> GetPurchaseSummary()
        {
            List<PurchaseSummary> PurchaseInvoiceSummaryList = new List<PurchaseSummary>();
            PurchaseSummary PurchaseInvoiceSummary = null;
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
                        cmd.CommandText = "[AMC].[GetPurchaseSummary]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    PurchaseInvoiceSummary = new PurchaseSummary();
                                    PurchaseInvoiceSummary.Month = (sdr["Month"].ToString() != "" ? sdr["Month"].ToString() : PurchaseInvoiceSummary.Month);
                                    PurchaseInvoiceSummary.MonthCode = (sdr["MonthCode"].ToString() != "" ? int.Parse(sdr["MonthCode"].ToString()) : PurchaseInvoiceSummary.MonthCode);
                                    PurchaseInvoiceSummary.Year = (sdr["Year"].ToString() != "" ? int.Parse(sdr["Year"].ToString()) : PurchaseInvoiceSummary.Year);
                                    PurchaseInvoiceSummary.Purchase = (sdr["Purchase"].ToString() != "" ? decimal.Parse(sdr["Purchase"].ToString()) : PurchaseInvoiceSummary.Purchase);
                                    PurchaseInvoiceSummaryList.Add(PurchaseInvoiceSummary);
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
            return PurchaseInvoiceSummaryList;
        }
        #endregion GetPurchaseSummary
    
}
}
