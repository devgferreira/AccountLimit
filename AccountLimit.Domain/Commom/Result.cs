using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountLimit.Domain.Commom
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }

        protected Result(bool IsSucess, string error)
        {
            IsSuccess = IsSucess;
            Error = error;
        }

        public static Result Success() => new(true, string.Empty);

        public static Result Failure(string error) => new(false, error);

        public static Result<T> Success<T>(T value) => new(value, true, string.Empty);

        public static Result<T> Failure<T>(string error) => new(default, false, error);

    }
    public class Result<T> : Result
    {
        public T? Value { get; }
        protected internal Result(T? value, bool isSuccess, string error)
            : base(isSuccess, error)
        {
            Value = value;
        }
    }
}
