using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Interfaces;

namespace NorthwindApiApp.Controllers
{
    [ApiController]
    [Route("picture")]
    public class ProductCategoriesPictureController : ControllerBase
    {
        private readonly IProductCategoryPicturesService service;
        public ProductCategoriesPictureController(IProductCategoryPicturesService service)
        {
            this.service = service;
        }

        [HttpGet("{id}", Name = "GetPicture")]
        public ActionResult<byte[]> GetPicture(int id)
        {
            this.service.TryShowPicture(id, out byte[] picture);

            return picture is null ? this.NotFound() : picture;
        }

        [HttpPut("{id}", Name = "UploadPicture")]
        public IActionResult Update(int id, IFormFile file)
        {
            if (file is null)
            {
                return this.NotFound();
            }

            if (file.Length <= 0)
            {
                return this.NotFound();
            }

            using (Stream stream = new MemoryStream())
            {
                file.CopyTo(stream);
                return this.service.UpdatePicture(id, stream) ? this.NoContent() : this.NotFound();
            }
        }

        [HttpDelete("{id}", Name = "DeletePicture")]
        public IActionResult Delete(int id)
        {
            return this.service.DestroyPicture(id) ? this.NoContent() : this.NotFound();
        }
    }
}
