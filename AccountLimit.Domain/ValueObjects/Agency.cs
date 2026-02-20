using AccountLimit.Domain.Commom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Domain.ValueObjects
{
    public sealed record Agency
    {
        private Agency(string value)
        {
            Value = value;
        }
        private string Value;

        public static Result<Agency> Create(string rawAgency)
        {
            if (string.IsNullOrWhiteSpace(rawAgency))
            {
                return Result.Failure<Agency>("Agency cannot be null or empty.");
            }
            var digitsOnlyAgency = new string(rawAgency.Where(char.IsDigit).ToArray());
            if (digitsOnlyAgency.Length != 4 || digitsOnlyAgency.Length != 5)
            {
                return Result.Failure<Agency>("Agency must have 4 or 5 digits.");
            }
            return Result.Success(new Agency(digitsOnlyAgency));
        }
        public override string ToString()
        {
            return Value;
        }
    }
}
