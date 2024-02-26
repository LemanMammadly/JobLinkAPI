using AutoMapper;
using JobLink.Business.Dtos.AbilityDtos;
using JobLink.Business.Exceptions.Common;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using JobLink.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobLink.Business.Services.Implements;

public class AbilityService : IAbilityService
{
    readonly IAbilityRepository _repo;
    readonly IMapper _mapper;

    public AbilityService(IAbilityRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task CreateAsync(string name)
    {
        if (await _repo.IsExistAsync(a => a.Name == name)) throw new IsAlreadyExistException<Ability>();

        await _repo.CreateAsync(new Ability
        {
            Name = name
        });
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Ability>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Ability>();
        _repo.Delete(entity);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<AbilityListItemDto>> GetAllAsync()
    {
        var entities = await _repo.GetAll().ToListAsync();
        return _mapper.Map<IEnumerable<AbilityListItemDto>>(entities);
    }

    public async Task<AbilityDetailItemDto> GetByIdAsync(int id)
    {
        if (id <= 0) throw new NegativeIdException<Ability>();
        var entity = await _repo.GetByIdAsync(id);
        if (entity is null) throw new NotFoundException<Ability>();
        return _mapper.Map<AbilityDetailItemDto>(entity);
    }
}

