using JobLink.Core.Enums;

namespace JobLink.Business.Dtos.AdvertisementDtos;

public record AdvertisementFilterDto
{
    public DateFilter? Date { get; set; }
    public Sort? Sort { get; set; }
    public Salary? Salary { get; set; }
    public string? Area { get; set; }
}

