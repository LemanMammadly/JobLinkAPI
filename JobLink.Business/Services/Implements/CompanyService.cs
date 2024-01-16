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
}

