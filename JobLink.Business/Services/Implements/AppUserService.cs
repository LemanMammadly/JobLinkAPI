using AutoMapper;
using JobLink.Business.Dtos.AppUserDtos;
using JobLink.Business.Exceptions.AppUserExceptions;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobLink.Business.Services.Implements;

public class AppUserService : IAppUserService
{
    readonly UserManager<AppUser> _userManager;
    readonly IMapper _mapper;

    public AppUserService(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
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

