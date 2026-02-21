using AccountLimit.Application.DTO.TransactionAuthorization;
using AccountLimit.Application.Interface.TransactionAuthorization;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> AuthorizeTransaction([FromBody] TransactionAuthorizationDTO request)
        {
            var result = await _transactionAuthorizationService.AuthorizePixTransaction(request);
            if (result.IsFailure)
            {
                return result.Code switch
                {
                    "NotFound" => NotFound(result),
                    "Ok" => Ok(result),
                    _ => BadRequest(result)
                };
            }
            return Ok(result);
        }

    }
}
