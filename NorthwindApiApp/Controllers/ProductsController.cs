using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Interfaces;
using Northwind.Services.Products;

namespace NorthwindApiApp.Controllers
{
    [ApiController]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManagementService service;
        public ProductsController(IProductManagementService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet(Name = "GetProducts")]
        public ActionResult<IEnumerable<Product>> GetAll(int offset, int limit)
        {
            return (List<Product>)this.service.ShowProducts(offset, limit);
        }

        [HttpGet("{id}", Name = "GetProductsById")]
        public ActionResult<Product> Get(int id)
        {
            this.service.TryShowProduct(id, out Product product);

            return product is null ? this.NotFound() : product;
        }

        [HttpPost(Name = "CreateProduct")]
        public IActionResult Create(Product product)
        {
            if (product is null)
            {
                return this.BadRequest();
            }

            this.service.CreateProduct(product);
            return this.CreatedAtAction(nameof(Create), product);
        }

        [HttpPut("{id}", Name = "UpdateProduct")]
        public IActionResult Update(int id, Product product)
        {
            if (product is null)
            {
                return this.NotFound();
            }

            var existingProduct = this.service.UpdateProduct(id, product);

            if (!existingProduct)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        [HttpDelete("{id}", Name = "DeleteProduct")]
        public IActionResult Delete(int id)
        {
            var result = this.service.DestroyProduct(id);

            if (!result)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
    }
}