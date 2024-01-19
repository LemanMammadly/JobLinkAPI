using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobLink.Business.Dtos.CompanyDtos;
using JobLink.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobLink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        readonly ICompanyService _service;

        public CompaniesController(ICompanyService service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm]CreateCompanyDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync(true));
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.GetByIdAsync(id,true));
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Update(int id,[FromForm]CompanyUpdateDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> ReverteSoftDelete(int id)
        {
            await _service.ReverteSoftDeleteAsync(id);
            return NoContent();
        }
    }
}

