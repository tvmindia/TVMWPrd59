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
    public class ProductionTrackingRepository: IProductionTrackingRepository
    {

        #region Constructor Injection
        private IDatabaseFactory _databaseFactory;
        Settings settings = new Settings();
        AppConst _appConst = new AppConst();
        public ProductionTrackingRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
        #endregion Constructor Injection

        #region GetProductionTrackingSearchList
        public List<ProductionTrackingSearch> GetProductionTrackingSearchList()
        {
            try
            {
                List<ProductionTrackingSearch> productionTrackingSearchList = null;
                using (SqlConnection con = _databaseFactory.GetDBConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        cmd.Connection = con;
                        cmd.CommandText = "[AMC].[GetProductionTrackingSearchList]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                productionTrackingSearchList = new List<ProductionTrackingSearch>();
                                while (sdr.Read())
                                {
                                    ProductionTrackingSearch productionTrackingSearch = new ProductionTrackingSearch();
                                    productionTrackingSearch.Text = (sdr["Text"].ToString() != "" ? sdr["Text"].ToString() : productionTrackingSearch.Text);
                                    productionTrackingSearch.Value = (sdr["Value"].ToString() != "" ? sdr["Value"].ToString() : productionTrackingSearch.Value);
                                    productionTrackingSearchList.Add(productionTrackingSearch);
                                }
                            }
                        }
                        con.Close();
                    }
                }
                return productionTrackingSearchList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion GetProductionTrackingSearchList    

    }
}
