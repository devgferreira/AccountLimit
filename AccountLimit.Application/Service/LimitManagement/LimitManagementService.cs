using AccountLimit.Application.DTO.LimitManagement;
using AccountLimit.Application.Interface.LimitManagement;
using AccountLimit.Domain.Commom;
using AccountLimit.Domain.Entities.LimitManagement;
using AccountLimit.Domain.Entities.LimitManagement.Request;
using AccountLimit.Domain.Interface;
using AccountLimit.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Application.Service.LimitManagement
{
    public class LimitManagementService : ILimitManagementService
    {
        private readonly ILimitManagementRepository _repository;

        public LimitManagementService(ILimitManagementRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> CreateLimitManagement(LimitManagementCreateDTO request)
        {

            var createResult = LimitManagementInfo.Create(
                request.Cpf,
                request.Agency,
                request.Account,
                request.PixTransactionLimit
            );

            if (createResult.IsFailure)
                return Result.Failure(createResult.Error);

            var limitManagement = await LimitManagementExists(new LimitManagementRequest { Cpf = request.Cpf, Agency = request.Agency });

            if (limitManagement.IsSuccess)
                return Result.Failure("Limit management already exists for this CPF and Agency.");

            await _repository.CreateLimitManagement(createResult.Value);

            return Result.Success();
        }

        public async Task<Result> DeleteLimitManagement(string cpf, string agency)
        {

            var limitManagement = await LimitManagementExists(new LimitManagementRequest { Cpf = cpf, Agency = agency });
            if (limitManagement.IsFailure)
                return Result.Failure(limitManagement.Error);


            await _repository.DeleteLimitManagement(cpf, agency);
            return Result.Success();
        }

        public async Task<Result> SelectLimitManagement(LimitManagementRequest request)
        {
            var result = await _repository.SelectLimitManagement(request);
            return Result.Success(result.Select(x => new LimitManagementDTO
            {
                Cpf = x.Cpf.ToString(),
                Agency = x.Agency.ToString(),
                Account = x.Account.ToString(),
                PixTransactionLimit = x.PixTransactionLimit.Value
            }).ToList());


        }

        public async Task<Result> UpdateLimitManagement(string cpf, string agency, LimitManagementUpdateDTO request)
        {
            var limitManagement = await LimitManagementExists(new LimitManagementRequest { Cpf = cpf, Agency = agency });
            if(limitManagement.IsFailure)
                return Result.Failure(limitManagement.Error);


            var updateePixTransactionLimitResult = limitManagement.Value.UpdatePixTransactionLimit(request.PixTransactionLimit);
            if (updateePixTransactionLimitResult.IsFailure)
                return updateePixTransactionLimitResult;

            await _repository.UpdateLimitManagement(limitManagement.Value);

            return Result.Success();
        }


        #region Validation
        private async Task<Result<LimitManagementInfo>> LimitManagementExists(LimitManagementRequest request)
        {
            var limitManagementList = await _repository.SelectLimitManagement(request);
            if (!limitManagementList.Any())
                Result.Failure("Limit management not found.");
            return Result.Success(limitManagementList.FirstOrDefault());
        }
        #endregion
    }
}
