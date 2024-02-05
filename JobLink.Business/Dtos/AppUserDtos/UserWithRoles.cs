using JobLink.Business.Dtos.CompanyDtos;
using JobLink.Core.Entities;

namespace JobLink.Business.Dtos.AppUserDtos;

public class UserWithRoles
{
    public UserListItemDto User { get; set; }
    public IEnumerable<string> Roles { get; set; }
}




