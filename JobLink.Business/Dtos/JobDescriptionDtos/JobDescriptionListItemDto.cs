namespace JobLink.Business.Dtos.JobDescriptionDtos;

public record JobDescriptionListItemDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public int AdvertisementId { get; set; }
}

