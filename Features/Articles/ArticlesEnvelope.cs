using System.Collections.Generic;
using Moyen.Domain.Models;

namespace Moyen.Features.Articles
{
    public class ArticlesEnvelope
    {
        public List<Article> Articles { get; set; }

        public int ArticlesCount { get; set; }
    }
}