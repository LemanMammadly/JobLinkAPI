using AutoMapper;
using JobLink.Business.Dtos.AbilityDtos;
using JobLink.Core.Entities;

namespace JobLink.Business.Profiles;

public class AbilityMappingProfile:Profile
{
    public AbilityMappingProfile()
    {
        CreateMap<Ability, AbilityListItemDto>().ReverseMap();
        CreateMap<Ability, AbilityDetailItemDto>().ReverseMap();
        CreateMap<Ability, AbilityInfoDto>().ReverseMap();
    }
}

