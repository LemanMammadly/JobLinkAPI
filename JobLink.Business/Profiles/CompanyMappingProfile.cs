using AutoMapper;
using JobLink.Business.Dtos.CompanyDtos;
using JobLink.Core.Entities;

namespace JobLink.Business.Profiles;

public class CompanyMappingProfile:Profile
{
    public CompanyMappingProfile()
    {
        CreateMap<CreateCompanyDto, Company>().ReverseMap();
        CreateMap<CompanyListItemDto, Company>().ReverseMap();
        CreateMap<CompanyIndustryDto, CompanyIndustry>().ReverseMap();
    }
}

