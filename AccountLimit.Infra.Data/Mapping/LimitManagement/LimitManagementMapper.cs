using AccountLimit.Domain.Commom;
using AccountLimit.Domain.Entities.LimitManagement;
using AccountLimit.Infra.Data.Entities.LimitManagement;

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
            return LimitManagementInfo.Create(
                rawCpf: entity.Cpf,
                rawAgency: entity.Agency,
                rawAccount: entity.Account,
                rawLimit: entity.PixTransactionLimit
            );
        }
    }


}
