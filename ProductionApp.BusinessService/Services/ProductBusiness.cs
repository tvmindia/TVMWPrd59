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
    public class ProductBusiness : IProductBusiness
    {
        private IProductRepository _productRepository;
        private ICommonBusiness _commonBusiness;

        public ProductBusiness(IProductRepository productRepository, ICommonBusiness commonBusiness)
        {
            _productRepository = productRepository;
            _commonBusiness = commonBusiness;
        }
        public List<SelectListItem> GetProductForSelectList()
        {
            List<SelectListItem> selectListItem = new List<SelectListItem>();
            List<Product> productList = _productRepository.GetProductForSelectList();
            if (productList != null)
                foreach (Product product in productList)
                {
                    selectListItem.Add(new SelectListItem
                    {
                        Text = product.Code+" - " +product.Name,
                        Value = product.ID.ToString(),
                        Selected = false
                    });
                }
            return selectListItem;
        }
        public List<Product> GetAllProduct(ProductAdvanceSearch productAdvanceSearch)
        {
            return _productRepository.GetAllProduct(productAdvanceSearch);
        }
        public object InsertUpdateProduct(Product product)
        {
            return _productRepository.InsertUpdateProduct(product);
        }
        public bool CheckProductCodeExist(Product product)
        {
            return _productRepository.CheckProductCodeExist(product);
        }
        public Product GetProduct(Guid id)
        {
            return _productRepository.GetProduct(id);
        }
        public object DeleteProduct(Guid id)
        {
            return _productRepository.DeleteProduct(id);
        }

        public List<FinishedGoodSummary> GetFinishGoodsSummary()
        {
            List<FinishedGoodSummary> result = _productRepository.GetFinishGoodsSummary();
            if (result != null)
            {
                int r = 150;
                int g = 120;
                int b = 80;
                string color = "rgba($r$,$g$,$b$,0.6)";
                foreach (FinishedGoodSummary s in result)
                {
                    Random rnd = new Random();

                    s.Color = color.Replace("$r$", r.ToString()).Replace("$g$", g.ToString()).Replace("$b$", b.ToString());
                    b = b + 50;
                    g = g + 30;
                    r = r + g;
                    if (b > 250)
                    {
                        b = 0;
                    }
                    if (g > 250)
                    {
                        g = 0;

                    }
                    if (r > 250)
                    {
                        r = 0;
                    }


                    s.ValueFormatted = _commonBusiness.ConvertCurrency(s.Value, 2);

                }
            }
            return result;
        }
    }
}
