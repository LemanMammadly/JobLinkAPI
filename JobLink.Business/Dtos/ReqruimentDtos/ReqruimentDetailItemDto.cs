namespace JobLink.Business.Dtos.ReqruimentDtos;

public record ReqruimentDetailItemDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int AdvertisementId { get; set; }
}

