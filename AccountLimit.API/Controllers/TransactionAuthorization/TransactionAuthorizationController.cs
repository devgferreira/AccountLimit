using AccountLimit.Application.DTO.TransactionAuthorization;
using AccountLimit.Application.Interface.TransactionAuthorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountLimit.API.Controllers.TransactionAuthorization
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionAuthorizationController : ControllerBase
    {
        private readonly ITransactionAuthorizationService _transactionAuthorizationService;

        public TransactionAuthorizationController(ITransactionAuthorizationService transactionAuthorizationService)
        {
            _transactionAuthorizationService = transactionAuthorizationService;
        }

        [HttpPost]
        public async Task<IActionResult> AuthorizeTransaction([FromBody] TransactionAuthorizationDTO    request)
        {
            var result = await _transactionAuthorizationService.AuthorizePixTransaction(request);
            if (result.IsFailure)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
