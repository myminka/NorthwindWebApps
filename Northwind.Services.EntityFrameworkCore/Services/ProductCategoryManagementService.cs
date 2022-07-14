using System.Collections.Generic;
using System.Linq;
using Northwind.Services.Interfaces;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFramework.Services
{
    public class ProductCategoryManagementService : IProductCategoryManagementService
    {
        private readonly NorthwindContext context;

        public ProductCategoryManagementService()
        {
            this.context = new NorthwindContext();
        }

        /// <inheritdoc/>
        public int CreateCategory(ProductCategory productCategory)
        {
            productCategory.Id = this.context.CategoryId++;
            this.context.Categories.Add(productCategory);
            return productCategory.Id;
        }

        /// <inheritdoc/>
        public bool DestroyCategory(int categoryId)
        {
            var res = this.context.Categories.Any(c => c.Id == categoryId);
            this.context.Categories.RemoveAll(c => c.Id == categoryId);
            return res;
        }

        /// <inheritdoc/>
        public IList<ProductCategory> LookupCategoriesByName(IList<string> names)
        {
            return this.context.Categories.Where(c => names.Contains(c.Name)).ToList();
        }

        /// <inheritdoc/>
        public IList<ProductCategory> ShowCategories(int offset, int limit)
        {
            return this.context.Categories.ToList();
        }

        /// <inheritdoc/>
        public bool TryShowCategory(int categoryId, out ProductCategory productCategory)
        {
            var res = this.context.Categories.Any(c => c.Id == categoryId);
            productCategory = res ? this.context.Categories.First(c => c.Id == categoryId) : null;
            return res;
        }

        /// <inheritdoc/>
        public bool UpdateCategories(int categoryId, ProductCategory productCategory)
        {
            var index = this.context.Categories.FindIndex(c => c.Id == categoryId);

            if (index != -1)
            {
                productCategory.Id = this.context.CategoryId++;
                this.context.Categories[index] = productCategory;
            }

            return index != -1;
        }
    }
}
