using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.BusinessService.Contracts
{
    public interface IProductBusiness
    {
        List<SelectListItem> GetProductForSelectList();
        List<Product> GetAllProduct(ProductAdvanceSearch productAdvanceSearch);
        bool CheckProductCodeExist(string productCode);
        object InsertUpdateProduct(Product product);
        Product GetProduct(Guid id);
        object DeleteProduct(Guid id);
        List<FinishedGoodSummary> GetFinishGoodsSummary();
    }
}
