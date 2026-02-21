using AccountLimit.Application.DTO.TransactionAuthorization;
using AccountLimit.Domain.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Application.Interface.TransactionAuthorization
{
    public interface ITransactionAuthorizationService
    {
        Task<Result> AuthorizePixTransaction(TransactionAuthorizationDTO request);
    }
}
