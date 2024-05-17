using AutoMapper;
using JobLink.Business.Constants;
using JobLink.Business.Dtos.IndustryDtos;
using JobLink.Business.Exceptions.Common;
using JobLink.Business.Exceptions.FileExceptions;
using JobLink.Business.Exceptions.IndustryExceptions;
using JobLink.Business.Extensions;
using JobLink.Business.ExternalServices.Interfaces;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using JobLink.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JobLink.Business.Services.Implements;

public class IndustryService : IIndustryService
{
    readonly IIndustryRepository _repo;
    readonly IMapper _mapper;
    readonly IFileService _fileService;
    readonly IAdvertisementService advertisementService;
    readonly IConfiguration _config;

    public IndustryService(IIndustryRepository repo, IMapper mapper, IFileService fileService, IAdvertisementService advertisementService, IConfiguration config)
    {
        _repo = repo;
        _mapper = mapper;
        _fileService = fileService;
        this.advertisementService = advertisementService;
        _config = config;
    }

    public async Task CreateAsync(CreateIndustryDto dto)
    {
        if (await _repo.IsExistAsync(i => i.Name == dto.Name)) throw new IndustryAlreadyIsExistException();

        if (!dto.LogoFile.IsSizeValid(3)) throw new SizeNotValidException();
        if (!dto.LogoFile.IsTypeValid("image")) throw new TypeNotValidException();

        var industry = _mapper.Map<Industry>(dto);
        industry.Logo = await _fileService.UploadAsync(dto.LogoFile, RootConstant.IndustryLogoRoot);

        await _repo.CreateAsync(industry);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Industry>();
        var entity = await _repo.GetSingleAsync(e => e.Id == id, "CompanyIndustries", "CompanyIndustries.Company");
        if (entity is null) throw new IndustryNotFoundException();


        foreach (var ent in entity.CompanyIndustries)
        {
            if (ent.Company != null)
            {
                throw new CompanyIsExistInIndustryException();
            }
        }

        _repo.Delete(entity);
        _fileService.Delete(entity.Logo);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<IndustryListItemDto>> GetAllAsync(bool takeAll)
    {
        if (takeAll)
        {
            var entities = _repo.GetAll("CompanyIndustries", "CompanyIndustries.Company", "CompanyIndustries.Company.Advertisements");
            var map = _mapper.Map<IEnumerable<IndustryListItemDto>>(entities);
            foreach (var item in map)
            {
                item.Logo = _config["Jwt:Issuer"] + "wwwroot/" + item.Logo;
            }
            return map;
        }
        else
        {
            var entities = _repo.FindAll(i => i.IsDeleted == false, "CompanyIndustries", "CompanyIndustries.Company" , "CompanyIndustries.Company.Advertisements");
            var map = _mapper.Map<IEnumerable<IndustryListItemDto>>(entities);
            foreach (var item in map)
            {
                item.Logo = _config["Jwt:Issuer"] + "wwwroot/" + item.Logo;
            }
            return map;
        }
    }

    public async Task<IndustryDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Industry>();
        Industry? entity;

        if(takeAll)
        {
            entity = await _repo.GetByIdAsync(id, "CompanyIndustries", "CompanyIndustries.Company");
            if (entity is null) throw new IndustryNotFoundException();
        }
        else
        {
            entity = await _repo.GetSingleAsync(i => i.Id == id && i.IsDeleted == false, "CompanyIndustries", "CompanyIndustries.Company");
            if (entity is null) throw new IndustryNotFoundException();
        }

        return _mapper.Map<IndustryDetailItemDto>(entity);
    }

    public async Task ReverteSoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Industry>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new IndustryNotFoundException();

        _repo.ReverteSoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Industry>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new IndustryNotFoundException();

        _repo.SoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id,UpdateIndustryDto dto)
    {
        if (id <= 0) throw new NegativeIdException<Industry>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new IndustryNotFoundException();

        if (await _repo.IsExistAsync(i => i.Name == dto.Name && i.Id != id)) throw new IndustryAlreadyIsExistException();

        if(dto.LogoFile!=null)
        {
            _fileService.Delete(entity.Logo);
            if (!dto.LogoFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.LogoFile.IsTypeValid("image")) throw new TypeNotValidException();
            entity.Logo = await _fileService.UploadAsync(dto.LogoFile, RootConstant.IndustryLogoRoot);
        }

        var newEntity = _mapper.Map(dto,entity);
        await _repo.SaveAsync();
    }
}

