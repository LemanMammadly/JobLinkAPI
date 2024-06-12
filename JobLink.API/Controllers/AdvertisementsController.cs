using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobLink.Business.Dtos.AdvertisementDtos;
using JobLink.Business.Dtos.JobDescriptionDtos;
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
        public async Task<IActionResult> GetTrue(int id)
        {
            return Ok(await _service.GetByIdAsync(id, true));
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetFalse(int id)
        {
            return Ok(await _service.GetByIdAsync(id, false));
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


        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutText(int id,[FromForm]List<int> ids, [FromForm]List<string> descs)
        {
            await _service.UpdateJobDescription(id,ids,descs);
            return NoContent();
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> PutReqruiment(int id, [FromForm] List<int> ids, [FromForm] List<string> descs)
        {
            await _service.UpdateReqruiment(id, ids, descs);
            return NoContent();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> CreateText(int id, [FromForm]List<string> descs)
        {
            await _service.UpdateAddJobDescription(id, descs);
            return NoContent();
        }

        [HttpPost("[action]/{id}")]
        public async Task<IActionResult> CreateReqruiment(int id, [FromForm] List<string> descs)
        {
            await _service.UpdateAddReqruiment(id, descs);
            return NoContent();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteText(int id, [FromForm] List<int> ids)
        {
            await _service.UpdateDeleteJobDescription(id, ids);
            return NoContent();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteReqruiment(int id, [FromForm] List<int> ids)
        {
            await _service.UpdateDeleteReqruiments(id, ids);
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

        [HttpGet("[action]")]
        public async Task<IActionResult> SortByDate([FromQuery] AdvertisementFilterDto dto)
        {
            return Ok(await _service.SortByDate(dto));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SortBy(Sort sort)
        {
            return Ok(await _service.SortBy(sort));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SortBySalary(Salary salary)
        {
            return Ok(await _service.SortBySalary(salary));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SortByArea(string area)
        {
            return Ok(await _service.SortByArea(area));
        }
    }
}

