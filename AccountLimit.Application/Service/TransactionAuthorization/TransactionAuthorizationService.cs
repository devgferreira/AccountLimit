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
            var payerData = ValidateTransactionAuthorizationData(request.PayerCpf, request.PayerAgency, request.PayerAccount, "Payer");
            if (payerData.IsFailure) 
                return payerData;

            var receiverData = ValidateTransactionAuthorizationData(request.ReceiverCpf, request.ReceiverAgency, request.ReceiverAccount, "Receiver");
            if (receiverData.IsFailure)
                return receiverData;

            var (payerCpf, payerAgency, payerAccount) = payerData.Value;

            var payerLimitManagements = await _limitManagementRepository.SelectLimitManagement(
                new LimitManagementRequest
                {
                    Agency = payerAgency.ToString(),
                    Cpf = payerCpf.ToString()
                });

            var payerLimitManagement = payerLimitManagements.FirstOrDefault();
            if (payerLimitManagement == null)
                return Result.Failure("Account Payer not found for this CPF and Agency.",
                    HttpStatusCode.NotFound.ToString());

            var authorize = payerLimitManagement.AuthorizePixTransaction(request.Amount, payerAccount.ToString());
            if (authorize.IsFailure)
                return Result.Failure(authorize.Error, new TransactionAuthorizationResponseDTO
                {
                    IsAuthorized = false,
                    LimitActual = payerLimitManagement.PixTransactionLimit.Value
                });

            await _limitManagementRepository.UpdateLimitManagement(payerLimitManagement);

            return Result.Success(new TransactionAuthorizationResponseDTO
            {
                IsAuthorized = true,
                LimitActual = authorize.Value
            });
        }


        private Result<(Cpf cpf, Agency agency, Account account)> ValidateTransactionAuthorizationData(string cpf, string agency, string account, string paymentRole)
        {

            var cpfCreated = Cpf.Create(cpf);
            if (cpfCreated.IsFailure)
                return Result.Failure<(Cpf, Agency, Account)>(
                     paymentRole + ": " + cpfCreated.Error,
                    HttpStatusCode.BadRequest.ToString());

            var agencyCreated = Agency.Create(agency);
            if (agencyCreated.IsFailure)
                return Result.Failure<(Cpf, Agency, Account)>(
                    paymentRole + ": " + agencyCreated.Error,
                    HttpStatusCode.BadRequest.ToString());

            var accountCreated = Account.Create(account);
            if (accountCreated.IsFailure)
                return Result.Failure<(Cpf, Agency, Account)>(
                     paymentRole + ": " + accountCreated.Error,
                    HttpStatusCode.BadRequest.ToString());

            return Result.Success((cpfCreated.Value, agencyCreated.Value, accountCreated.Value));
        }
    }
}
