using AccountLimit.Domain.Commom;

namespace AccountLimit.Domain.ValueObjects
{
    public sealed record Account
    {
        private Account(string value)
        {
            Value = value;
        }
        public string Value { get; }

        public static Result<Account> Create(string rawAccount)
        {
            if (string.IsNullOrWhiteSpace(rawAccount))
            {
                return Result.Failure<Account>("Account cannot be null or empty.");
            }
            var digitsOnlyAccount = new string(rawAccount.Where(char.IsDigit).ToArray());
            if (digitsOnlyAccount.Length != 6)
            {
                return Result.Failure<Account>("Account must have exactly 6 digits.");
            }
            return Result.Success(new Account(digitsOnlyAccount));
        }
        public override string ToString()
        {
            return Value;
        }

    }
}
