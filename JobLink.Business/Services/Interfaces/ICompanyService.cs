using JobLink.Business.Dtos.CompanyDtos;

namespace JobLink.Business.Services.Interfaces;

public interface ICompanyService
{
    Task CreateAsync(CreateCompanyDto dto);
    Task<IEnumerable<CompanyListItemDto>> GetAllAsync(bool takeAll);
}

