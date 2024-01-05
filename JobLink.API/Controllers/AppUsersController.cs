using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobLink.Business.Dtos.AppUserDtos;
using JobLink.Business.ExternalServices.Interfaces;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public AppUsersController(IAppUserService service, UserManager<AppUser> userManager, IEmailSenderService emailService)
        {
            _service = service;
            _userManager = userManager;
            _emailService = emailService;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm]RegisterDto dto)
        {
            await _service.Register(dto);
            var user = await _userManager.FindByEmailAsync(dto.Email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "AppUsers", new { token, email = dto.Email },
                Request.Scheme);
            var message = new Message(new string[] { dto.Email! }, "Confirmation email link", confirmationLink!);
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
    }
}

