using AutoMapper;
using JobLink.Business.Dtos.AppUserDtos;
using JobLink.Business.Dtos.TokenDtos;
using JobLink.Business.Exceptions.AppUserExceptions;
using JobLink.Business.ExternalServices.Interfaces;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobLink.Business.Services.Implements;

public class AppUserService : IAppUserService
{
    readonly UserManager<AppUser> _userManager;
    readonly ITokenService _tokenService;
    readonly IMapper _mapper;

    public AppUserService(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<TokenResponseDto> Login(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user is null) throw new LoginFailedException("Username or Password is wrong");
        var result = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!result) throw new LoginFailedException("Username or Password is wrong");
        return _tokenService.CreateAppUserToken(user);
    }

    public async Task Register(RegisterDto dto)
    {
        if (await _userManager.Users.AnyAsync(u => u.UserName == dto.Username)) throw new UsernameIsAlreadyExistException();
        if (await _userManager.Users.AnyAsync(u => u.Email == dto.Email)) throw new EmailIsAlreadyExistException();
        if (dto.ConfirmPassword != dto.Password) throw new ConfirmPasswordIsNotSameException();

        var user = _mapper.Map<AppUser>(dto);
        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            string sb = String.Empty;
            foreach (var item in result.Errors)
            {
                sb += item.Description + ' ';
            }
            throw new RegisterFailedException();
        }

    }
}

