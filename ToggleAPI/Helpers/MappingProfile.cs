using AutoMapper;
using ToggleAPI.Models;
using ToggleAPI.Dtos;

namespace ToggleAPI.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>();
        }        
    }
}