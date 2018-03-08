using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Services
{
    public class MaterialReturnFromProductionRepository : IMaterialReturnFromProductionRepository
    {
        private IDatabaseFactory _databaseFactory;
        AppConst _appConst = new AppConst();
        Settings settings = new Settings();
        public MaterialReturnFromProductionRepository(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }
    }
}
