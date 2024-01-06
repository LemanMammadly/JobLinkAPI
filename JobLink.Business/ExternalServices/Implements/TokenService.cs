using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobLink.Business.Dtos.TokenDtos;
using JobLink.Business.ExternalServices.Interfaces;
using JobLink.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JobLink.Business.ExternalServices.Implements;

public class TokenService : ITokenService
{
    readonly IConfiguration _configuration;
    readonly UserManager<AppUser> _userManager;

    public TokenService(IConfiguration configuration, UserManager<AppUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public string CreateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }

    public TokenResponseDto CreateAppUserToken(AppUser appUser, int expires = 60)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name,appUser.UserName),
            new Claim(ClaimTypes.NameIdentifier,appUser.Id),
            new Claim(ClaimTypes.Email,appUser.Email),
            new Claim(ClaimTypes.GivenName,appUser.Name),
            new Claim(ClaimTypes.Surname,appUser.Surname)
        };

        foreach (var userRole in _userManager.GetRolesAsync(appUser).Result)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

        SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurity = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            DateTime.UtcNow.AddHours(4),
            DateTime.UtcNow.AddHours(4).AddMinutes(expires),
            credentials);

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        string token = tokenHandler.WriteToken(jwtSecurity);

        string refreshtoken = CreateRefreshToken();
        var refreshtokenExpires = jwtSecurity.ValidTo.AddMinutes(expires / 3);
        appUser.RefreshToken = refreshtoken;
        appUser.RefreshTokenExpiresDate = refreshtokenExpires;
        _userManager.UpdateAsync(appUser).Wait();
        return new()
        {
            Token = token,
            Username = appUser.UserName,
            Expires = jwtSecurity.ValidTo,
            RefreshToken = refreshtoken,
            RefreshTokenExpires = refreshtokenExpires,
            Roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList()
        };
    }
}