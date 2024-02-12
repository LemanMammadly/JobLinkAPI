using System.Security.Claims;
using AutoMapper;
using JobLink.Business.Constants;
using JobLink.Business.Dtos.CompanyDtos;
using JobLink.Business.Dtos.IndustryDtos;
using JobLink.Business.Exceptions.AppUserExceptions;
using JobLink.Business.Exceptions.Common;
using JobLink.Business.Exceptions.FileExceptions;
using JobLink.Business.Exceptions.IndustryExceptions;
using JobLink.Business.Extensions;
using JobLink.Business.ExternalServices.Interfaces;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using JobLink.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobLink.Business.Services.Implements;

public class CompanyService : ICompanyService
{
    readonly ICompanyRepository _repo;
    readonly IIndustryRepository _industryRepository;
    readonly IMapper _mapper;
    readonly IFileService _fileService;
    readonly IHttpContextAccessor _httpContextAccessor;
    readonly string? _userId;
    readonly UserManager<AppUser> _userManager;

    public CompanyService(ICompanyRepository repo, IIndustryRepository industryRepository, IMapper mapper, IFileService fileService, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
    {
        _repo = repo;
        _industryRepository = industryRepository;
        _mapper = mapper;
        _fileService = fileService;
        _httpContextAccessor = httpContextAccessor;
        _userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _userManager = userManager;
    }

    public async Task CreateAsync(CreateCompanyDto dto)
    {
        if (String.IsNullOrWhiteSpace(_userId)) throw new ArgumentIsNullException();
        var user =await _userManager.Users.Include(u=>u.Company).SingleOrDefaultAsync(u=>u.Id==_userId);
        if (user is null) throw new AppUserNotFoundException();
        if (user.Company != null) throw new UserHaveCompanyAlreadyExistException();

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
        company.AppUserId = _userId;

        if (dto.LogoFile != null)
        {
            company.Logo = await _fileService.UploadAsync(dto.LogoFile, RootConstant.CompanyLogoRoot);
        }

        await _repo.CreateAsync(company);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (String.IsNullOrWhiteSpace(_userId)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.Include(u => u.Company).SingleOrDefaultAsync(u => u.Id == _userId);
        if (user is null) throw new AppUserNotFoundException();

        if (id <= 0) throw new NegativeIdException<Company>();
        var entity = await _repo.GetByIdAsync(id, "CompanyIndustries", "CompanyIndustries.Industry", "AppUser");
        if (entity is null) throw new NotFoundException<Company>();
        if (entity.AppUserId != _userId) throw new UserCompanyIsNotTheSameException();
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
            var entities = await _repo.GetAll("CompanyIndustries", "CompanyIndustries.Industry", "AppUser", "Advertisements").ToListAsync();
            return _mapper.Map<IEnumerable<CompanyListItemDto>>(entities);
        }
        else
        {
            var entities =await _repo.FindAll(c=>c.IsDeleted==false,"CompanyIndustries", "CompanyIndustries.Industry", "AppUser", "Advertisements").ToListAsync();
            return _mapper.Map<IEnumerable<CompanyListItemDto>>(entities);
        }
    }

    public async Task<CompanyDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Company>();
        Company? entity;

        if(takeAll)
        {
            entity = await _repo.GetByIdAsync(id, "CompanyIndustries", "CompanyIndustries.Industry", "AppUser", "Advertisements");
            if (entity is null) throw new NotFoundException<Company>();
        }
        else
        {
            entity = await _repo.GetSingleAsync(c => c.Id == id && c.IsDeleted == false, "CompanyIndustries", "CompanyIndustries.Industry", "AppUser", "Advertisements");
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
        if (String.IsNullOrWhiteSpace(_userId)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.Include(u => u.Company).SingleOrDefaultAsync(u => u.Id == _userId);
        if (user is null) throw new AppUserNotFoundException();

        if (id <= 0) throw new NegativeIdException<Company>();
        var entity = await _repo.GetByIdAsync(id, "CompanyIndustries", "CompanyIndustries.Industry", "AppUser");
        if (entity is null) throw new NotFoundException<Company>();
        if (entity.AppUserId != _userId) throw new UserCompanyIsNotTheSameException();

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

