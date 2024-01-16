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
    }
}

