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
        List<SelectListItem> GetProductForSelectList(string type=null);
        List<Product> GetAllProduct(ProductAdvanceSearch productAdvanceSearch);
        bool CheckProductCodeExist(Product product);
        object InsertUpdateProduct(Product product);
        Product GetProduct(Guid id);
        object DeleteProduct(Guid id,string deletedBy);
        List<FinishedGoodSummary> GetFinishGoodsSummary();
        List<Product> GetProductListForBillOfMaterial(string componentIDs);
        List<Product> GetProductListByCategoryCode(string code);
    }
}
