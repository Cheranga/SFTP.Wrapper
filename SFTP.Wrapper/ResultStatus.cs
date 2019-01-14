using System;

namespace SFTP.Wrapper
{
    public class ResultStatus<T>
    {
        public bool Status { get; set; }

        public Exception Exception { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public static ResultStatus<T> Error()
        {
            return new ResultStatus<T>
            {
                Status = false
            };
        }

        public static ResultStatus<T> Error(string message)
        {
            return new ResultStatus<T>
            {
                Status = false,
                Message = message
            };
        }

        public static ResultStatus<T> Error(string message, Exception exception)
        {
            return new ResultStatus<T>
            {
                Status = false,
                Message = message,
                Exception = exception
            };
        }

        public static ResultStatus<T> Error(Exception exception)
        {
            return new ResultStatus<T>
            {
                Status = false,
                Exception = exception
            };
        }

        public static ResultStatus<T> Error(T data)
        {
            return new ResultStatus<T>
            {
                Status = false,
                Data = data
            };
        }

        public static ResultStatus<T> Success()
        {
            return new ResultStatus<T>
            {
                Status = true
            };
        }

        public static ResultStatus<T> Success(T data)
        {
            return new ResultStatus<T>
            {
                Status = true,
                Data = data
            };
        }

        public static ResultStatus<T> Success(T data, string message)
        {
            return new ResultStatus<T>
            {
                Status = true,
                Data = data,
                Message = message
            };
        }
    }
}