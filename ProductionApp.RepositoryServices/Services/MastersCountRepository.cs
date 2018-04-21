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
  public  class MastersCountRepository:IMastersCountRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public MastersCountRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        #region GetRecentMastersCount
        public List<MastersCount> GetRecentMastersCount()
        {
            List<MastersCount> mastersCountList = new List<MastersCount>();
            MastersCount mastersCount = null;
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
                        cmd.CommandText = "[AMC].[GetRecentMastersCount]";
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader sdr = cmd.ExecuteReader())
                        {
                            if ((sdr != null) && (sdr.HasRows))
                            {
                                while (sdr.Read())
                                {
                                    mastersCount = new MastersCount();
                                    mastersCount.Approver= (sdr["ApproversCount"].ToString() != "" ? int.Parse(sdr["ApproversCount"].ToString()) : mastersCount.Approver);
                                    mastersCount.Bank= (sdr["BankCount"].ToString() != "" ? int.Parse(sdr["BankCount"].ToString()) : mastersCount.Bank);
                                    mastersCount.ChartOfAccounts= (sdr["ChartOfAccountCount"].ToString() != "" ? int.Parse(sdr["ChartOfAccountCount"].ToString()) : mastersCount.ChartOfAccounts);
                                    mastersCount.Customer= (sdr["CustomerCount"].ToString() != "" ? int.Parse(sdr["CustomerCount"].ToString()) : mastersCount.Customer);
                                    mastersCount.Employee= (sdr["EmployeeCount"].ToString() != "" ? int.Parse(sdr["EmployeeCount"].ToString()) : mastersCount.Employee);
                                    mastersCount.Material = (sdr["MaterialCount"].ToString() != "" ? int.Parse(sdr["MaterialCount"].ToString()) : mastersCount.Material);
                                    mastersCount.Product = (sdr["ProductCount"].ToString() != "" ? int.Parse(sdr["ProductCount"].ToString()) : mastersCount.Product);
                                    mastersCount.ProductCategory = (sdr["ProductCategoryCount"].ToString() != "" ? int.Parse(sdr["ProductCategoryCount"].ToString()) : mastersCount.ProductCategory);
                                    mastersCount.ProductStage = (sdr["ProductStageCount"].ToString() != "" ? int.Parse(sdr["ProductStageCount"].ToString()) : mastersCount.ProductStage);
                                    mastersCount.SubComponents = (sdr["SubComponentCount"].ToString() != "" ? int.Parse(sdr["SubComponentCount"].ToString()) : mastersCount.SubComponents);
                                    mastersCount.Supplier = (sdr["SupplierCount"].ToString() != "" ? int.Parse(sdr["SupplierCount"].ToString()) : mastersCount.Supplier);
                                    mastersCountList.Add(mastersCount);
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
            return mastersCountList;
        }

   
        #endregion GetRecentMastersCount
    
    }
}
