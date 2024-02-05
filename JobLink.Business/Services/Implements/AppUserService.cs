using System;
using System.Data;
using System.Security.Claims;
using AutoMapper;
using JobLink.Business.Dtos.AppUserDtos;
using JobLink.Business.Dtos.TokenDtos;
using JobLink.Business.Exceptions.AppUserExceptions;
using JobLink.Business.Exceptions.Common;
using JobLink.Business.Exceptions.RoleExceptions;
using JobLink.Business.ExternalServices.Interfaces;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

namespace JobLink.Business.Services.Implements;

public class AppUserService : IAppUserService
{
    readonly UserManager<AppUser> _userManager;
    readonly ITokenService _tokenService;
    readonly IHttpContextAccessor _httpContextAccessor;
    readonly string? _userId;
    readonly IMapper _mapper;
    readonly IEmailSenderService _emailService;
    readonly SignInManager<AppUser> _signInManager;
    readonly RoleManager<IdentityRole> _roleManager;

    public AppUserService(UserManager<AppUser> userManager, IMapper mapper, ITokenService tokenService, IHttpContextAccessor httpContextAccessor, IEmailSenderService emailService, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _mapper = mapper;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
        _userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _emailService = emailService;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task AddRoleAsync(AddRoleDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user is null) throw new AppUserNotFoundException();

        var role = await _roleManager.FindByNameAsync(dto.RoleName);
        if (role is null) throw new RoleIsNotFoundException();

        var result = await _userManager.AddToRoleAsync(user, role.Name);
        if (!result.Succeeded) throw new AppUserAddToRoleFailedException();
    }

    public async Task ChangePassword(ChangePasswordDto dto)
    {
        if (String.IsNullOrWhiteSpace(_userId)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == _userId);
        if (user is null) throw new ArgumentIsNullException();



        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);

        if (!result.Succeeded)
        {
            string sb = String.Empty;
            foreach (var item in result.Errors)
            {
                sb += item.Description + ' ';
            }
            throw new PasswordChangeFailedException();
        }
    }

    public async Task DeleteAsync(string id)
    {
        if (String.IsNullOrWhiteSpace(id)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user is null) throw new AppUserNotFoundException();

        var result = await _userManager.DeleteAsync(user);
        if(!result.Succeeded)
        {
            string sb = String.Empty;
            foreach (var item in result.Errors)
            {
                sb += item.Description + ' ';
            }
            throw new AppUserDeleteFailedException();
        }
    }

    public async Task<ICollection<UserWithRoles>> GetAllAsync(bool takeAll)
    {
        ICollection<UserWithRoles> users = new List<UserWithRoles>();

        if(takeAll)
        {
            foreach (var user in await _userManager.Users.Include(u=>u.Company).ToListAsync())
            {
                users.Add(new UserWithRoles {
                    User = _mapper.Map<UserListItemDto>(user),
                    Roles = await _userManager.GetRolesAsync(user),
                });
            }
        }
        else
        {
            foreach (var user in await _userManager.Users.Include(u => u.Company).Where(u=>u.IsDeleted==false).ToListAsync())
            {
                users.Add(new UserWithRoles
                {
                    User = _mapper.Map<UserListItemDto>(user),
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }
        }
        return users;
    }

    public async Task<UserWithRole> GetByIdAsync(string id, bool takeAll)
    {
        if (String.IsNullOrWhiteSpace(id)) throw new ArgumentIsNullException();
        UserWithRole user = new UserWithRole();
        if(takeAll)
        {
            var user1 = await _userManager.Users.Include(u => u.Company).SingleOrDefaultAsync(u => u.Id == id);
            if (user1 is null) throw new AppUserNotFoundException();
            user = new UserWithRole
            {
                User = _mapper.Map<UserDetailItemDto>(user1),
                Roles = await _userManager.GetRolesAsync(user1)
            };
        }
        else
        {
            var user1 = await _userManager.Users.Include(u => u.Company).SingleOrDefaultAsync(u => u.Id == id && u.IsDeleted==false);
            if (user1 is null) throw new AppUserNotFoundException();
            user = new UserWithRole
            {
                User = _mapper.Map<UserDetailItemDto>(user1),
                Roles = await _userManager.GetRolesAsync(user1)
            };
        }
        return user;
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

    public async Task Logout()
    {
        if (String.IsNullOrWhiteSpace(_userId)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == _userId);
        if (user is null) throw new AppUserNotFoundException();
        await _signInManager.SignOutAsync();
        user.RefreshToken = null;
        user.RefreshTokenExpiresDate = null;
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            string sb = String.Empty;
            foreach (var item in result.Errors)
            {
                sb += item.Description + ' ';
            }
            throw new LogoutFailedException("Logout ");
        }
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

    public async Task RemoveRoleAsync(RemoveRoleDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId);
        if (user is null) throw new AppUserNotFoundException();

        var userRole = await _userManager.GetRolesAsync(user);

        IdentityRole identityRole = new IdentityRole()
        {
            Name = userRole[0]
        };
        var role = await _roleManager.FindByNameAsync(dto.RoleName);
        if (role is null) throw new RoleIsNotFoundException();

        if (role.Name != identityRole.Name) throw new ThisRoleIsNotOwnThisUserException();

        var result = await _userManager.RemoveFromRoleAsync(user,role.Name);
        if (!result.Succeeded) throw new AppUserRemoveRoleException();
    }

    public async Task ReverteDeleteAsync(string id)
    {
        if (String.IsNullOrWhiteSpace(id)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user is null) throw new AppUserNotFoundException();
        user.IsDeleted = false;
        await _userManager.UpdateAsync(user);
    }

    public async Task SoftDeleteAsync(string id)
    {
        if (String.IsNullOrWhiteSpace(id)) throw new ArgumentIsNullException();
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id);
        if (user is null) throw new AppUserNotFoundException();
        user.IsDeleted = true;
        await _userManager.UpdateAsync(user);
    }

    public async Task UpdateAsync(UpdateDto dto)
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

