using System;
using System.Collections.Generic;
using System.Linq;
using Northwind.Services.Interfaces;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFramework.Services
{
    /// <summary>
    /// Represents a stub for a product management service.
    /// </summary>
    public sealed class ProductManagementService : IProductManagementService
    {
        private readonly NorthwindContext context;

        public ProductManagementService()
        {
            this.context = new NorthwindContext();
        }

        /// <inheritdoc/>
        public int CreateProduct(Product product)
        {
            product.Id = this.context.ProductId++;
            this.context.Products.Add(product);
            return product.Id;
        }

        /// <inheritdoc/>
        public bool DestroyProduct(int productId)
        {
            var res = this.context.Products.Any(p => p.Id == productId);
            var c = this.context.Products.RemoveAll(p => p.Id == productId);
            return res;
        }

        /// <inheritdoc/>
        public IList<Product> LookupProductsByName(IList<string> names)
        {
            return this.context.Products.Where(p => names.Contains(p.Name)).ToList();
        }

        /// <inheritdoc/>
        public IList<Product> ShowProducts(int offset, int limit)
        {
            return this.context.Products.ToList();
        }

        /// <inheritdoc/>
        public IList<Product> ShowProductsForCategory(int categoryId)
        {
            return this.context.Products.Where(p => p.CategoryId == categoryId).ToList();
        }

        /// <inheritdoc/>
        public bool TryShowProduct(int productId, out Product product)
        {
            var res = this.context.Products.Any(p => p.Id == productId);
            product = res ? this.context.Products.First(p => p.Id == productId) : null;
            return res;
        }

        /// <inheritdoc/>
        public bool UpdateProduct(int productId, Product product)
        {
            var index = this.context.Categories.FindIndex(p => p.Id == productId);

            if (index != -1)
            {
                product.Id = this.context.ProductId++;
                this.context.Products[index] = product;
            }

            return index != -1;
        }
    }
}
