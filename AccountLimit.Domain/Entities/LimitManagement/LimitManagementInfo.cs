using AccountLimit.Domain.Commom;
using AccountLimit.Domain.Enums;
using AccountLimit.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Domain.Entities.LimitManagement
{
    public class LimitManagementInfo
    {
        public Cpf Cpf { get; private set; }
        public Agency Agency { get; private set; }
        public Account Account { get; private set; }
        public Period Period { get; private set; }
        public PixTransactionLimit PixTransactionLimit { get; private set; }

        public LimitManagementInfo(PixTransactionLimit pixTransactionLimit)
        {
            PixTransactionLimit = pixTransactionLimit;
        }

        public LimitManagementInfo()
        {
        }

        public Result UpdatePixTransactionLimit(decimal newLimit)
        {
            var result = PixTransactionLimit.Create(newLimit);

            if (result.IsFailure)
                return Result.Failure(result.Error);

            PixTransactionLimit = result.Value;

            return Result.Success();
        }

        public static Result<LimitManagementInfo> Create(string rawCpf, string rawAgency, string rawAccount, Period period, decimal rawLimit)
        {
            var cpfResult = Cpf.Create(rawCpf);
            if (cpfResult.IsFailure)
                return Result.Failure<LimitManagementInfo>(cpfResult.Error);

            var agencyResult = Agency.Create(rawAgency);
            if (agencyResult.IsFailure)
                return Result.Failure<LimitManagementInfo>(agencyResult.Error);

            var accountResult = Account.Create(rawAccount);
            if (accountResult.IsFailure)
                return Result.Failure<LimitManagementInfo>(accountResult.Error);

            if (!Enum.GetValues<Period>().Contains(period))
                return Result.Failure<LimitManagementInfo>("Period invalid.");

            var limitResult = PixTransactionLimit.Create(rawLimit);
            if (limitResult.IsFailure)
                return Result.Failure<LimitManagementInfo>(limitResult.Error);

            var entity = new LimitManagementInfo
            { 
                Cpf = cpfResult.Value,
                Agency = agencyResult.Value,
                Account = accountResult.Value,
                Period = period,
                PixTransactionLimit = limitResult.Value
            };

            return Result.Success(entity);
        }

    }
}
