using AccountLimit.Domain.Commom;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Domain.ValueObjects
{
    public sealed record PixTransactionLimit
    {
        public decimal Value { get; }

        private PixTransactionLimit(decimal value)
        {
            Value = value;
        }

        public static Result<PixTransactionLimit> Create(decimal rawLimit)
        {
            if (rawLimit < 0)
                return Result.Failure<PixTransactionLimit>("Pix transaction limit cannot be negative.");

            return Result.Success(new PixTransactionLimit(rawLimit));
        }

        public override string ToString()
        {
            return Value.ToString("F2");
        }
    }
}
