using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public string? Error { get; }
        public HttpStatusCode StatusCode { get; } 

        private Result(bool isSuccess, T? value, string? error, HttpStatusCode statusCode)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
            StatusCode = statusCode;
        }

        public static Result<T> Success(T value,HttpStatusCode statusCode=HttpStatusCode.OK)
            => new(true, value, null, statusCode);
        public static Result<T> Success(HttpStatusCode statusCode=HttpStatusCode.OK)
            => new(true, default, null, statusCode);

        public static Result<T> Failure(string error, HttpStatusCode statusCode)
            => new(false, default, error, statusCode);
    }
}
