using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Services.Blogging
{
    public interface IBloggingService
    {
        public Task<int> CreateBlogArticle(BlogArticle article);

        public Task<List<BlogArticle>> GetAll();

        public Task<bool> GetById(int id, out BlogArticle article);

        public Task<int> DeleteBlogArticle(int id);

        public Task<bool> UpdateBlogArticle(int id, BlogArticle article);
    }
}