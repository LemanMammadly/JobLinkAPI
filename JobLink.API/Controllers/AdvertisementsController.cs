using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobLink.Business.Dtos.AdvertisementDtos;
using JobLink.Business.Services.Interfaces;
using JobLink.Core.Enums;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JobLink.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementsController : ControllerBase
    {
        readonly IAdvertisementService _service;

        public AdvertisementsController(IAdvertisementService service)
        {
            _service = service;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get()
        {
            return Ok(await _service.GetAllAsync(true));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAccept()
        {
            return Ok(await _service.GetAllAcceptAsync());
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _service.GetByIdAsync(id, true));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create([FromForm]CreateAdvertisementDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm]UpdateAdvertisementDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> SoftDelete(int id)
        {
            await _service.SoftDeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("[action]/{id}")]
        public async Task<IActionResult> ReverteSoftDelete(int id)
        {
            await _service.ReverteSoftDeleteAsync(id);
            return NoContent();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Accept(int id)
        {
            await _service.AcceptAdvertisement(id);
            return NoContent();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            await _service.RejectAdvertisement(id);
            return NoContent();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> ChangeState(int id,State state)
        {
            await _service.UpdateStateAsync(id,state);
            return NoContent();
        }
    }
}

