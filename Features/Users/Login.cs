using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moyen.Infrastructure.Errors;
using Moyen.Infrastructure.Security;
using Moyen.Persistence.Contexts;

namespace Moyen.Features.Users {
        public class Login {
            public class UserData {
                public string Email { get; set; }
                public string Password { get; set; }
            }

            public class UserDataValidator : AbstractValidator<UserData> {
                public UserDataValidator () {
                    RuleFor (x => x.Email).NotNull ().NotEmpty ();
                    RuleFor (x => x.Password).NotNull ().NotEmpty ();
                }
            }

            public class Command : IRequest<UserEnvelope> {
                public UserData User { get; set; }
            }

            public class CommandValidator : AbstractValidator<Command> {

            }

            public class Handler : IRequestHandler<Command, UserEnvelope> {
                private readonly MoyenContext _context;
                private readonly IMapper _mapper;
                private readonly IPasswordHasher _passwordHasher;

                public Handler (MoyenContext context, IMapper mapper, IPasswordHasher passwordHasher) {
                    _context = context;
                    _mapper = mapper;
                    _passwordHasher = passwordHasher;
                }

                public async Task<UserEnvelope> Handle (Command message, CancellationToken cancellationToken) {
                    var person = await _context.Persons.Where (x => x.Email == message.User.Email).SingleOrDefaultAsync (cancellationToken);

                    if (person == null) {
                        throw new RestException (HttpStatusCode.Unauthorized, new { Error = "Invalid email/password" });
                    }
                    if (!person.Hash.SequenceEqual (_passwordHasher.Hash (message.User.Password, person.Salt))) {
                        throw new RestException (HttpStatusCode.Unauthorized, new { Error = "Invalid email / password." });
                    }

                    var user = _mapper.Map<Domain.Person, User> (person);
                    // user.Token = await _jwtTokenGenerator.CreateToken (person.Username);
                    return new UserEnvelope (user);
                }
            }
        }