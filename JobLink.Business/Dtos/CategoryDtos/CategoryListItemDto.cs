namespace JobLink.Business.Dtos.CategoryDtos;

public record CategoryListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public bool IsDeleted { get; set; }
}

