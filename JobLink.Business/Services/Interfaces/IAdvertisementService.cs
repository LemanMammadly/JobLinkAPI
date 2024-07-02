using JobLink.Business.Dtos.AdvertisementDtos;
using JobLink.Business.Dtos.JobDescriptionDtos;
using JobLink.Core.Entities;
using JobLink.Core.Enums;

namespace JobLink.Business.Services.Interfaces;

public interface IAdvertisementService
{
    Task<IEnumerable<AdvertisementListItemDto>> GetAllAsync(bool takeAl);
    Task<IEnumerable<AdvertisementListItemDto>> GetAllAcceptAsync();
    Task<AdvertisementDetailItemDto> GetByIdAsync(int id, bool takeAll);
    Task CreateAsync(CreateAdvertisementDto dto);
    Task UpdateAsync(int id, UpdateAdvertisementDto dto);
    Task UpdateJobDescription(int id,List<int> ids,List<string> descs);
    Task UpdateReqruiment(int id,List<int> ids,List<string> reqs);
    Task UpdateAddJobDescription(int id,List<string> descs);
    Task UpdateAddReqruiment(int id,List<string> descs);
    Task UpdateDeleteJobDescription(int id, List<int> ids);
    Task UpdateDeleteReqruiments(int id, List<int> ids);
    Task UpdateStateAsync(int id, State state);
    Task AcceptAdvertisement(int id);
    Task RejectAdvertisement(int id);
    Task CheckStatus();
    Task ExpiredDeletion();
    Task SoftDeleteAsync(int id);
    Task ReverteSoftDeleteAsync(int id);
    Task DeleteAsync(int id);
    Task<CountAdverDto> AdvertisementCount();
    Task<IEnumerable<AdvertisementListItemDto>> SortBy(IEnumerable<AdvertisementListItemDto> advertisements,Sort? sort);
    Task<IEnumerable<AdvertisementListItemDto>> SortBySalary(IEnumerable<AdvertisementListItemDto> advertisements,Salary? salary);
    Task<IEnumerable<AdvertisementListItemDto>> SortByArea(IEnumerable<AdvertisementListItemDto> advertisements, string area);
    Task<IEnumerable<AdvertisementListItemDto>> SortByDate(IEnumerable<AdvertisementListItemDto> advertisements, DateFilter? filter);
    Task<IEnumerable<AdvertisementListItemDto>> GetAllFilter(AdvertisementFilterDto filter);
}


