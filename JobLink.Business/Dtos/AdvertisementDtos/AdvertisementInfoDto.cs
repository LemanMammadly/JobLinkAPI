using JobLink.Core.Enums;

namespace JobLink.Business.Dtos.AdvertisementDtos;

public record AdvertisementInfoDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string City { get; set; }
    public decimal? Salary { get; set; }
    public string WorkGraphic { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? Ability { get; set; }
    public string JobDesc { get; set; }
    public string Reqruiment { get; set; }
    public string? Experience { get; set; }
    public string? Education { get; set; }
    public int ViewCount { get; set; }
    public string Status { get; set; }
    public string State { get; set; }
    public bool IsDeleted { get; set; }
}

