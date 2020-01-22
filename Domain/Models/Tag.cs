using System;
using System.Collections.Generic;

namespace Moyen.Domain.Models
{
    public class Tag
    {
        public string TagId { get; set; }
        public List<ArticleTag> ArticleTags { get; set; }
    }
}
