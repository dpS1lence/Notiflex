using AutoMapper;
using Notiflex.Core.Models.DTOs;
using Notiflex.ViewModels;

namespace Notiflex.MapperProfiles
{
    public class DashboardMapperProfile : Profile
    {
        public DashboardMapperProfile()
        {
            CreateMap<WeatherCardDto, WeatherCardViewModel>();
        }
    }
}
