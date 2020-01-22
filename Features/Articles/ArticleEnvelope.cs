using Moyen.Domain.Models;

namespace Moyen.Features.Articles
{
    public class ArticleEnvelope
    {
        public ArticleEnvelope(Article article){
            Article = article;
        }

        public Article Article { get; }
    }
}