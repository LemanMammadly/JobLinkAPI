using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobLink.Business.Dtos.JobDescriptionDtos;
using JobLink.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobLink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobDescriptionController : ControllerBase
    {
        readonly IJobDescriptionService _service;

        public JobDescriptionController(IJobDescriptionService service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm] CreateJobDescriptionDto dto, int adverId)
        {
            await _service.CreateAsync(dto, adverId);
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Put([FromForm]UpdateJobDescriptionDto dto)
        {
            await _service.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}

