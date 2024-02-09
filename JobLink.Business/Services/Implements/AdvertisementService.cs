﻿using System.Security.Claims;
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
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new AppUserNotFoundException();

        var advertisement = _mapper.Map<Advertisement>(dto);
        advertisement.Status = AdvertisementStatus.Active;

        var category = await _catrepo.GetByIdAsync(dto.CategoryId);
        if (category is null || category.IsDeleted) throw new NotFoundException<Category>();

        advertisement.EndDate = DateTime.Now.AddDays(31).AddHours(4);

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

    public async Task<AdvertisementDetailItemDto> GetByIdAsync(int id, bool takeAll)
    {
        if (id <= 0) throw new NegativeIdException<Advertisement>();
        Advertisement? advertisement;

        if(takeAll)
        {
            advertisement = await _repo.GetByIdAsync(id);
            if (advertisement is null) throw new NotFoundException<Advertisement>();
        }
        else
        {
            advertisement = await _repo.GetSingleAsync(a => a.IsDeleted == false && a.Id == id);
            if (advertisement is null) throw new NotFoundException<Advertisement>();
            advertisement.ViewCount++;
        }

        await _repo.SaveAsync();
        return _mapper.Map<AdvertisementDetailItemDto>(advertisement);
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
        var advertisement = await _repo.GetSingleAsync(a => a.Id == id && a.IsDeleted == false);
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

        _mapper.Map(dto, advertisement);
        await _repo.SaveAsync();
    }
}

