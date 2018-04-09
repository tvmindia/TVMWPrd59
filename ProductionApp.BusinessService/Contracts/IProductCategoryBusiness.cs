using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IProductCategoryBusiness
    {
        List<SelectListItem> GetProductCategoryForSelectList();
        List<ProductCategory> GetAllProductCategory(ProductCategoryAdvanceSearch productCategoryAdvanceSearch);
        ProductCategory GetProductCategory(string code);
        object InsertUpdateProductCategory(ProductCategory productCategory);
        object DeleteProductCategory(string code);
    }
}
