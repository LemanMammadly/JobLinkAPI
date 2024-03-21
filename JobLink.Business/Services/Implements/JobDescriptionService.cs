using AutoMapper;
using JobLink.Business.Dtos.JobDescriptionDtos;
using JobLink.Business.Exceptions.Common;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using JobLink.DAL.Repositories.Interfaces;

namespace JobLink.Business.Services.Implements;

public class JobDescriptionService : IJobDescriptionService
{
    readonly IJobDescriptionRepository _repo;
    readonly IMapper _mapper;
    readonly IAdvertisementRepository _adver;

    public JobDescriptionService(IJobDescriptionRepository repo, IMapper mapper,
        IAdvertisementRepository adver)
    {
        _repo = repo;
        _mapper = mapper;
        _adver = adver;
    }

    public async Task CreateAsync(CreateJobDescriptionDto dto,int adverId)
    {
        if (adverId < 0) throw new NegativeIdException<Advertisement>();
        var adver = await _adver.GetByIdAsync(adverId);
        if (adver == null) throw new NotFoundException<Advertisement>();

        var newAdver = _mapper.Map<JobDescription>(dto);
        newAdver.AdvertisementId = adverId;
        await _repo.CreateAsync(newAdver);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var jobdesc = await _repo.GetByIdAsync(id);
        if (jobdesc is null || jobdesc.IsDeleted == true) throw new NotFoundException<JobDescription>();
        _repo.Delete(jobdesc);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<JobDescriptionListItemDto>> GetAllAsync()
    {
        var desc = _repo.GetAll();
        return _mapper.Map<IEnumerable<JobDescriptionListItemDto>>(desc);
    }

    public async Task<JobDescriptionDetailDto> GetByIdAsync(int id)
    {
        if (id < 0) throw new NegativeIdException<Advertisement>();
        JobDescription? entity;
        entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<JobDescription>();
        return _mapper.Map<JobDescriptionDetailDto>(entity);
    }

    public async Task UpdateAsync(UpdateJobDescriptionDto dto)
    {
        if (dto.Id < 0) throw new NegativeIdException<Advertisement>();
        var desc = await _repo.GetByIdAsync(dto.Id);
        if (desc is null || desc.IsDeleted == true) throw new NotFoundException<JobDescription>();
        _mapper.Map(dto, desc);
        await _repo.SaveAsync();
    }
}

