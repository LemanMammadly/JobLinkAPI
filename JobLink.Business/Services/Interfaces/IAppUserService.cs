using JobLink.Business.Dtos.AppUserDtos;
using JobLink.Business.Dtos.TokenDtos;

namespace JobLink.Business.Services.Interfaces;

public interface IAppUserService
{
    Task Register(RegisterDto dto);
    Task<TokenResponseDto> Login(LoginDto dto);
    Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshtoken);
    Task UpdateAsync(UpdateDto dto);
    Task ChangePassword(ChangePasswordDto dto);
    Task<ICollection<UserWithRoles>> GetAllAsync(bool takeAll);
    Task<UserWithRole> GetByIdAsync(string id, bool takeAll);
    Task AddRoleAsync(AddRoleDto dto);
    Task SoftDeleteAsync(string id);
    Task ReverteDeleteAsync(string id);
    Task DeleteAsync(string id);
    Task Logout();
}

