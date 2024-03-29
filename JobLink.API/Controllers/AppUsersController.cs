﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JobLink.Business.Dtos.AppUserDtos;
using JobLink.Business.Exceptions.AppUserExceptions;
using JobLink.Business.ExternalServices.Interfaces;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using JobLink.DAL.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobLink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        readonly IAppUserService _service;
        readonly UserManager<AppUser> _userManager;
        readonly IEmailSenderService _emailService;
        readonly AppDbContext _context;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly string? _userId;

        public AppUsersController(IAppUserService service, UserManager<AppUser> userManager, IEmailSenderService emailService, AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _userManager = userManager;
            _emailService = emailService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm] RegisterDto dto)
        {
            await _service.Register(dto);

            var user = await _userManager.FindByEmailAsync(dto.Email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "AppUsers", new { token, email = dto.Email }, Request.Scheme);

            EmailToken emailToken = new EmailToken();
            emailToken.AppUserId = user.Id;
            emailToken.Token = token;
            emailToken.SendDate = DateTime.UtcNow.AddHours(4);

            await _context.EmailTokens.AddAsync(emailToken);
            await _context.SaveChangesAsync();

            string emailContentTemplate = _emailService.GetEmailConfirmationTemplate("emailConfirmationTemplate.html");

            string emailContent = emailContentTemplate.Replace("{USERNAME}", user.UserName).Replace("{CONFIRMATION_LINK}",
                confirmationLink);

            var message = new Message(new string[] { dto.Email! }, "Confirmation email link", emailContent);
            _emailService.SendEmail(message);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromForm]LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user is null) throw new LoginFailedException("Username or Password is wrong");
            var result = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!result) throw new LoginFailedException("Username or Password is wrong");

            if (user.EmailConfirmed==false)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var sendemail = await _context.EmailTokens.SingleOrDefaultAsync(u => u.AppUserId == user.Id);

                if (sendemail != null && sendemail.SendDate.AddHours(24) > DateTime.Now) throw new ConfirmationEmailIsAlreadySentException();
                else
                {
                    var confirmationLink = Url.Action("ConfirmEmail", "AppUsers", new { token, email = user.Email }, Request.Scheme);

                    string emailContentTemplate = _emailService.GetEmailConfirmationTemplate("emailConfirmationTemplate.html");
                    string emailContent = emailContentTemplate.Replace("{USERNAME}", user.UserName).Replace("{CONFIRMATION_LINK}", confirmationLink);

                    var message = new Message(new string[] { user.Email! }, "Confirmation email link", emailContent);

                    if(sendemail !=null)
                    {
                        sendemail.Token = token;
                        sendemail.SendDate = DateTime.UtcNow.AddHours(4);
                    }

                    await _context.SaveChangesAsync();
                    _emailService.SendEmail(message);
                }
            }
            return Ok(await _service.Login(dto));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LoginWithRefreshToken([FromForm]string refreshtoken)
        {
            return Ok(await _service.LoginWithRefreshTokenAsync(refreshtoken));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromForm]UpdateDto dto)
        {
            var email = await _userManager.Users.AnyAsync(u => u.Email == dto.Email);
            if (!email)
            {
                var user = await _userManager.FindByIdAsync(_userId);
                user.EmailConfirmed = false;
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "AppUsers", new { token, email = dto.Email }, Request.Scheme);
                var sendemail = await _context.EmailTokens.SingleOrDefaultAsync(u => u.AppUserId == user.Id);
                string emailContentTemplate = _emailService.GetEmailConfirmationTemplate("emailConfirmationTemplate.html");
                string emailContent = emailContentTemplate.Replace("{USERNAME}", dto.UserName).Replace("{CONFIRMATION_LINK}", confirmationLink);

                var message = new Message(new string[] { dto.Email! }, "Confirmation email link", emailContent);

                if (sendemail != null)
                {
                    sendemail.Token = token;
                    sendemail.SendDate = DateTime.UtcNow.AddHours(4);
                }

                await _context.SaveChangesAsync();
                _emailService.SendEmail(message);
            }
            await _service.UpdateAsync(dto);
            return Ok();
        }

        [HttpPatch("[action]")]
        public async Task<IActionResult> ChangePassword([FromForm]ChangePasswordDto dto)
        {
            await _service.ChangePassword(dto);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync(true));
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _service.GetByIdAsync(id,true));
        }

        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> SoftDelete(string id)
        {
            await _service.SoftDeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> ReverteDelete(string id)
        {
            await _service.ReverteDeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _service.Logout();
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddRole([FromForm]AddRoleDto dto)
        {
            await _service.AddRoleAsync(dto);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RemoveRole([FromForm] RemoveRoleDto dto)
        {
            await _service.RemoveRoleAsync(dto);
            return NoContent();
        }
    }
}


