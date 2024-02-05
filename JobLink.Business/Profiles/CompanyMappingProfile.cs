using AutoMapper;
using JobLink.Business.Dtos.CompanyDtos;
using JobLink.Business.Dtos.IndustryDtos;
using JobLink.Core.Entities;

namespace JobLink.Business.Profiles;

public class CompanyMappingProfile:Profile
{
    public CompanyMappingProfile()
    {
        CreateMap<CreateCompanyDto, Company>().ReverseMap();
        CreateMap<CompanyListItemDto, Company>().ReverseMap();
        CreateMap<CompanyDetailItemDto, Company>().ReverseMap();
        CreateMap<CompanyUpdateDto, Company>().ReverseMap();
        CreateMap<Dtos.CompanyDtos.CompanyIndustryDto, CompanyIndustry>().ReverseMap();
        CreateMap<CompanyInfoDto, Company>().ReverseMap();
        CreateMap<IndustryInfoDto, Industry>().ReverseMap();
    }
}

