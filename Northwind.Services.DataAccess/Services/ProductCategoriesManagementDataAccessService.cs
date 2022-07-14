using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.DataAccess.Interfaces;
using Northwind.DataAccess.Products;
using Northwind.DataAccess.SqlServer;

namespace Northwind.Services.DataAccess.Services
{
    public class ProductCategoriesManagementDataAccessService : IProductCategoryDataAccessObject
    {
        private readonly IProductCategoryDataAccessObject dao;
        public ProductCategoriesManagementDataAccessService(NorthwindDataAccessFactory accessFactory)
        {
            this.dao = accessFactory is null ? throw new ArgumentNullException(nameof(accessFactory))
                : accessFactory.GetProductCategoryDataAccessObject();
        }

        public Task<bool> DeleteProductCategoryAsync(int productCategoryId)
        {
            return this.dao.DeleteProductCategoryAsync(productCategoryId);
        }

        public Task<ProductCategoryTransferObject> FindProductCategoryAsync(int productCategoryId)
        {
            return this.dao.FindProductCategoryAsync(productCategoryId);
        }

        public Task<int> InsertProductCategoryAsync(ProductCategoryTransferObject productCategory)
        {
            return this.dao.InsertProductCategoryAsync(productCategory);
        }

        public Task<IList<ProductCategoryTransferObject>> SelectProductCategoriesAsync(int offset, int limit)
        {
            return this.dao.SelectProductCategoriesAsync(offset, limit);
        }

        public Task<IList<ProductCategoryTransferObject>> SelectProductCategoriesByNameAsync(ICollection<string> productCategoryNames)
        {
            return this.dao.SelectProductCategoriesByNameAsync(productCategoryNames);
        }

        public Task<bool> UpdateProductCategoryAsync(ProductCategoryTransferObject productCategory)
        {
            return this.dao.UpdateProductCategoryAsync(productCategory);
        }
    }
}