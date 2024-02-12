using AutoMapper;
using JobLink.Business.Dtos.AdvertisementDtos;
using JobLink.Core.Entities;

namespace JobLink.Business.Profiles;

public class AdvertisementMappingProfile:Profile
{
    public AdvertisementMappingProfile()
    {
        CreateMap<UpdateAdvertisementDto, Advertisement>().ReverseMap();
        CreateMap<CreateAdvertisementDto, Advertisement>().ReverseMap();
        CreateMap<Advertisement, AdvertisementListItemDto>().ReverseMap();
        CreateMap<Advertisement, AdvertisementDetailItemDto>().ReverseMap();
        CreateMap<Advertisement, AdvertisementInfoDto>().ReverseMap();
    }
}


