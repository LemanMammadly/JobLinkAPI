using JobLink.Business.Dtos.AppUserDtos;

namespace JobLink.Business.Services.Interfaces;

public interface IAppUserService
{
    Task Register(RegisterDto dto);
}

