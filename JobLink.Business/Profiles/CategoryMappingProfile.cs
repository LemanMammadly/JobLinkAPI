using AutoMapper;
using JobLink.Business.Dtos.CategoryDtos;
using JobLink.Core.Entities;

namespace JobLink.Business.Profiles;

public class CategoryMappingProfile:Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<CreateCategoryDto, Category>().ReverseMap();
        CreateMap<UpdateCategoryDto, Category>().ReverseMap();
        CreateMap<CategoryListItemDto, Category>().ReverseMap();
        CreateMap<CategoryDetailItemDto, Category>().ReverseMap();
        CreateMap<CategoryInfoDto, Category>().ReverseMap();
    }
}

