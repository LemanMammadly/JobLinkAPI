using JobLink.Business.Dtos.AbilityDtos;
using JobLink.Core.Entities;

namespace JobLink.Business.Services.Interfaces;

public interface IAbilityService
{
    Task<IEnumerable<AbilityListItemDto>> GetAllAsync();
    Task<AbilityDetailItemDto> GetByIdAsync(int id);
    Task CreateAsync(string name);
    Task DeleteAsync(int id);

}

