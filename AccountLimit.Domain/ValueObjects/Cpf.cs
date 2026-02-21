using AccountLimit.Domain.Commom;
namespace AccountLimit.Domain.ValueObjects
{
    public sealed record Cpf
    {
        private Cpf(string value)
        {
            Value = value;
        }
        public string Value { get; }

        public static Result<Cpf> Create(string rawCpf)
        {
            if (string.IsNullOrWhiteSpace(rawCpf))
            {
                return Result.Failure<Cpf>("CPF cannot be null or empty.");
            }

            var digitsOnlyCpf = new string(rawCpf.Where(char.IsDigit).ToArray());

            if (digitsOnlyCpf.Length != 11)
            {
                return Result.Failure<Cpf>("CPF must have exactly 11 digits.");
            }

            if (Enumerable.Range(0, 10).Any(d => digitsOnlyCpf == new string((char)('0' + d), 11)))
                return Result.Failure<Cpf>("CPF cannot have identical numbers.");

            int sum = 0;
            for (int i = 0; i < 9; i++)
                sum += (digitsOnlyCpf[i] - '0') * (10 - i);

            int remains = sum % 11;
            int digit1 = remains < 2 ? 0 : 11 - remains;

            if ((digitsOnlyCpf[9] - '0') != digit1)
                return Result.Failure<Cpf>("CPF invalid.");

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += (digitsOnlyCpf[i] - '0') * (11 - i);

            remains = sum % 11;
            int digit2 = remains < 2 ? 0 : 11 - remains;

            if ((digitsOnlyCpf[10] - '0') != digit2)
                return Result.Failure<Cpf>("CPF invalid.");

            return Result.Success(new Cpf(digitsOnlyCpf));
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
