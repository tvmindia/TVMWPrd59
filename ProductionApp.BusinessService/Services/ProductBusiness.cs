using ProductionApp.BusinessService.Contracts;
using ProductionApp.DataAccessObject.DTO;
using ProductionApp.RepositoryServices.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.BusinessService.Services
{
    public class ProductBusiness: IProductBusiness
    {
        private IProductRepository _productRepository;

        public ProductBusiness(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAllProduct(ProductAdvanceSearch productAdvanceSearch)
        {
            return _productRepository.GetAllProduct(productAdvanceSearch);
        }
        public object InsertUpdateProduct(Product product)
        {
            return _productRepository.InsertUpdateProduct(product);
        }
        public Product GetProduct(Guid id)
        {
            return _productRepository.GetProduct(id);
        }

    }
}
