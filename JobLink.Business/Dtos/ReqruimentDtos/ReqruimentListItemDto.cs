namespace JobLink.Business.Dtos.ReqruimentDtos;

public record ReqruimentListItemDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int AdvertisementId { get; set; }
}

