using JobLink.Business.Dtos.AdvertisementDtos;
using JobLink.Core.Enums;

namespace JobLink.Business.Services.Interfaces;

public interface IAdvertisementService
{
    Task<IEnumerable<AdvertisementListItemDto>> GetAllAsync(bool takeAl);
    Task<IEnumerable<AdvertisementListItemDto>> GetAllAcceptAsync();
    Task<AdvertisementDetailItemDto> GetByIdAsync(int id, bool takeAll);
    Task CreateAsync(CreateAdvertisementDto dto);
    Task UpdateAsync(int id, UpdateAdvertisementDto dto);
    Task UpdateStateAsync(int id, State state);
    Task AcceptAdvertisement(int id);
    Task RejectAdvertisement(int id);
    Task CheckStatus();
    Task ExpiredDeletion();
    Task SoftDeleteAsync(int id);
    Task ReverteSoftDeleteAsync(int id);
    Task DeleteAsync(int id);
}


