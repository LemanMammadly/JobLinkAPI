using AutoMapper;
using JobLink.Business.Dtos.IndustryDtos;
using JobLink.Core.Entities;

namespace JobLink.Business.Profiles;

public class IndustryMappingProfile:Profile
{
    public IndustryMappingProfile()
    {
        CreateMap<CreateIndustryDto, Industry>().ReverseMap();
        CreateMap<UpdateIndustryDto, Industry>().ReverseMap();
        CreateMap<IndustryListItemDto, Industry>().ReverseMap();
        CreateMap<IndustryDetailItemDto, Industry>().ReverseMap();
    }
}

