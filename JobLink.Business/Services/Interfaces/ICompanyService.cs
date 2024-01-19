using JobLink.Business.Dtos.CompanyDtos;

namespace JobLink.Business.Services.Interfaces;

public interface ICompanyService
{
    Task CreateAsync(CreateCompanyDto dto);
    Task<IEnumerable<CompanyListItemDto>> GetAllAsync(bool takeAll);
    Task<CompanyDetailItemDto> GetByIdAsync(int id,bool takeAll);
    Task UpdateAsync(int id, CompanyUpdateDto dto);
    Task DeleteAsync(int id);
    Task SoftDeleteAsync(int id);
    Task ReverteSoftDeleteAsync(int id);
}

