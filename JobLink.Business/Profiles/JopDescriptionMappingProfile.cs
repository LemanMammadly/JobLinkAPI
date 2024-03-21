using AutoMapper;
using JobLink.Business.Dtos.JobDescriptionDtos;
using JobLink.Core.Entities;

namespace JobLink.Business.Profiles;

public class JopDescriptionMappingProfile:Profile
{
    public JopDescriptionMappingProfile()
    {
        CreateMap<CreateJobDescriptionDto, JobDescription>().ReverseMap();
        CreateMap<UpdateJobDescriptionDto, JobDescription>().ReverseMap();
        CreateMap<JobDescription, JobDescriptionDetailDto>().ReverseMap();
        CreateMap<JobDescription, JobDescriptionListItemDto>().ReverseMap();
    }
}

