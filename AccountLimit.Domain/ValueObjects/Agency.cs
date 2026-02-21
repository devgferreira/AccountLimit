using AccountLimit.Domain.Commom;

namespace AccountLimit.Domain.ValueObjects
{
    public sealed record Agency
    {
        private Agency(string value)
        {
            Value = value;
        }
        public string Value;

        public static Result<Agency> Create(string rawAgency)
        {
            if (string.IsNullOrWhiteSpace(rawAgency))
            {
                return Result.Failure<Agency>("Agency cannot be null or empty.");
            }
            var digitsOnlyAgency = new string(rawAgency.Where(char.IsDigit).ToArray());
            if (digitsOnlyAgency.Length != 4)
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
