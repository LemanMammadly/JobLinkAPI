using System.Security.Claims;
using AutoMapper;
using JobLink.Business.Dtos.AdvertisementDtos;
using JobLink.Business.Exceptions.AdvertisementsException;
using JobLink.Business.Exceptions.AppUserExceptions;
using JobLink.Business.Exceptions.Common;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using JobLink.Core.Enums;
using JobLink.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobLink.Business.Services.Implements;

public class AdvertisementService : IAdvertisementService
{
    readonly IAdvertisementRepository _repo;
    readonly ICategoryRepository _catrepo;
    readonly IMapper _mapper;
    readonly IHttpContextAccessor _httpContextAccessor;
    readonly string? userId;
    readonly UserManager<AppUser> _userManager;
    readonly IAbilityRepository _abilityRepository;

    public AdvertisementService(IAdvertisementRepository repo, IMapper mapper, ICategoryRepository catrepo, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IAbilityRepository abilityRepository)
    {
        _repo = repo;
        _mapper = mapper;
        _catrepo = catrepo;
        _httpContextAccessor = httpContextAccessor;
        userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _userManager = userManager;
        _abilityRepository = abilityRepository;
    }

    public async Task AcceptAdvertisement(int id)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var advertisement = await _repo.GetSingleAsync(a=>a.Id==id && a.IsDeleted==false && a.State==State.Pending);
        if (advertisement is null || advertisement.IsDeleted == true) throw new NotFoundException<Advertisement>();

        advertisement.State = State.Accept;
        await _repo.SaveAsync();
    }

    public async Task CheckStatus()
    {
        var advertisements = _repo.FindAll(a=>a.IsDeleted==false);

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
        var user = await _userManager.Users.Include(a => a.Company).FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null) throw new AppUserNotFoundException();

        var advertisement = _mapper.Map<Advertisement>(dto);
        advertisement.Status = AdvertisementStatus.Active;
        advertisement.State = State.Pending;

        var category = await _catrepo.GetByIdAsync(dto.CategoryId);
        if (category is null || category.IsDeleted) throw new NotFoundException<Category>();


        List<AdvertisementAbilities> advertisementAbilities = new List<AdvertisementAbilities>();
        if(dto.AbilityIds != null)
        {
            foreach (var id in dto.AbilityIds)
            {
                var ability = await _abilityRepository.GetByIdAsync(id);
                if (ability is null || ability.IsDeleted==true) throw new NotFoundException<Ability>();
                advertisementAbilities.Add(new AdvertisementAbilities { Ability = ability });
            }
        }

        advertisement.EndDate = DateTime.Now.AddDays(31).AddHours(4);
        advertisement.AdvertisementAbilities = advertisementAbilities;

        if(user.Company != null)
        {
            advertisement.CompanyId = user.Company.Id;  
        }
        else
        {
            throw new UserHasNotCompanyException();
        }

        await _repo.CreateAsync(advertisement);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Advertisement>();

        _repo.Delete(entity);
        await _repo.SaveAsync();
    }

    public async Task ExpiredDeletion()
    {
        var advertisements = _repo.FindAll(a => a.IsDeleted == false && a.Status == AdvertisementStatus.Deactive);

        foreach (var advertisement in advertisements)
        {
            if (advertisement.EndDate.AddDays(7) <= DateTime.UtcNow.AddHours(4))
            {
                _repo.SoftDelete(advertisement);
            }
        }
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<AdvertisementListItemDto>> GetAllAcceptAsync()
    {
        var advertisements = _repo.FindAll(a => a.IsDeleted == false && a.State == State.Accept, "AdvertisementAbilities", "AdvertisementAbilities.Ability");
        return _mapper.Map<IEnumerable<AdvertisementListItemDto>>(advertisements);
    }

    public async Task<IEnumerable<AdvertisementListItemDto>> GetAllAsync(bool takeAl)
    {
        if(takeAl)
        {
            var advertisements = _repo.GetAll("AdvertisementAbilities", "AdvertisementAbilities.Ability");
            return _mapper.Map<IEnumerable<AdvertisementListItemDto>>(advertisements);
        }
        else
        {
            var advertisements = _repo.FindAll(a => a.IsDeleted == false, "AdvertisementAbilities", "AdvertisementAbilities.Ability");
            return _mapper.Map<IEnumerable<AdvertisementListItemDto>>(advertisements);
        }
    }

    public async Task<AdvertisementDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        Advertisement? advertisement;

        if(takeAll)
        {
            advertisement = await _repo.GetByIdAsync(id,"AdvertisementAbilities", "AdvertisementAbilities.Ability");
            if (advertisement is null) throw new NotFoundException<Advertisement>();
        }
        else
        {
            advertisement = await _repo.GetSingleAsync(a => a.IsDeleted == false && a.Id == id, "AdvertisementAbilities", "AdvertisementAbilities.Ability");
            if (advertisement is null) throw new NotFoundException<Advertisement>();
            advertisement.ViewCount++;
        }

        await _repo.SaveAsync();
        return _mapper.Map<AdvertisementDetailItemDto>(advertisement);
    }

    public async Task RejectAdvertisement(int id)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var advertisement = await _repo.GetSingleAsync(a=>a.Id==id && a.IsDeleted == false && a.State==State.Pending);
        if (advertisement is null || advertisement.IsDeleted == true) throw new NotFoundException<Advertisement>();

        advertisement.State = State.Reject;
        await _repo.SaveAsync();
    }

    public async Task ReverteSoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Advertisement>();
        entity.CreateDate = DateTime.UtcNow.AddHours(4);
        entity.EndDate = DateTime.UtcNow.AddDays(31).AddHours(4);

        _repo.ReverteSoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Advertisement>();
        entity.CreateDate = DateTime.MinValue;
        entity.EndDate = DateTime.MinValue;

        _repo.SoftDelete(entity);
        await _repo.SaveAsync();
    }

    public async Task UpdateAsync(int id, UpdateAdvertisementDto dto)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var advertisement = await _repo.GetSingleAsync(a => a.Id == id && a.IsDeleted == false, "AdvertisementAbilities", "AdvertisementAbilities.Ability");
        if (advertisement is null) throw new NotFoundException<Advertisement>();

        if(dto.CategoryId != null)
        {
            var category = await _catrepo.GetSingleAsync(c=>c.Id==dto.CategoryId);
            if (category is null || category.IsDeleted) throw new NotFoundException<Category>();
        }
        else
        {
            throw new AdvertisementCategoryCouldNotBeNullException();
        }

        List<AdvertisementAbilities> advertisementAbilities = new List<AdvertisementAbilities>();
        if (dto.AbilityIds != null)
        {
            foreach (var iditem in dto.AbilityIds)
            {
                var ability = await _abilityRepository.GetByIdAsync(iditem);
                if (ability is null || ability.IsDeleted == true) throw new NotFoundException<Ability>();
                advertisementAbilities.Add(new AdvertisementAbilities { Ability = ability });
            }
        }

        var newEntity = _mapper.Map(dto, advertisement);
        newEntity.AdvertisementAbilities = advertisementAbilities;

        await _repo.SaveAsync();
    }

    public async Task UpdateStateAsync(int id, State state)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var advertisement = await _repo.GetSingleAsync(a => a.Id == id && a.IsDeleted == false);
        if (advertisement is null) throw new NotFoundException<Advertisement>();

        advertisement.State = state;
        await _repo.SaveAsync();
    }
}

