using JobLink.Business.Dtos.AppUserDtos;
using JobLink.Business.Dtos.TokenDtos;

namespace JobLink.Business.Services.Interfaces;

public interface IAppUserService
{
    Task Register(RegisterDto dto);
    Task<TokenResponseDto> Login(LoginDto dto);
    Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshtoken);
    Task UpdateAsync(UpdateDto dto);
}

