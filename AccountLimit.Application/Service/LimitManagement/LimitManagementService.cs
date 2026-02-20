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
                request.Period,
                request.PixTransactionLimit
            );

            if (createResult.IsFailure)
                return Result.Failure(createResult.Error);

            await _repository.CreateLimitManagement(createResult.Value);

            return Result.Success();
        }

        public async Task<Result> DeleteLimitManagement(string cpf)
        {
            try
            {
                await LimitManagementExists(cpf);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }

            await _repository.DeleteLimitManagement(cpf);
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
                Period = x.Period,
                PixTransactionLimit = x.PixTransactionLimit.Value
            }).ToList());


        }

        public async Task<Result> UpdateLimitManagement(string cpf, LimitManagementUpdateDTO request)
        {
            var limitManagement = await LimitManagementExists(cpf);

            var updateePixTransactionLimitResult = limitManagement.UpdatePixTransactionLimit(request.PixTransactionLimit);
            if (updateePixTransactionLimitResult.IsFailure)
                return updateePixTransactionLimitResult;

            await _repository.UpdateLimitManagement(limitManagement);

            return Result.Success();
        }


        #region Validation
        private async Task<LimitManagementInfo> LimitManagementExists(string cpf)
        {
            var limitManagementList = await _repository.SelectLimitManagement(new LimitManagementRequest { Cpf = cpf});
            if (!limitManagementList.Any())
                throw new Exception("Limit management not found.");
            return limitManagementList.FirstOrDefault();
        }
        #endregion
    }
}
