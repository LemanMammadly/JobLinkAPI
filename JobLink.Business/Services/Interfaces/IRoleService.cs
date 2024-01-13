using JobLink.Business.Dtos.RolesDtos;
using Microsoft.AspNetCore.Identity;

namespace JobLink.Business.Services.Interfaces;

public interface IRoleService
{
    Task CreateAsync(string name);
    Task<IEnumerable<IdentityRole>> GetAllAync();
    Task UpdateAsync(UpdateRolesDto dto);
    Task DeleteAsync(string name);
}

