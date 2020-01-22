// using System.Threading;
// using System.Threading.Tasks;
// using MediatR;
// using Microsoft.EntityFrameworkCore;
// using Moyen.Persistence.Contexts;

// namespace Moyen.Features.Articles {
//     public class Delete {
//         public class Command : IRequest {

//             public Command (string slug) {
//                 // To know the slug of the article we want to delete
//                 Slug = slug;
//             }

//             public string Slug { get; set; }
//         }

//         public class Handler : IRequestHandler<Command> {
//             private readonly MoyenContext _context;

//             public Handler (MoyenContext context) {

//                 _context = context;
//             }

//             public async Task<Unit> Handle (Command message, CancellationToken cancellationToken) {
//                 var article = await _context.Articles.FirstOrDefaultAsync (x => x.Slug == message.Slug, cancellationToken);

//                 if (article == null) {
//                     // throw new RestException(HttpStatusCode.NotFound, new {Article= Constants.NOT_FOUND});
//                 }
//                 // Remove the deleted article from the context
//                 _context.Articles.Remove (article);
//                 await _context.SaveChangesAsync (cancellationToken);

//                 return Unit.Value;
//             }

//         }
//     }
// }