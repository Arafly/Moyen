using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moyen.Domain.Models;
using Moyen.Infrastructure;
using Moyen.Persistence.Contexts;

namespace Moyen.Features.Articles {
    public class Create {
        public class ArticleData {
            public string Title { get; set; }

            public string Description { get; set; }

            public string Body { get; set; }

            // public string[] TagList { get; set; }
        }

        // ArticleDataValidator

        public class Command : IRequest<ArticleEnvelope> {
            public ArticleData Article { get; set; }
        }

        //CommandValidator

        public class Handler : IRequestHandler<Command, ArticleEnvelope> {
            private readonly MoyenContext _context;
            private readonly ICurrentUser _currentUser;
            public Handler (MoyenContext context, ICurrentUser currentUser) {
                _context = context;
                _currentUser = currentUser;
            }

            // public async Task<ArticleEnvelope> Handle (Command message, CancellationToken cancellationToken) {
            //     // var author = await _context.Persons.FirstAsync (x => x.Username == _currentUser.GetCurrentUsername (), cancellationToken);

            //     // var tags = new List<Tag> ();
            //     // foreach (var tag in (message.Article.TagList ?? Enumerable.Empty<string> ())) {
            //     //     var t = await _context.Tags.FindAsync (tag);
            //     //     if (t == null) {
            //     //         t = new Tag () {
            //     //         TagId = tag
            //     //         };
            //     //         await _context.Tags.AddAsync (t, cancellationToken);
            //     //         // Save immediately for reuse
            //     //         await _context.SaveChangesAsync (cancellationToken);
            //     //     }
            //     //     tags.Add (t);
            //     // }

            //     var article = new Article () {
            //         // Author = author,
            //      //   Body = message.Article.Body,
            //      //   Description = message.Article.Title,
            //         CreatedAt = DateTime.UtcNow,
            //         UpdatedAt = DateTime.UtcNow,
            //         // Slug = message.Article.Title.GenerateSlug ()

            //     };

            //     await _context.Articles.AddAsync (article, cancellationToken);

            //     // await _context.ArticleTags.AddRangeAsync (tags.Select (x => new ArticleTag () {

            //     //     Article = article,
            //     //         Tag = x
            //     // }), cancellationToken);

            //     await _context.SaveChangesAsync (cancellationToken);

            //     return new ArticleEnvelope (article);
            // }

            public async Task<ArticleEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                var article = new Article()
                {
                    // Id = request.Id,
                    Title = request.Article.Title,
                    Body = request.Article.Body,
                    Description = request.Article.Description,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await _context.Articles.AddAsync (article, cancellationToken);
                await _context.SaveChangesAsync (cancellationToken);

                return new ArticleEnvelope (article);
            }

        }

    }
}