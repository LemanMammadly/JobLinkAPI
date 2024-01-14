using JobLink.Business.Dtos.IndustryDtos;

namespace JobLink.Business.Services.Interfaces;

public interface IIndustryService
{
    Task<IEnumerable<IndustryListItemDto>> GetAllAsync(bool takeAll);
    Task<IndustryDetailItemDto> GetByIdAsync(int id,bool takeAll);
    Task CreateAsync(CreateIndustryDto dto);
    Task UpdateAsync(int id,UpdateIndustryDto dto);
    Task DeleteAsync(int id);
    Task SoftDeleteAsync(int id);
    Task ReverteSoftDeleteAsync(int id);
}


