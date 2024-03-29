﻿using AutoMapper;
using Notiflex.Core.Models.DTOs;
using Notiflex.Infrastructure.Data.Models.UserModels;
using Notiflex.ViewModels;

namespace Notiflex.MapperProfiles
{
    public class AccountMapperProfile : Profile
    {
        public AccountMapperProfile() 
        {
            CreateMap<RegisterViewModel, RegisterDto>();
            CreateMap<NotiflexUser, ProfileDto>()
                .ForMember(a => a.TelegramChatId, b => b.MapFrom(src => src.TelegramInfo));
            CreateMap<ProfileDto, ProfileViewModel>();
        }
    }
}
