using JobLink.Business.Dtos.TokenDtos;
using JobLink.Core.Entities;

namespace JobLink.Business.ExternalServices.Interfaces;

public interface ITokenService
{
    TokenResponseDto CreateAppUserToken(AppUser appUser, int expires = 60);
    string CreateRefreshToken();
}

