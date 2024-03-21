using JobLink.Business.Dtos.JobDescriptionDtos;

namespace JobLink.Business.Services.Interfaces;

public interface IJobDescriptionService
{
    Task CreateAsync(CreateJobDescriptionDto dto, int adverId);
    Task<IEnumerable<JobDescriptionListItemDto>> GetAllAsync();
    Task<JobDescriptionDetailDto> GetByIdAsync(int id);
    Task UpdateAsync(UpdateJobDescriptionDto dto);
    Task DeleteAsync(int id);
}

