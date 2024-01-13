using AutoMapper;
using JobLink.Business.Dtos.AppUserDtos;
using JobLink.Core.Entities;

namespace JobLink.Business.Profiles;

public class AppUserMappingProfile:Profile
{
    public AppUserMappingProfile()
    {
        CreateMap<RegisterDto, AppUser>().ReverseMap();
        CreateMap<UpdateDto, AppUser>().ReverseMap();
        CreateMap<AppUser, UserListItemDto>().ReverseMap();
        CreateMap<AppUser, UserDetailItemDto>().ReverseMap();
    }
}

