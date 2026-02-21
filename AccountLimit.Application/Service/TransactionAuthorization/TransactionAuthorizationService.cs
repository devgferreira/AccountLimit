using AccountLimit.Application.DTO.TransactionAuthorization;
using AccountLimit.Application.Interface.TransactionAuthorization;
using AccountLimit.Domain.Commom;
using AccountLimit.Domain.Entities.LimitManagement.Request;
using AccountLimit.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var limitManagements = await _limitManagementRepository.SelectLimitManagement(
                new LimitManagementRequest { Agency = request.Agency, Cpf = request.Cpf });

            var limitManagement = limitManagements.FirstOrDefault();
            if (limitManagement == null)
                return Result.Failure("Account not found for this CPF and Agency.");

            var authorize = limitManagement.AuthorizePixTransaction(request.Amount);
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
