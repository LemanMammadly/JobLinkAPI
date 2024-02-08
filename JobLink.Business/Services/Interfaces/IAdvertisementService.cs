using JobLink.Business.Dtos.AdvertisementDtos;

namespace JobLink.Business.Services.Interfaces;

public interface IAdvertisementService
{
    Task<IEnumerable<AdvertisementListItemDto>> GetAllAsync(bool takeAl);
    Task CreateAsync(CreateAdvertisementDto dto);
    Task CheckStatus();
}

