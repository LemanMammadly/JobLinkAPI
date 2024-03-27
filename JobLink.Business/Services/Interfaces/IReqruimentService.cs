using JobLink.Business.Dtos.ReqruimentDtos;

namespace JobLink.Business.Services.Interfaces;

public interface IReqruimentService
{
    Task<IEnumerable<ReqruimentListItemDto>> GetAllAsync();
    Task<ReqruimentDetailItemDto> GetByIdAsync(int id);
    Task CreateAsync(CreateReqruimentDto dto,int adverId);
    Task UpdateAsync(int id,string text);
    Task DeleteAsync(int id);
}

