using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IDatabaseFactory
    {
        SqlConnection GetDBConnection();
        Boolean DisconectDB();
    }
}
