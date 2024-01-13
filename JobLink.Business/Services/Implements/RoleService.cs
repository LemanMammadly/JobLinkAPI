using JobLink.Business.Dtos.RolesDtos;
using JobLink.Business.Exceptions.Common;
using JobLink.Business.Exceptions.RoleExceptions;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobLink.Business.Services.Implements;

public class RoleService : IRoleService
{
    readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task CreateAsync(string name)
    {
        if (String.IsNullOrWhiteSpace(name)) throw new ArgumentIsNullException();
        var role = await _roleManager.FindByNameAsync(name);
        if (role != null) throw new RoleIsAlreadyExistException();

        IdentityRole identityRole = new IdentityRole();
        identityRole.Name = name;

        var result = await _roleManager.CreateAsync(identityRole);
        if (!result.Succeeded) throw new RoleCreatedFailedException();
    }

    public async Task DeleteAsync(string name)
    {
        if (String.IsNullOrWhiteSpace(name)) throw new ArgumentIsNullException();
        var role = await _roleManager.FindByNameAsync(name);
        if (role is null) throw new RoleIsNotFoundException();

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded) throw new RoleDeletedFailedException();
    }

    public async Task<IEnumerable<IdentityRole>> GetAllAync()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles;
    }

    public async Task UpdateAsync(UpdateRolesDto dto)
    {
        var role = await _roleManager.FindByNameAsync(dto.RoleName);
        if (role is null) throw new RoleIsNotFoundException();

        var newRole = await _roleManager.FindByNameAsync(dto.NewRoleName);
        if (newRole != null) throw new RoleIsAlreadyExistException();

        role.Name = dto.NewRoleName;
        var result = await _roleManager.UpdateAsync(role);

        if (!result.Succeeded) throw new RoleUpdateFailedException();
    }
}

