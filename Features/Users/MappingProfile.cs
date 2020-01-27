using AutoMapper;
using Moyen.Domain.Models;

namespace Moyen.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile(){
            CreateMap<Domain.Person, User>(MemberList.None);
        }
    }
}