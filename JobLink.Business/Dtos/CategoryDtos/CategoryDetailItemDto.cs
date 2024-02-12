using JobLink.Business.Dtos.AdvertisementDtos;

namespace JobLink.Business.Dtos.CategoryDtos;

public record CategoryDetailItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public bool IsDeleted { get; set; }
    public IEnumerable<AdvertisementInfoDto> Advertisements { get; set; }
}

