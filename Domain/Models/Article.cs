using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;

namespace Moyen.Domain.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Body { get; set; }
        public Person Author { get; set; }
        public List<Comment> Comments { get; set; }

        [JsonIgnore]
        public List<ArticleFave> ArticleFaves { get; set; }

        [NotMapped]
        public bool Faved => ArticleFaves?.Any() ?? false;

        [NotMapped]
        public int FavouritesCount => ArticleFaves?.Count ?? 0;

        [NotMapped]
        public List<string> TagList => (ArticleTags?.Select(x => x.TagId) ?? Enumerable.Empty<string>()).ToList();

        [JsonIgnore]
        public List<ArticleTag> ArticleTags { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
