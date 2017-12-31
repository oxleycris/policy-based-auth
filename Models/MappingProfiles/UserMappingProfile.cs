using AutoMapper;
using PBA.Models.Users;

namespace PBA.Models.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ApplicationUser, User>();
        }
    }
}
