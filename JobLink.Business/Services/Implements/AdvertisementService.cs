using System.Security.Claims;
using AutoMapper;
using JobLink.Business.Dtos.AdvertisementDtos;
using JobLink.Business.Exceptions.AppUserExceptions;
using JobLink.Business.Exceptions.Common;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using JobLink.Core.Enums;
using JobLink.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace JobLink.Business.Services.Implements;

public class AdvertisementService : IAdvertisementService
{
    readonly IAdvertisementRepository _repo;
    readonly ICategoryRepository _catrepo;
    readonly IMapper _mapper;
    readonly IHttpContextAccessor _httpContextAccessor;
    readonly string? userId;
    readonly UserManager<AppUser> _userManager;

    public AdvertisementService(IAdvertisementRepository repo, IMapper mapper, ICategoryRepository catrepo, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
    {
        _repo = repo;
        _mapper = mapper;
        _catrepo = catrepo;
        _httpContextAccessor = httpContextAccessor;
        userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _userManager = userManager;
    }

    public async Task CheckStatus()
    {
        var advertisements = _repo.GetAll();

        foreach (var advertisement in advertisements)
        {
            if(advertisement.EndDate<=DateTime.UtcNow.AddHours(4))
            {
                advertisement.Status = AdvertisementStatus.Deactive;
            }
        }
        await _repo.SaveAsync();
    }

    public async Task CreateAsync(CreateAdvertisementDto dto)
    {
        if (String.IsNullOrWhiteSpace(userId)) throw new ArgumentIsNullException();
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new AppUserNotFoundException();

        var advertisement = _mapper.Map<Advertisement>(dto);
        advertisement.Status = AdvertisementStatus.Active;

        var category = await _catrepo.GetByIdAsync(dto.CategoryId);
        if (category is null || category.IsDeleted) throw new NotFoundException<Category>();

        advertisement.EndDate = DateTime.Now.AddDays(31);

        await _repo.CreateAsync(advertisement);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<AdvertisementListItemDto>> GetAllAsync(bool takeAl)
    {
        if(takeAl)
        {
            var advertisements = _repo.GetAll();
            return _mapper.Map<IEnumerable<AdvertisementListItemDto>>(advertisements);
        }
        else
        {
            var advertisements = _repo.FindAll(a => a.IsDeleted == false);
            return _mapper.Map<IEnumerable<AdvertisementListItemDto>>(advertisements);
        }
    }
}

