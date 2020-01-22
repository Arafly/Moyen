using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moyen.Infrastructure;
using Moyen.Infrastructure.Errors;
using Moyen.Persistence.Contexts;

namespace Moyen.Features.Articles {
    public class Edit {
        public class ArticleData {
            public string Title { get; set; }

            public string Description { get; set; }

            public string Body { get; set; }

            public string[] TagList { get; set; }
        }

        public class Command : IRequest<ArticleEnvelope> {
            public ArticleData Article { get; set; }
            public string Slug { get; set; }
        }

        public class Handler : IRequestHandler<Command, ArticleEnvelope> {
            private readonly MoyenContext _context;

            public Handler (MoyenContext context) {
                _context = context;
            }

            public async Task<ArticleEnvelope> Handle (Command message, CancellationToken cancellationToken) {
                var article = await _context.Articles.Include (x => x.ArticleTags)
                    .Where (x => x.Slug == message.Slug)
                    .FirstOrDefaultAsync (cancellationToken);

                if (article == null) {
                    // throw new RestException(HttpStatusCode.NotFound, new {Article= Constants.NOT_FOUND});
                }

                // A user can decide to edit only one or more of the field
                article.Description = message.Article.Description ?? article.Description;
                article.Body = message.Article.Body ?? article.Body;
                article.Title = message.Article.Title ?? article.Title;
                article.Slug = article.Title.GenerateSlug ();

                // List of currently saved article tags for the given article
                var articleTagList = (message.Article.TagList ?? Enumerable.Empty<string> ());
                var articleTagsToCreate = GetArticleTagsToCreate (article, articleTagList);
                var articleTagsToDelete = GetArticleTagsToDelete (article, articleTagList);

                if (_context.ChangeTracker.Entries ().First (x => x.Entity == article).State == EntityState.Modified || articleTagsToCreate.Any () || articleTagsToDelete.Any ()) {
                    article.UpdatedAt = DateTime.UtcNow;
                }

                // Add the new article tags
                await _context.ArticleTags.AddRangeAsync (articleTagsToCreate, cancellationToken);
                //delete the tags that do not exist anymore
                await _context.ArticleTags.RemoveRange (articleTagsToDelete);

                await _context.SaveChangesAsync (cancellationToken);

                return new ArticleEnvelope (await _context.Articles.GetAllData()
                    .Where (x => x.Slug == article.Slug)
                    .FirstOrDefaultAsync (cancellationToken));
            }
        }

    }
}