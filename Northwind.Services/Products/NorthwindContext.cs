using System.Collections.Generic;

namespace Northwind.Services.Products
{
    public class NorthwindContext
    {
        public NorthwindContext()
        {
            this.Products = new List<Product>();
            this.Categories = new List<ProductCategory>();
            this.Products.Add(new Product() { Name = "Table" });
            this.Categories.Add(new ProductCategory() { Name = "Pizza", Id = 0, Description = "Yummy" });

            this.ProductId = this.Products.Count;
            this.CategoryId = this.Categories.Count;
        }

        public List<Product> Products { get; }

        public List<ProductCategory> Categories { get; }

        public int ProductId { get; set; }

        public int CategoryId { get; set; }
    }
}
