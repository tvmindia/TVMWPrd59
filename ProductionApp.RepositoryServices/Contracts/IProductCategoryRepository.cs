using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IProductCategoryRepository
    {
        List<ProductCategory> GetProductCategoryForSelectList();
        List<ProductCategory> GetAllProductCategory(ProductCategoryAdvanceSearch productCategoryAdvanceSearch);
        ProductCategory GetProductCategory(string code);
        object InsertUpdateProductCategory(ProductCategory productCategory);
        object DeleteProductCategory(string code);

    }
}
