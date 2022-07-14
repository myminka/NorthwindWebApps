using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Interfaces;
using Northwind.Services.Products;

namespace NorthwindApiApp.Controllers
{
    [ApiController]
    [Route("categories")]
    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryManagementService service;
        public ProductCategoriesController(IProductCategoryManagementService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet(Name = "GetProductCategories")]
        public ActionResult<IEnumerable<ProductCategory>> GetAll(int offset, int limit)
        {
            return (List<ProductCategory>)this.service.ShowCategories(offset, limit);
        }

        [HttpGet("{id}", Name = "GetProductCategoriesById")]
        public ActionResult<ProductCategory> Get(int id)
        {
            this.service.TryShowCategory(id, out ProductCategory category);

            return category is null ? this.NotFound() : category;
        }

        [HttpPost(Name = "CreateProductCategory")]
        public IActionResult Create(ProductCategory category)
        {
            if (category is null)
            {
                return this.BadRequest();
            }

            this.service.CreateCategory(category);
            return this.CreatedAtAction(nameof(Create), category);
        }

        [HttpPut("{id}", Name = "UpdateProductCategory")]
        public IActionResult Update(int id, ProductCategory category)
        {
            if (category is null)
            {
                return this.NotFound();
            }

            return this.service.UpdateCategories(id, category) ? this.NoContent() : this.NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteProductCategory")]
        public IActionResult Delete(int id)
        {
            return this.service.DestroyCategory(id) ? this.NoContent() : this.NotFound();
        }
    }
}