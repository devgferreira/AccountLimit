using AccountLimit.Application.DTO.TransactionAuthorization;
using AccountLimit.Domain.Commom;

namespace AccountLimit.Application.Interface.TransactionAuthorization
{
    public interface ITransactionAuthorizationService
    {
        Task<Result> AuthorizePixTransaction(TransactionAuthorizationDTO request);
    }
}
