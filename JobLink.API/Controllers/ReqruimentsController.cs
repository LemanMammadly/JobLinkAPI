using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobLink.Business.Dtos.ReqruimentDtos;
using JobLink.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace JobLink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReqruimentsController : ControllerBase
    {
        readonly IReqruimentService _service;

        public ReqruimentsController(IReqruimentService service)
        {
            _service = service;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Post([FromForm]CreateReqruimentDto dto,int adverId)
        {
            await _service.CreateAsync(dto,adverId);
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
        public async Task<IActionResult> Put(int id,string text)
        {
            await _service.UpdateAsync(id,text);
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

