using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moyen.Domain.Models;
using Moyen.Persistence.Contexts;

namespace Moyen.Features.Articles
{
    public class Details
    {
        public class Query : IRequest<ArticleEnvelope>
        {
            public Query(string slug){
                Slug = slug;
            }

            public string Slug { get; set; }
        }

        public class Handler : IRequestHandler<Query, ArticleEnvelope>
        {
            private readonly MoyenContext _context;
            public Handler(MoyenContext context)
            {
                _context = context;
            }
            public async Task<ArticleEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var article = await _context.Articles.GetAllData().FirstOrDefaultAsync(x => x.Slug == message.Slug, cancellationToken);

                if (article == null){
                    // throw new RestException(HttpStatusCode.NotFound, new {Article= Constants.NOT_FOUND});
                }

                return new ArticleEnvelope(article);
            }

        }
    }
}