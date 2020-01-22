using System.Collections.Generic;
using System.Linq;
using Moyen.Domain.Models;

namespace Moyen.Persistence.Contexts {
    public class Seed {
        public static void SeedData (MoyenContext context) {
            if (!context.Articles.Any()) {
                var articles = new List<Article> {
                    new Article {
                        Title = "Test",
                            Description = "Testing testing",
                            Body = "Still testing",
                            // TagList = ["under, test"]
                    }
                };
                context.Articles.AddRange(articles);
                context.SaveChanges();
            }
        }
    }
}