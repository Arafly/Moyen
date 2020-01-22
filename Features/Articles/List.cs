using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moyen.Domain.Models;
using Moyen.Infrastructure;
using Moyen.Persistence.Contexts;

namespace Moyen.Features.Articles
{
    public class List
    {
        public class Query : IRequest<ArticlesEnvelope> {
            public Query(string tag, string author, string faved)
            {
                Tag = tag;
                Author = author;
                FavoritedUsername = faved;
                // Limit = limit;
                // Offset = offset;
            }

            public string Tag { get; }
            public string Author { get; }
            public string FavoritedUsername { get; }
            // public int? Limit { get; }
            // public int? Offset { get; }
            public bool IsFeed { get; set; }
        }

        public class Handler : IRequestHandler<Query, ArticlesEnvelope>{
            private readonly MoyenContext _context;
            private readonly ICurrentUser _currentUser;

            public Handler(MoyenContext context, ICurrentUser currentUser){
                _context = context;
                _currentUser = currentUser;
            }

            public async Task<ArticlesEnvelope> Handle(Query message, CancellationToken cancellationToken) {
                IQueryable<Article> queryable = _context.Articles.GetAllData();

                // if (message.IsFeed && _currentUser.GetCurrentUsername() != null)
                // {
                //     var currentUser = await _context.Persons.Include(x => x.Following).FirstOrDefaultAsync(x => x.Username == _currentUserAccessor.GetCurrentUsername(), cancellationToken);
                //     queryable = queryable.Where(x => currentUser.Following.Select(y => y.TargetId).Contains(x.Author.PersonId));
                // }

                if(!string.IsNullOrWhiteSpace(message.Tag)){
                    var tag = await _context.ArticleTags.FirstOrDefaultAsync(x => x.TagId == message.Tag, cancellationToken);
                    if(tag != null){
                        queryable = queryable.Where(x => x.ArticleTags.Select(y => y.TagId).Contains(tag.TagId));
                    }else {
                        return new ArticlesEnvelope();
                    }
                }

                // var articles = await queryable

                return new ArticlesEnvelope(){
                    // Articles = articles,
                    ArticlesCount = queryable.Count()
                };

            }
        }
    }
}