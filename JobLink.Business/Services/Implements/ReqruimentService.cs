using AutoMapper;
using JobLink.Business.Dtos.ReqruimentDtos;
using JobLink.Business.Exceptions.Common;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using JobLink.DAL.Repositories.Interfaces;

namespace JobLink.Business.Services.Implements;

public class ReqruimentService : IReqruimentService
{
    readonly IReqruimentRepository _repo;
    readonly IAdvertisementRepository _adver;
    readonly IMapper _mapper;

    public ReqruimentService(IReqruimentRepository repo, IMapper mapper, IAdvertisementRepository adver)
    {
        _repo = repo;
        _mapper = mapper;
        _adver = adver;
    }

    public async Task CreateAsync(CreateReqruimentDto dto,int adverId)
    {
        if (adverId <= 0) throw new NegativeIdException<Advertisement>();
        var adver = await _adver.GetByIdAsync(adverId);
        if (adver is null) throw new NotFoundException<Advertisement>();

        var reqruiment = _mapper.Map<Reqruiment>(dto);
        reqruiment.AdvertisementId = adverId;
        await _repo.CreateAsync(reqruiment);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Reqruiment>();
        var reqruiment = await _repo.GetByIdAsync(id);
        if (reqruiment is null) throw new NotFoundException<Reqruiment>();

        await _repo.DeleteAsync(id);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<ReqruimentListItemDto>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<ReqruimentListItemDto>>(_repo.GetAll());
    }

    public async Task<ReqruimentDetailItemDto> GetByIdAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Reqruiment>();
        var reqruiment = await _repo.GetByIdAsync(id);
        if (reqruiment is null) throw new NotFoundException<Reqruiment>();

        return _mapper.Map<ReqruimentDetailItemDto>(reqruiment);
    }

    public async Task UpdateAsync(int id, string text)
    {
        if (id <= 0) throw new NegativeIdException<Reqruiment>();
        var reqruiment = await _repo.GetByIdAsync(id);
        if (reqruiment is null) throw new NotFoundException<Reqruiment>();
        reqruiment.Text = text;
        await _repo.SaveAsync();
    }
}

