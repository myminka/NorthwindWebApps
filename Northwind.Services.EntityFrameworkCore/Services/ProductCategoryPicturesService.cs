using System;
using System.IO;
using System.Linq;
using Northwind.Services.Interfaces;
using Northwind.Services.Products;

namespace Northwind.Services.EntityFramework.Services
{
    public class ProductCategoryPicturesService : IProductCategoryPicturesService
    {
        private readonly NorthwindContext context;
        public ProductCategoryPicturesService()
        {
            this.context = new NorthwindContext();
            this.context.Categories.Add(new ProductCategory { Description = "asd", Id = 0, Name = "Pizza" });
        }

        /// <inheritdoc/>
        public bool DestroyPicture(int categoryId)
        {
            var res = this.context.Categories.Any(c => c.Id == categoryId);
            this.context.Categories.Where(c => c.Id == categoryId).First().Picture = Array.Empty<byte>();
            return res;
        }

        /// <inheritdoc/>
        public bool TryShowPicture(int categoryId, out byte[] bytes)
        {
            var res = this.context.Categories.Any(c => c.Id == categoryId);
            bytes = res ? this.context.Categories.First(c => c.Id == categoryId).Picture : null;
            return res;
        }

        /// <inheritdoc/>
        public bool UpdatePicture(int categoryId, Stream stream)
        {
            var index = this.context.Categories.FindIndex(c => c.Id == categoryId);
            var res = index == -1;

            if (res)
            {
                byte[] array;
                using (MemoryStream s = new MemoryStream())
                {
                    stream.CopyTo(s);
                    array = s.ToArray();
                }

                this.context.Categories[index].Picture = array;
            }

            return res;
        }
    }
}
