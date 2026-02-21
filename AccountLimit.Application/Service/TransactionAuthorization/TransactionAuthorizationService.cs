using AccountLimit.Application.DTO.TransactionAuthorization;
using AccountLimit.Application.Interface.TransactionAuthorization;
using AccountLimit.Domain.Commom;
using AccountLimit.Domain.Entities.LimitManagement.Request;
using AccountLimit.Domain.Interface;
using AccountLimit.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Application.Service.TransactionAuthorization
{
    public class TransactionAuthorizationService : ITransactionAuthorizationService
    {
        private readonly ILimitManagementRepository _limitManagementRepository;

        public TransactionAuthorizationService(ILimitManagementRepository limitManagementRepository)
        {
            _limitManagementRepository = limitManagementRepository;
        }

        public async Task<Result> AuthorizePixTransaction(TransactionAuthorizationDTO request)
        {
            var cpfCreated = Cpf.Create(request.Cpf);
            if (cpfCreated.IsFailure)
                return Result.Failure(cpfCreated.Error, HttpStatusCode.BadRequest.ToString());

            var agencyCreate = Agency.Create(request.Agency);
            if (agencyCreate.IsFailure)
                return Result.Failure(agencyCreate.Error, HttpStatusCode.BadRequest.ToString());

            var accountCreate = Account.Create(request.Account);
            if (accountCreate.IsFailure)
                return Result.Failure(accountCreate.Error, HttpStatusCode.BadRequest.ToString());

            var limitManagements = await _limitManagementRepository.SelectLimitManagement(
                new LimitManagementRequest { Agency = agencyCreate.Value.ToString(), Cpf = cpfCreated.Value.ToString() });

            var limitManagement = limitManagements.FirstOrDefault();
            if (limitManagement == null)
                return Result.Failure("Account not found for this CPF and Agency.", HttpStatusCode.NotFound.ToString());

            var authorize = limitManagement.AuthorizePixTransaction(request.Amount, accountCreate.Value.ToString());
            if (authorize.IsFailure)
                return Result.Failure(authorize.Error, new TransactionAuthorizationResponseDTO
                {
                    IsAuthorized = false,
                    LimitActual = limitManagement.PixTransactionLimit.Value
                });

            await _limitManagementRepository.UpdateLimitManagement(limitManagement);

            return Result.Success(new TransactionAuthorizationResponseDTO
            {
                IsAuthorized = true,
                LimitActual = authorize.Value
            });
        }
    }
}
