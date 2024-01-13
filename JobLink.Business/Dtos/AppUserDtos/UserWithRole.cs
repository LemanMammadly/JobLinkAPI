namespace JobLink.Business.Dtos.AppUserDtos;

public record UserWithRole
{
    public UserDetailItemDto User { get; set; }
    public IEnumerable<string> Roles { get; set; }
}

