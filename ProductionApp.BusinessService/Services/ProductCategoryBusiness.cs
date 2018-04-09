using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductionApp.BusinessService.Services
{
    public class ProductCategoryBusiness : IProductCategoryBusiness
    {
        private IProductCategoryRepository _productCategoryRepository;
        private ICommonBusiness _commonBusiness;

        public ProductCategoryBusiness(IProductCategoryRepository productCategoryRepository, ICommonBusiness commonBusiness)
        {
            _productCategoryRepository = productCategoryRepository;
            _commonBusiness = commonBusiness;
        }
        public List<SelectListItem> GetProductCategoryForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<ProductCategory> productCategoryList = _productCategoryRepository.GetProductCategoryForSelectList();
            if (productCategoryList != null)
                foreach (ProductCategory productCategory in productCategoryList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = productCategory.Description,
                        Value = productCategory.Code,
                        Selected = false
                    });
                }
            return selectListItem;
        }

        //public List<ProductCategory> GetProductCategoryForSelectList()
        //{
        //    return _productCategoryRepository.GetProductCategoryForSelectList();
        //}
        public List<ProductCategory> GetAllProductCategory(ProductCategoryAdvanceSearch productCategoryAdvanceSearch)
        {
            return _productCategoryRepository.GetAllProductCategory(productCategoryAdvanceSearch);
        }
        public ProductCategory GetProductCategory(string code)
        {
            return _productCategoryRepository.GetProductCategory(code);
        }
        public object InsertUpdateProductCategory(ProductCategory productCategory)
        {
            return _productCategoryRepository.InsertUpdateProductCategory(productCategory);
        }
        public object DeleteProductCategory(string code)
        {
            return _productCategoryRepository.DeleteProductCategory(code);
        }

    }
}
