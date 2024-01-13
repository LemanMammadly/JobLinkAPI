using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobLink.Business.Dtos.RolesDtos;
using JobLink.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobLink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(string name)
        {
            await _roleService.CreateAsync(name);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _roleService.GetAllAync());
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Put([FromForm]UpdateRolesDto dto)
        {
            await _roleService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(string name)
        {
            await _roleService.DeleteAsync(name);
            return NoContent();
        }
    }
}

