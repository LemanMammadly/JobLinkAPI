using AutoMapper;
using JobLink.Business.Constants;
using JobLink.Business.Dtos.CompanyDtos;
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

namespace JobLink.Business.Services.Implements;

public class CompanyService : ICompanyService
{
    readonly ICompanyRepository _repo;
    readonly IIndustryRepository _industryRepository;
    readonly IMapper _mapper;
    readonly IFileService _fileService;

    public CompanyService(ICompanyRepository repo, IIndustryRepository industryRepository, IMapper mapper, IFileService fileService)
    {
        _repo = repo;
        _industryRepository = industryRepository;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task CreateAsync(CreateCompanyDto dto)
    {
        if (await _repo.IsExistAsync(c => c.Name == dto.Name)) throw new IsAlreadyExistException<Company>();

        if (dto.LogoFile != null)
        {
            if (!dto.LogoFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.LogoFile.IsTypeValid("image")) throw new TypeNotValidException();
        }

        List<CompanyIndustry> companyIndustries = new List<CompanyIndustry>();
        foreach (var id in dto.IndustryIds)
        {
            var industry = await _industryRepository.GetByIdAsync(id);
            if (industry == null) throw new IndustryNotFoundException();
            companyIndustries.Add(new CompanyIndustry { Industry = industry });
        }

        var company = _mapper.Map<Company>(dto);
        company.CompanyIndustries = companyIndustries;
        if (dto.LogoFile != null)
        {
            company.Logo = await _fileService.UploadAsync(dto.LogoFile, RootConstant.CompanyLogoRoot);
        }

        await _repo.CreateAsync(company);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Company>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Company>();

        if(entity.Logo != null)
        {
            _fileService.Delete(entity.Logo);
        }

        _repo.Delete(entity);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<CompanyListItemDto>> GetAllAsync(bool takeAll)
    {
        if(takeAll)
        {
            var entities = _repo.GetAll("CompanyIndustries", "CompanyIndustries.Industry");
            return _mapper.Map<IEnumerable<CompanyListItemDto>>(entities);
        }
        else
        {
            var entities = _repo.FindAll(c=>c.IsDeleted==false,"CompanyIndustries", "CompanyIndustries.Industry");
            return _mapper.Map<IEnumerable<CompanyListItemDto>>(entities);
        }
    }

    public async Task<CompanyDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Company>();
        Company? entity;

        if(takeAll)
        {
            entity = await _repo.GetByIdAsync(id, "CompanyIndustries", "CompanyIndustries.Industry");
            if (entity is null) throw new NotFoundException<Company>();
        }
        else
        {
            entity = await _repo.GetSingleAsync(c => c.Id == id && c.IsDeleted == false, "CompanyIndustries", "CompanyIndustries.Industry");
            if (entity is null) throw new NotFoundException<Company>();
        }
        return _mapper.Map<CompanyDetailItemDto>(entity);
    }

    public async Task ReverteSoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Company>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Company>();

        _repo.ReverteSoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Company>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Company>();

        _repo.SoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id, CompanyUpdateDto dto)
    {
        if (id <= 0) throw new NegativeIdException<Company>();
        var entity = await _repo.GetByIdAsync(id, "CompanyIndustries", "CompanyIndustries.Industry");
        if (entity is null) throw new NotFoundException<Company>();
        if (await _repo.IsExistAsync(c => c.Name == dto.Name && c.Id != id)) throw new IsAlreadyExistException<Company>();
        if (dto.LogoFile != null)
        {
            if(entity.Logo!=null)
            {
                _fileService.Delete(entity.Logo);
            }
            if (!dto.LogoFile.IsSizeValid(3)) throw new SizeNotValidException();
            if (!dto.LogoFile.IsTypeValid("image")) throw new TypeNotValidException();
            entity.Logo = await _fileService.UploadAsync(dto.LogoFile, RootConstant.CompanyLogoRoot);
        }
        List<CompanyIndustry> companyIndustries = new List<CompanyIndustry>();
        if (dto.IndustryIds != null) 
        {
            foreach (var ids in dto.IndustryIds)
            {
                var industry = await _industryRepository.GetByIdAsync(ids, "CompanyIndustries", "CompanyIndustries.Company");
                if (industry is null) throw new NotFoundException<Industry>();
                companyIndustries.Add(new CompanyIndustry { Industry=industry});
            }
        }
        var newEntity = _mapper.Map(dto, entity);
        if(dto.IndustryIds != null)
        {
            newEntity.CompanyIndustries = companyIndustries;
        }
        await _repo.SaveAsync();
    }
}

