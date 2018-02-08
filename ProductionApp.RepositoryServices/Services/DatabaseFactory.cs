using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Services
{
    public class DatabaseFactory:IDatabaseFactory
    {
        private SqlConnection _SQLCon = null;
        public SqlConnection GetDBConnection()
        {
            try
            {
                _SQLCon = new SqlConnection(ConfigurationManager.ConnectionStrings["SPAppsConnection"].ConnectionString);

            }
            catch (Exception ex)
            {

                throw ex;

            }
            return _SQLCon;
        }

        public Boolean DisconectDB()
        {
            try
            {
                if (_SQLCon.State == ConnectionState.Open)
                {
                    _SQLCon.Close();
                    _SQLCon.Dispose();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
    }
}
