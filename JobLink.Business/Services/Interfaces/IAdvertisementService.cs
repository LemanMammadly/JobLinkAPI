using JobLink.Business.Dtos.AdvertisementDtos;

namespace JobLink.Business.Services.Interfaces;

public interface IAdvertisementService
{
    Task<IEnumerable<AdvertisementListItemDto>> GetAllAsync(bool takeAl);
    Task<AdvertisementDetailItemDto> GetByIdAsync(int id, bool takeAll);
    Task CreateAsync(CreateAdvertisementDto dto);
    Task UpdateAsync(int id, UpdateAdvertisementDto dto);
    Task CheckStatus();
    Task ExpiredDeletion();
    Task SoftDeleteAsync(int id);
    Task ReverteSoftDeleteAsync(int id);
    Task DeleteAsync(int id);
}

