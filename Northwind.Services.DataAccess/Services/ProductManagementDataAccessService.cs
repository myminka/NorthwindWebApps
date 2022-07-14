using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.DataAccess.Interfaces;
using Northwind.DataAccess.Products;
using Northwind.DataAccess.SqlServer;

namespace Northwind.Services.DataAccess.Services
{
    public class ProductManagementDataAccessService : IProductDataAccessObject
    {
        private readonly IProductDataAccessObject dao;
        public ProductManagementDataAccessService(NorthwindDataAccessFactory accessFactory)
        {
            this.dao = accessFactory is null ? throw new ArgumentNullException(nameof(accessFactory))
                : accessFactory.GetProductDataAccessObject();
        }

        public Task<bool> DeleteProductAsync(int productId)
        {
            return this.dao.DeleteProductAsync(productId);
        }

        public Task<ProductTransferObject> FindProductAsync(int productId)
        {
            return this.dao.FindProductAsync(productId);
        }

        public Task<int> InsertProductAsync(ProductTransferObject product)
        {
            return this.dao.InsertProductAsync(product);
        }

        public Task<IList<ProductTransferObject>> SelectProductByCategoryAsync(ICollection<int> collectionOfCategoryId)
        {
            return this.dao.SelectProductByCategoryAsync(collectionOfCategoryId);
        }

        public Task<IList<ProductTransferObject>> SelectProductsAsync(int offset, int limit)
        {
            return this.dao.SelectProductsAsync(offset, limit);
        }

        public Task<IList<ProductTransferObject>> SelectProductsByNameAsync(ICollection<string> productNames)
        {
            return this.dao.SelectProductsByNameAsync(productNames);
        }

        public Task<bool> UpdateProductAsync(ProductTransferObject product)
        {
            return this.dao.UpdateProductAsync(product);
        }
    }
}
