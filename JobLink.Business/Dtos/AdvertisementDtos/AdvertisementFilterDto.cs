using JobLink.Core.Enums;

namespace JobLink.Business.Dtos.AdvertisementDtos;

public record AdvertisementFilterDto
{
    public DateFilter? Date { get; set; }
}

