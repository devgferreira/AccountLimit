using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AccountLimit.Domain.Commom
{
    public class Result
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IsSuccess { get; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool IsFailure => !IsSuccess;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Error { get; }

        [JsonIgnore]
        public string? Code { get; }

        protected Result(bool IsSucess, string error)
        {
            IsSuccess = IsSucess;
            Error = error;
        }

        protected Result(bool IsSucess, string error, string code)
        {
            IsSuccess = IsSucess;
            Error = error;
            Code = code;
        }

        protected Result(bool IsSucess)
        {
            IsSuccess = IsSucess;
        }

        public static Result Success() => new(true, null);

        public static Result Failure(string error, string code) => new(false, error, code);

        public static Result<T> Success<T>(T value) => new(value, true, null);

        public static Result<T> Failure<T>(string error) => new(default, false, error);
        public static Result<T> Failure<T>(string error, T value) => new(value, false, error);

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
