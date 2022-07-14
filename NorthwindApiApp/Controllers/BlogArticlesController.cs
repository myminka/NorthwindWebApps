using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.Services.Blogging;

namespace NorthwindApiApp.Controllers
{
    [ApiController]
    [Route("articles")]
    public class BlogArticlesController : ControllerBase
    {
        private readonly IBloggingService _service;
        public BlogArticlesController(IBloggingService service)
        {
            this._service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet(Name = "GetArticles")]
        public async Task<ActionResult<List<BlogArticle>>> Get()
        {
            throw new NotImplementedException();
        }

        [HttpPost(Name = "CreateArticles")]
        public async Task<IActionResult> Create(BlogArticle blogArticle)
        {
            if (blogArticle is null)
            {
                return this.BadRequest();
            }

            await this._service.CreateBlogArticle(blogArticle);

            return this.CreatedAtAction(nameof(Create), blogArticle);
        }
    }
}