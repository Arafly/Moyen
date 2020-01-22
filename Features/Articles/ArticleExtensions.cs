using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moyen.Domain.Models;

namespace Moyen.Features.Articles
{
    public static class ArticleExtensions
    {
        public static IQueryable<Article> GetAllData(this DbSet<Article> articles){
            return articles.Include(x => x.Author)
                            .Include(x => x.ArticleFaves)
                            .Include(x => x.ArticleTags)
                            .AsNoTracking();
        }
    }
}