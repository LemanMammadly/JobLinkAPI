using System.Security.Claims;
using AutoMapper;
using JobLink.Business.Dtos.AppUserDtos;
using JobLink.Business.Dtos.TokenDtos;
using JobLink.Business.Exceptions.AppUserExceptions;
using JobLink.Business.Exceptions.Common;
using JobLink.Business.ExternalServices.Interfaces;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobLink.Business.Services.Implements;

public class AppUserService : IAppUserService
{
    readonly UserManager<AppUser> _userManager;
    readonly ITokenService _tokenService;
    readonly IHttpContextAccessor _httpContextAccessor;
    readonly IMapper _mapper;
    readonly string? _userId;

    public AppUserService(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
        _userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task<TokenResponseDto> Login(LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user is null) throw new LoginFailedException("Username or Password is wrong");
        var result = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!result) throw new LoginFailedException("Username or Password is wrong");
        return _tokenService.CreateAppUserToken(user);
    }

    public async Task<TokenResponseDto> LoginWithRefreshTokenAsync(string refreshtoken)
    {
        if (string.IsNullOrWhiteSpace(refreshtoken)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.SingleOrDefaultAsync(s => s.RefreshToken == refreshtoken);
        if (user is null) throw new ArgumentIsNullException();
        if (user.RefreshTokenExpiresDate < DateTime.UtcNow.AddHours(4)) throw new RefreshTokenExpiresIsOldException();
        return _tokenService.CreateAppUserToken(user);
    }

    public async Task Register(RegisterDto dto)
    {
        if (await _userManager.Users.AnyAsync(u => u.UserName == dto.UserName)) throw new UsernameIsAlreadyExistException();
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

    public async Task UpdateAsync( UpdateDto dto)
    {
        if (String.IsNullOrWhiteSpace(_userId)) throw new ArgumentIsNullException();
        if (!await _userManager.Users.AnyAsync(u => u.Id == _userId)) throw new AppUserNotFoundException();

        var user =await _userManager.FindByIdAsync(_userId);

        if (await _userManager.Users.AnyAsync(a => a.Email == dto.Email && a.Id != _userId)) throw new AppUserEmailIsAlreadyExistException();
        if (await _userManager.Users.AnyAsync(a => a.UserName == dto.UserName && a.Id != _userId)) throw new AppUserUsernameIsAlreadyExistException();


        var newuser = _mapper.Map(dto, user);
        var result = await _userManager.UpdateAsync(newuser);
        if (!result.Succeeded)
        {
            string a = " ";
            foreach (var item in result.Errors)
            {
                a += item.Description + " ";
            }
            throw new AppUserUpdateFailedException(a);
        }
    }
}

