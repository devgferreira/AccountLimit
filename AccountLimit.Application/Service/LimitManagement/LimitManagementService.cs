using AccountLimit.Application.DTO.LimitManagement;
using AccountLimit.Application.Interface.LimitManagement;
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

        public async Task CreateLimitManagement(LimitManagementCreateDTO request)
        {
            var limitManagement = LimitManagementInfo.Create(request.Cpf, request.Agency, request.Account, request.Period, request.PixTransactionLimit);
            await _repository.CreateLimitManagement(limitManagement.Value);
        }

        public async Task DeleteLimitManagement(Guid id)
        {
            await LimitManagementExists(id);
            await _repository.DeleteLimitManagement(id);
        }

        public async Task<List<LimitManagementDTO>> SelectLimitManagement(LimitManagementRequest request)
        {
            var limitManagementList = await _repository.SelectLimitManagement(request);
            return limitManagementList.Select(x => new LimitManagementDTO
            {
                Id = x.Id,
                Cpf = x.Cpf.ToString(),
                Agency = x.Agency.ToString(),
                Account = x.Account.ToString(),
                Period = x.Period,
                PixTransactionLimit = x.PixTransactionLimit.Value
            }).ToList();
        }

        public async Task UpdateLimitManagement(Guid id, LimitManagementUpdateDTO request)
        {
            await LimitManagementExists(id);
            var pixTransactionLimit = PixTransactionLimit.Create(request.PixTransactionLimit);
            await _repository.UpdateLimitManagement(id, new LimitManagementInfo(pixTransactionLimit.Value));
        }


        #region Validation
        private async Task LimitManagementExists(Guid id)
        {
            var limitManagementList = await _repository.SelectLimitManagement(new LimitManagementRequest { Id = id });
            if (!limitManagementList.Any())
                throw new Exception("Limit management not found.");
        }
        #endregion
    }
}
