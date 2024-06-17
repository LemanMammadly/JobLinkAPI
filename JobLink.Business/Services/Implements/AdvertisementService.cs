using System.Security.Claims;
using AutoMapper;
using JobLink.Business.Dtos.AdvertisementDtos;
using JobLink.Business.Dtos.JobDescriptionDtos;
using JobLink.Business.Dtos.ReqruimentDtos;
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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
    readonly IJobDescriptionService _jobDescriptionService;
    readonly IJobDescriptionRepository _jobRepo;
    readonly IReqruimentService _reqruimentService;
    readonly IReqruimentRepository _reqruimentRepository;
    readonly IConfiguration _config;

    public AdvertisementService(IAdvertisementRepository repo, IMapper mapper, ICategoryRepository catrepo, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IAbilityRepository abilityRepository, IJobDescriptionService jobDescriptionService, IJobDescriptionRepository jobRepo, IReqruimentService reqruimentService, IReqruimentRepository reqruimentRepository, IConfiguration config)
    {
        _repo = repo;
        _mapper = mapper;
        _catrepo = catrepo;
        _httpContextAccessor = httpContextAccessor;
        userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _userManager = userManager;
        _abilityRepository = abilityRepository;
        _jobDescriptionService = jobDescriptionService;
        _jobRepo = jobRepo;
        _reqruimentService = reqruimentService;
        _reqruimentRepository = reqruimentRepository;
        _config = config;
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

        if (dto.Desc != null)
        {
            foreach (var item in dto.Desc)
            {
                await _jobDescriptionService.CreateAsync(new CreateJobDescriptionDto { Description = item },advertisement.Id);
            }
            await _jobRepo.SaveAsync();
        }

        if(dto.Reqruiment != null)
        {
            foreach (var item in dto.Reqruiment)
            {
                await _reqruimentService.CreateAsync(new CreateReqruimentDto { Text = item }, advertisement.Id);
            }
            await _jobRepo.SaveAsync();
        }
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
        var advertisements = _repo.FindAll(a => a.IsDeleted == false && a.State == State.Accept, "AdvertisementAbilities", "AdvertisementAbilities.Ability", "JobDescriptions", "Reqruiments", "Company");
        var map = _mapper.Map<IEnumerable<AdvertisementListItemDto>>(advertisements);

        foreach (var adver in map)
        {
            adver.Company.Logo = _config["Jwt:Issuer"] + "wwwroot/" + adver.Company.Logo;
        }
        return map;
    }

    public async Task<IEnumerable<AdvertisementListItemDto>> GetAllAsync(bool takeAl)
    {
        if(takeAl)
        {
            var advertisements = _repo.GetAll("AdvertisementAbilities", "AdvertisementAbilities.Ability", "JobDescriptions", "Reqruiments", "Company");
            var map = _mapper.Map<IEnumerable<AdvertisementListItemDto>>(advertisements);
            foreach (var adver in map)
            {
                adver.Company.Logo = _config["Jwt:Issuer"] + "wwwroot/" + adver.Company.Logo;
            }
            return map;
        }
        else
        {
            var advertisements = _repo.FindAll(a => a.IsDeleted == false, "AdvertisementAbilities", "AdvertisementAbilities.Ability", "JobDescriptions", "Reqruiments", "Company");
            var map = _mapper.Map<IEnumerable<AdvertisementListItemDto>>(advertisements);

            foreach (var adver in map)
            {
                adver.Company.Logo = _config["Jwt:Issuer"] + "wwwroot/" + adver.Company.Logo;
            }
            return map;
        }
    }

    public async Task<IEnumerable<AdvertisementListItemDto>> SortByDate(IEnumerable<AdvertisementListItemDto> advertisements, DateFilter? filter)
    {
        var NowDate = DateTime.Now;
        DateTime? filterDate=null;
        IEnumerable<AdvertisementListItemDto> advertisementsList = new List<AdvertisementListItemDto>();

        if(filter != null)
        {
            filterDate = NowDate.AddDays(-(int)filter);
        }
        advertisementsList = advertisements.Where(a => a.CreateDate >= filterDate);
        return advertisementsList;
    }

    public async Task<AdvertisementDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        Advertisement? advertisement;

        if(takeAll)
        {
            advertisement = await _repo.GetByIdAsync(id,"AdvertisementAbilities", "AdvertisementAbilities.Ability", "JobDescriptions", "Reqruiments", "Company");
            if (advertisement is null) throw new NotFoundException<Advertisement>();
        }
        else
        {
            advertisement = await _repo.GetSingleAsync(a => a.IsDeleted == false && a.Id == id, "AdvertisementAbilities", "AdvertisementAbilities.Ability", "JobDescriptions", "Reqruiments", "Company");
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

    public async Task<IEnumerable<AdvertisementListItemDto>> SortBy(IEnumerable<AdvertisementListItemDto> advertisements, Sort? sort)
    {
        IEnumerable<AdvertisementListItemDto> advertisementsList = new List<AdvertisementListItemDto>();
        if(sort==Sort.Salary)
        {
            advertisementsList = advertisements.OrderByDescending(a => a.Salary);
        }
        if (sort == Sort.Company)
        {
            advertisementsList = advertisements.OrderBy(a => a.Company.Name);
        }
        if (sort == Sort.ViewCount)
        {
            advertisementsList = advertisements.OrderByDescending(a => a.ViewCount);
        }
        if (sort == Sort.Position)
        {
            advertisementsList = advertisements.OrderBy(a => a.Title);
        }

        return advertisementsList;
    }

    public async Task<IEnumerable<AdvertisementListItemDto>> SortByArea(IEnumerable<AdvertisementListItemDto> advertisements, string area)
    {
        IEnumerable<AdvertisementListItemDto> advertisementsList = new List<AdvertisementListItemDto>();
        advertisementsList = advertisements.Where(a => a.City == area);
        return advertisementsList;
    }

    public async Task<IEnumerable<AdvertisementListItemDto>> SortBySalary(IEnumerable<AdvertisementListItemDto> advertisements, Salary? salary)
    {
        IEnumerable<AdvertisementListItemDto> advertisementsList = new List<AdvertisementListItemDto>();

        if (salary == Salary.ZeroFiveHundred)
        {
            advertisementsList = advertisements.Where(a => a.Salary > 0 && a.Salary <= 500);
        }
        if (salary == Salary.FiveHundredOneThousand)
        {
            advertisementsList = advertisements.Where(a => a.Salary > 500 && a.Salary <= 1000);
        }
        if (salary == Salary.OneThousandTwoThousand)
        {
            advertisementsList = advertisements.Where(a => a.Salary > 1000 && a.Salary <= 2000);
        }
        if (salary == Salary.TwoThousandFiveThousand)
        {
            advertisementsList = advertisements.Where(a => a.Salary > 2000 && a.Salary <= 5000);
        }
        if (salary == Salary.FiveThousandUpper)
        {
            advertisementsList = advertisements.Where(a => a.Salary > 5000);
        }

        return advertisementsList;
    }

    public async Task UpdateAddJobDescription(int id,List<string> descs)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var advertisement = await _repo.GetByIdAsync(id, "JobDescriptions");
        if (advertisement is null) throw new NotFoundException<Advertisement>();

        if(descs!=null)
        {
            foreach (var desc in descs)
            {
                await _jobDescriptionService.CreateAsync(new CreateJobDescriptionDto { Description = desc }, advertisement.Id);
            }
            await _jobRepo.SaveAsync();
        }
    }

    public async Task UpdateAddReqruiment(int id, List<string> descs)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var advertisement = await _repo.GetByIdAsync(id, "JobDescriptions");
        if (advertisement is null) throw new NotFoundException<Advertisement>();

        if(descs != null)
        {
            foreach (var desc in descs)
            {
                await _reqruimentService.CreateAsync(new CreateReqruimentDto { Text = desc }, advertisement.Id);
            }
            await _reqruimentRepository.SaveAsync();
        }
    }

    public async Task UpdateAsync(int id, UpdateAdvertisementDto dto)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var advertisement = await _repo.GetSingleAsync(a => a.Id == id && a.IsDeleted == false, "AdvertisementAbilities", "AdvertisementAbilities.Ability", "JobDescriptions");
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

        await _repo.SaveAsync();
    }

    public async Task UpdateDeleteJobDescription(int id, List<int> ids)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var advertisement = await _repo.GetByIdAsync(id, "JobDescriptions");
        if (advertisement is null) throw new NotFoundException<Advertisement>();

        if(ids!=null)
        {
            foreach (var item in ids)
            {
                bool isExists = advertisement.JobDescriptions.Any(i => i.Id == item);
                if(isExists)
                {
                    var desc = await _jobRepo.GetByIdAsync(item);
                    if (desc is null) throw new NotFoundException<JobDescription>();

                    await _jobDescriptionService.DeleteAsync(item);
                }
                else
                {
                    throw new NotFoundException<JobDescription>();
                }
            }
            await _jobRepo.SaveAsync();
        }
    }

    public async Task UpdateDeleteReqruiments(int id, List<int> ids)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var advertisement = await _repo.GetByIdAsync(id, "Reqruiments");
        if (advertisement is null) throw new NotFoundException<Advertisement>();

        if(ids != null)
        {
            foreach (var iditem in ids)
            {
                bool isExist = advertisement.Reqruiments.Any(r => r.Id == iditem);
                if(isExist)
                {
                    var reqs = await _reqruimentRepository.GetByIdAsync(iditem);
                    if (reqs is null) throw new NotFoundException<Reqruiment>();

                    await _reqruimentService.DeleteAsync(iditem);
                }
                else
                {
                    throw new NotFoundException<Reqruiment>();
                }
            }
            await _reqruimentRepository.SaveAsync();
        }
    }

    public async Task UpdateJobDescription(int id, List<int> ids, List<string> descs)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var advertisement = await _repo.GetByIdAsync(id, "JobDescriptions");
        if (advertisement is null) throw new NotFoundException<Advertisement>();

        if (advertisement.JobDescriptions != null)
        {
            for (int i = 0; i < descs.Count; i++)
            {
                var description = await _jobRepo.GetByIdAsync(ids[i]);
                if (description is null) throw new NotFoundException<JobDescription>();

                if (description.AdvertisementId != id) throw new NotFoundException<Advertisement>();

                await _jobDescriptionService.UpdateAsync(ids[i], descs[i]);
            }
            await _jobRepo.SaveAsync();
        }
    }

    public async Task UpdateReqruiment(int id, List<int> ids, List<string> reqs)
    {
        if (id <= 0) throw new NegativeIdException<Reqruiment>();
        var advertisement = await _repo.GetByIdAsync(id, "Reqruiments");
        if (advertisement is null || advertisement.IsDeleted == true) throw new NotFoundException<Advertisement>();

        if(advertisement.Reqruiments != null)
        {
            for (int i = 0; i < reqs.Count; i++)
            {
                var reqruiment = await _reqruimentRepository.GetByIdAsync(ids[i]);
                if (reqruiment is null) throw new NotFoundException<Reqruiment>();

                if (reqruiment.AdvertisementId != id) throw new NotFoundException<Advertisement>();
                await _reqruimentService.UpdateAsync(ids[i], reqs[i]);
            }
            await _jobRepo.SaveAsync();
        }
    }

    public async Task UpdateStateAsync(int id, State state)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        var advertisement = await _repo.GetSingleAsync(a => a.Id == id && a.IsDeleted == false);
        if (advertisement is null) throw new NotFoundException<Advertisement>();

        advertisement.State = state;
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<AdvertisementListItemDto>> GetAllFilter(AdvertisementFilterDto filter)
    {
        var advertisements = _repo.FindAll(a => a.IsDeleted == false && a.State == State.Accept, "AdvertisementAbilities", "AdvertisementAbilities.Ability", "JobDescriptions", "Reqruiments", "Company");
        var map = _mapper.Map<IEnumerable<AdvertisementListItemDto>>(advertisements);

        var sortedAdvers = map;

        if(filter.Sort != null)
        {
            sortedAdvers = await SortBy(sortedAdvers, filter.Sort); 
        }
        if(filter.Area != null)
        {
            sortedAdvers = await SortByArea(sortedAdvers, filter.Area);
        }
        if (filter.Date != null)
        {
            sortedAdvers = await SortByDate(sortedAdvers, filter.Date);
        }
        if (filter.Salary != null)
        {
            sortedAdvers = await SortBySalary(sortedAdvers, filter.Salary);
        }
        foreach (var adver in sortedAdvers)
        {
            adver.Company.Logo = _config["Jwt:Issuer"] + "wwwroot/" + adver.Company.Logo;
        }

        return sortedAdvers;
    }
}

