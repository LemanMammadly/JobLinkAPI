using JobLink.Business.Dtos.CategoryDtos;

namespace JobLink.Business.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryListItemDto>> GetAllAsync(bool takeAll);
    Task<CategoryDetailItemDto> GetByIdAsync(int id, bool takeAll);
    Task CreateAsync(CreateCategoryDto dto);
    Task UpdateAsync(int id,UpdateCategoryDto dto);
    Task DeleteAsync(int id);
    Task SoftDeleteAsync(int id);
    Task ReverteSoftDeleteAsync(int id);
}

