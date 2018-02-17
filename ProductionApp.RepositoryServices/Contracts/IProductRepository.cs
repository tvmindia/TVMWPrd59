﻿using ProductionApp.DataAccessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductionApp.RepositoryServices.Contracts
{
    public interface IProductRepository
    {
        List<Product> GetAllProduct(ProductAdvanceSearch productAdvanceSearch);
        object InsertUpdateProduct(Product product);
        Product GetProduct(Guid id);
    }
}
