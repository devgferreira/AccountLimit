using AccountLimit.Domain.Commom;

namespace AccountLimit.Domain.ValueObjects
{
    public sealed record PixTransactionLimit
    {

        private PixTransactionLimit(decimal value)
        {
            Value = value;
        }
        public decimal Value { get; }

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
