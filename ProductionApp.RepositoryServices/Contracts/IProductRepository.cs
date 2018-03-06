using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IProductRepository
    {
        List<Product> GetProductForSelectList();
        List<Product> GetAllProduct(ProductAdvanceSearch productAdvanceSearch);
        bool CheckProductCodeExist(string productCode);
        object InsertUpdateProduct(Product product);
        Product GetProduct(Guid id);
        object DeleteProduct(Guid id);
        List<FinishedGoodSummary> GetFinishGoodsSummary();

    }
}
