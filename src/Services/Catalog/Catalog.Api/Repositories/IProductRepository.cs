﻿using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();

        Task<Product> GetProductAsync(string id);

        Task<IEnumerable<Product>> GetProductByName(string name);

        Task<IEnumerable<Product>> GetProductByCategoryAsync(string categoryName);

        Task CreateProductAsync(Product product);

        Task<bool> UpdateProduct(Product product);

        Task<bool> DeleteProduct(string id);
    }
}