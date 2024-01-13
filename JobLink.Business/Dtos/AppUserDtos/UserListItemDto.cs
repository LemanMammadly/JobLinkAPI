namespace JobLink.Business.Dtos.AppUserDtos;

public record UserListItemDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public bool IsDeleted { get; set; }
}



