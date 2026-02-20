using AccountLimit.Domain.Commom;
using AccountLimit.Domain.Entities.LimitManagement;
using AccountLimit.Domain.Enums;
using AccountLimit.Infra.Data.Entities.LimitManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Infra.Data.Mapping.LimitManagement
{
    public static class LimitManagementMapper
    {
        public static LimitManagementEntity ToEntity(this LimitManagementInfo request)
        {
            return new LimitManagementEntity
            {
                Cpf = request.Cpf.Value,
                Agency = request.Agency.Value,
                Account = request.Account.Value,
                PixTransactionLimit = request.PixTransactionLimit.Value
            };

        }
        public static Result<LimitManagementInfo> ToDomain(this LimitManagementEntity entity)
        {
            if (!Enum.TryParse<Period>(entity.Period, true, out var period))
                return Result.Failure<LimitManagementInfo>("Period invalid.");

            return LimitManagementInfo.Create(
                rawCpf: entity.Cpf,
                rawAgency: entity.Agency,
                rawAccount: entity.Account,
                rawLimit: entity.PixTransactionLimit
            );
        }
    }

    
}
