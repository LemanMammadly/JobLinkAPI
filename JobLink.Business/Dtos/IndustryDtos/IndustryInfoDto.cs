namespace JobLink.Business.Dtos.IndustryDtos;

public record IndustryInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public bool IsDeleted { get; set; }
}

