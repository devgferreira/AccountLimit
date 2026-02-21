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

        public Result<decimal> AuthorizePixTransaction(decimal amount)
        {
            if (amount <= 0)
                return Result.Failure<decimal>("Amount must be greater than zero.");


            if (amount > PixTransactionLimit.Value)
                return Result.Failure<decimal>("Transaction amount exceeds the available limit.");

            UpdatePixTransactionLimit(PixTransactionLimit.Value - amount);

            return Result.Success(PixTransactionLimit.Value);
        }

        public static Result<LimitManagementInfo> Create(string rawCpf, string rawAgency, string rawAccount, decimal rawLimit)
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

            var limitResult = PixTransactionLimit.Create(rawLimit);
            if (limitResult.IsFailure)
                return Result.Failure<LimitManagementInfo>(limitResult.Error);

            var entity = new LimitManagementInfo
            { 
                Cpf = cpfResult.Value,
                Agency = agencyResult.Value,
                Account = accountResult.Value,
                PixTransactionLimit = limitResult.Value
            };

            return Result.Success(entity);
        }

    }
}
