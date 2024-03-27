using AutoMapper;
using JobLink.Business.Dtos.ReqruimentDtos;
using JobLink.Core.Entities;

namespace JobLink.Business.Profiles;

public class ReqruimentMappingProfile:Profile
{
    public ReqruimentMappingProfile()
    {
        CreateMap<CreateReqruimentDto, Reqruiment>().ReverseMap();
        CreateMap<UpdateReqruimentDto, Reqruiment>().ReverseMap();
        CreateMap<Reqruiment, ReqruimentDetailItemDto>().ReverseMap();
        CreateMap<Reqruiment, ReqruimentListItemDto>().ReverseMap();
    }
}


