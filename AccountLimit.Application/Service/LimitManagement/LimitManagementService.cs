using AccountLimit.Application.DTO.LimitManagement;
using AccountLimit.Application.Interface.LimitManagement;
using AccountLimit.Domain.Commom;
using AccountLimit.Domain.Entities.LimitManagement;
using AccountLimit.Domain.Entities.LimitManagement.Request;
using AccountLimit.Domain.Interface;
using AccountLimit.Domain.ValueObjects;
using System.Net;
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
                return Result.Failure(createResult.Error, HttpStatusCode.BadRequest.ToString());

            var limitManagement = await LimitManagementExists(new LimitManagementRequest { Cpf = request.Cpf, Agency = request.Agency });

            if (limitManagement != null)
                return Result.Failure("Limit management already exists for this CPF and Agency.", HttpStatusCode.OK.ToString());

            await _repository.CreateLimitManagement(createResult.Value);

            return Result.Success();
        }

        public async Task<Result> DeleteLimitManagement(string cpf, string agency)
        {

            var cpfCreated = Cpf.Create(cpf);
            if (cpfCreated.IsFailure)
                return Result.Failure(cpfCreated.Error, HttpStatusCode.BadRequest.ToString());

            var agencyCreate = Agency.Create(agency);
            if (agencyCreate.IsFailure)
                return Result.Failure(agencyCreate.Error, HttpStatusCode.BadRequest.ToString());

            var limitManagement = await LimitManagementExists(new LimitManagementRequest { Cpf = cpfCreated.Value.ToString(), Agency = agencyCreate.Value.ToString() });
            if (limitManagement == null)
                return Result.Failure("Limit Managemnet not found", HttpStatusCode.NotFound.ToString());


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
            var cpfCreated = Cpf.Create(cpf);
            if (cpfCreated.IsFailure)
                return Result.Failure(cpfCreated.Error, HttpStatusCode.BadRequest.ToString());

            var agencyCreate = Agency.Create(agency);
            if (agencyCreate.IsFailure)
                return Result.Failure(agencyCreate.Error, HttpStatusCode.BadRequest.ToString());

            var limitManagement = await LimitManagementExists(new LimitManagementRequest { Cpf = cpfCreated.Value.ToString(), Agency = agencyCreate.Value.ToString() });
            if(limitManagement == null)
                return Result.Failure("Limit Managemnet not found", HttpStatusCode.NotFound.ToString());

            var updateePixTransactionLimitResult = limitManagement.UpdatePixTransactionLimit(request.PixTransactionLimit);
            if (updateePixTransactionLimitResult.IsFailure)
                return updateePixTransactionLimitResult;

            await _repository.UpdateLimitManagement(limitManagement);

            return Result.Success();
        }


        #region Validation

        private async Task<LimitManagementInfo> LimitManagementExists(LimitManagementRequest request)
        {
            var limitManagementList = await _repository.SelectLimitManagement(request);
            return limitManagementList?.FirstOrDefault();
        }
        #endregion
    }
}
