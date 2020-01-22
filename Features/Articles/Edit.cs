using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moyen.Domain.Models;
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
                    throw new RestException (HttpStatusCode.NotFound, new { Article = Constants.NOT_FOUND });
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
                _context.ArticleTags.RemoveRange (articleTagsToDelete);

                await _context.SaveChangesAsync (cancellationToken);

                return new ArticleEnvelope (await _context.Articles.GetAllData ()
                    .Where (x => x.Slug == article.Slug)
                    .FirstOrDefaultAsync (cancellationToken));
            }

            /// <summary>
            /// get the list of Tags to be added
            /// </summary>
            /// <param name="articleTagList"></param>
            /// <returns></returns>
            public async Task<List<Tag>> GetTagsToCreate (IEnumerable<string> articleTagList) {
                var tagsToCreate = new List<Tag> ();

                foreach (var tag in articleTagList) {
                    var t = await _context.Tags.FindAsync (tag);
                    if (t == null) {
                        t = new Tag () {
                        TagId = tag
                        };
                        tagsToCreate.Add (t);
                    }
                }

                return tagsToCreate;
            }

            /// <summary>
            /// check which article tags need to be added
            /// </summary>
            static List<ArticleTag> GetArticleTagsToCreate (Article article, IEnumerable<string> articleTagList){
                var articleTagsToCreate = new List<ArticleTag>();
                foreach(var tag in articleTagList){
                    var ag = article.ArticleTags.FirstOrDefault(t => t.TagId == tag);
                    if(ag == null){
                        ag = new ArticleTag(){
                            Article = article,
                            ArticleId = article.ArticleId,
                            Tag = new Tag() { TagId = tag },
                            TagId = tag
                        };
                        articleTagsToCreate.Add(ag);
                    }
                }

                return articleTagsToCreate;
            }

            /// <summary>
            /// check which article tags needs to be deleted
            /// </summary>
            static List<ArticleTag> GetArticleTagsToDelete (Article article, IEnumerable<string> articleTagList){
                var articleTagsToDelete = new List<ArticleTag>();
                foreach(var tag in article.ArticleTags){
                    var ag = articleTagList.FirstOrDefault(t => t == tag.TagId);
                    if(ag == null){
                        articleTagsToDelete.Add(tag);
                    }
                }

                return articleTagsToDelete;
            }

        }

    }
}