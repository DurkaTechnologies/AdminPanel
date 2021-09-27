using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminPanel.Application.Common.Models
{
    public interface IResult
    {
        string Message { get; set; }

        bool Succeeded { get; set; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }

    public class Result : IResult
    {
        public bool Failed => !Succeeded;

        public string Message { get; set; }

        public bool Succeeded { get; set; }

        public static IResult Failure()
        {
            return new Result { Succeeded = false };
        }

        public static IResult Failure(string message)
        {
            return new Result { Succeeded = false, Message = message };
        }

        public static Task<IResult> FailureAsync()
        {
            return Task.FromResult(Failure());
        }

        public static Task<IResult> FailureAsync(string message)
        {
            return Task.FromResult(Failure(message));
        }

        public static IResult Success()
        {
            return new Result { Succeeded = true };
        }

        public static IResult Success(string message)
        {
            return new Result { Succeeded = true, Message = message };
        }

        public static Task<IResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IResult> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }
    }

    public class Result<T> : Result, IResult<T>
    {
        public Result()
        {
        }

        public T Data { get; set; }

        public static new Result<T> Failure()
        {
            return new Result<T> { Succeeded = false };
        }

        public static new Result<T> Failure(string message)
        {
            return new Result<T> { Succeeded = false, Message = message };
        }

        public static new Task<Result<T>> FailureAsync()
        {
            return Task.FromResult(Failure());
        }

        public static new Task<Result<T>> FailureAsync(string message)
        {
            return Task.FromResult(Failure(message));
        }

        public static new Result<T> Success()
        {
            return new Result<T> { Succeeded = true };
        }

        public static new Result<T> Success(string message)
        {
            return new Result<T> { Succeeded = true, Message = message };
        }

        public static Result<T> Success(T data)
        {
            return new Result<T> { Succeeded = true, Data = data };
        }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T> { Succeeded = true, Data = data, Message = message };
        }

        public static new Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static new Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<Result<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }
    }


}
