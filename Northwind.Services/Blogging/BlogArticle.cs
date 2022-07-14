using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Services.Blogging
{
    public class BlogArticle
    {
        [Key]
        [Column("blog_article_id")]
        public int BlogArticleId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("created")]
        public DateTime Created { get; set; }

        [Column("employee_id")]
        public int EmployeeId { get; set; }
    }
}
