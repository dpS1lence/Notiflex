using AutoMapper;
using Notiflex.Core.Models.DTOs;
using Notiflex.ViewModels;

namespace Notiflex.MapperProfiles
{
    public class AccountMapperProfile : Profile
    {
        public AccountMapperProfile() 
        {
            CreateMap<RegisterViewModel, UserDto>();
        }
    }
}
