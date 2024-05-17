using AutoMapper;
using JobLink.Business.Constants;
using JobLink.Business.Dtos.CategoryDtos;
using JobLink.Business.Exceptions.Common;
using JobLink.Business.Exceptions.FileExceptions;
using JobLink.Business.Extensions;
using JobLink.Business.ExternalServices.Interfaces;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using JobLink.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace JobLink.Business.Services.Implements;

public class CategoryService : ICategoryService
{
    readonly ICategoryRepository _repo;
    readonly IMapper _mapper;
    readonly IFileService _fileService;
    readonly IConfiguration _config;

    public CategoryService(ICategoryRepository repo, IMapper mapper, IFileService fileService, IConfiguration config)
    {
        _repo = repo;
        _mapper = mapper;
        _fileService = fileService;
        _config = config;
    }

    public async Task CreateAsync(CreateCategoryDto dto)
    {
        if (await _repo.IsExistAsync(c => c.Name == dto.Name)) throw new IsAlreadyExistException<Category>();

        if (!dto.LogoFile.IsSizeValid(3)) throw new SizeNotValidException();
        if (!dto.LogoFile.IsTypeValid("image")) throw new TypeNotValidException();

        var category = _mapper.Map<Category>(dto);
        category.Logo = await _fileService.UploadAsync(dto.LogoFile, RootConstant.CategoryLogoRoot);

        await _repo.CreateAsync(category);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Category>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Category>();

        _fileService.Delete(entity.Logo);
        _repo.Delete(entity);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<CategoryListItemDto>> GetAllAsync(bool takeAll)
    {
        if(takeAll)
        {
            var entities = _repo.GetAll("Advertisements");
            var map = _mapper.Map<IEnumerable<CategoryListItemDto>>(entities);
            foreach (var item in map)
            {
                item.Logo = _config["Jwt:Issuer"] + "wwwroot/" + item.Logo;
            }
            return map;
        }
        else
        {
            var entites = _repo.FindAll(c => c.IsDeleted == false, "Advertisements");
            var map = _mapper.Map<IEnumerable<CategoryListItemDto>>(entites);
            foreach (var item in map)
            {
                item.Logo = _config["Jwt:Issuer"] + "wwwroot/" + item.Logo;
            }
            return map;
        }
    }

    public async Task<CategoryDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Category>();
        Category? category;

        if(takeAll)
        {
            category = await _repo.GetByIdAsync(id, "Advertisements");
            if (category is null) throw new NotFoundException<Category>();
        }
        else
        {
            category = await _repo.GetSingleAsync(c => c.IsDeleted == false && c.Id == id, "Advertisements");
            if (category is null) throw new NotFoundException<Category>();
        }

        var map = _mapper.Map<CategoryDetailItemDto>(category);
        map.Logo = _config["Jwt:Issuer"] + "wwwroot/" + map.Logo;

        return map;
    }

    public async Task ReverteSoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Category>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Category>();

        _repo.ReverteSoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Category>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Category>();

        _repo.SoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id,UpdateCategoryDto dto)
    {
        if (id <= 0) throw new NegativeIdException<Category>();
        var category = await _repo.GetByIdAsync(id);
        if (category is null) throw new NotFoundException<Category>();

        if (await _repo.IsExistAsync(c => c.Name == dto.Name && c.Id != id)) throw new IsAlreadyExistException<Category>();

        if(dto.LogoFile != null)
        {
            _fileService.Delete(category.Logo);
            if (!dto.LogoFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.LogoFile.IsTypeValid("image")) throw new TypeNotValidException();
            category.Logo = await _fileService.UploadAsync(dto.LogoFile, RootConstant.CategoryLogoRoot);
        }

        _mapper.Map(dto, category);
        await _repo.SaveAsync();
    }
}

