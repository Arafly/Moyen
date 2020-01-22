using System;

namespace Moyen.Domain.Models {

    public class ArticleFave {
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}