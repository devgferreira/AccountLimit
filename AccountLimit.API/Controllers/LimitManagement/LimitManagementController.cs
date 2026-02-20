using AccountLimit.Application.DTO.LimitManagement;
using AccountLimit.Application.Interface.LimitManagement;
using AccountLimit.Domain.Entities.LimitManagement.Request;
using Microsoft.AspNetCore.Mvc;

namespace AccountLimit.API.Controllers.LimitManagement
{
    [ApiController]
    [Route("api/[controller]/")]
    public class LimitManagementController : ControllerBase
    {
        private readonly ILimitManagementService _service;

        public LimitManagementController(ILimitManagementService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLimitManagement([FromBody] LimitManagementCreateDTO request)
        {
            var result = await _service.CreateLimitManagement(request);
            if (result.IsFailure)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateLimitManagement([FromQuery] string cpf, [FromQuery] string agency, [FromBody] LimitManagementUpdateDTO request)
        {
            var result = await _service.UpdateLimitManagement(cpf, agency, request);
            if (result.IsFailure)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteLimitManagement([FromQuery] string cpf, [FromQuery] string agency)
        {
            var result = await _service.DeleteLimitManagement(cpf, agency);
            if (result.IsFailure)
                return BadRequest(result);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> SelectLimitManagement([FromQuery] LimitManagementRequest request)
        {
            var result = await _service.SelectLimitManagement(request);
            if (result.IsFailure)
                return BadRequest(result);
            return Ok(result);
        }
    }
}
