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
        List<Product> GetProductForSelectList(string type);
        List<Product> GetAllProduct(ProductAdvanceSearch productAdvanceSearch);
        bool CheckProductCodeExist(Product product);
        object InsertUpdateProduct(Product product);
        Product GetProduct(Guid id);
        object DeleteProduct(Guid id,string deletedBy, DateTime CreatedDate);
        List<FinishedGoodSummary> GetFinishGoodsSummary();
        List<Product> GetProductListForBillOfMaterial(string componentIDs);
        List<Product> GetProductListByCategoryCode(string code);
    }
}
