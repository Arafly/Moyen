using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moyen.Domain.Models;
using Moyen.Infrastructure.Errors;
using Moyen.Persistence.Contexts;

namespace Moyen.Features.Users {
    public class Create {
        public class UserData {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        // UserDatatValidator

        public class Command : IRequest<UserEnvelope> {
            public UserData User { get; set; }
        }

        // CommandValidator 

        public class Handler : IRequestHandler<Command, UserEnvelope> {
            // private readonly IPasswordHasher _passwordHasher
            // private readonly IJwtTokenGenerator _jwtTokenGenerator
            // private readonly IMapper _mapper
            private readonly MoyenContext _context;
            public Handler (MoyenContext context) {
                _context = context;
            }

            public Task<UserEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                throw new System.NotImplementedException();
            }

            // public async Task<UserEnvelope> Handle (Command message, CancellationToken cancellationToken) {
            // if (await _context.Persons.Where(x => x.Username == message.User.Username).AnyAsync(cancellationToken))
            // {
            //     throw new RestException(HttpStatusCode.BadRequest, new { Username = Constants.IN_USE });
            // }

            // if (await _context.Persons.Where(x => x.Email == message.User.Email).AnyAsync(cancellationToken))
            // {
            //     throw new RestException(HttpStatusCode.BadRequest, new { Email = Constants.IN_USE });
            // }

            // var person = new Person{
            //     Username = message.User.Username,
            //     Email = message.User.Email
            // };

            // _context.Persons.Add(person);
            // await _context.SaveChangesAsync(cancellationToken);

            // return new UserEnvelope();

            // }

        }

    }
}