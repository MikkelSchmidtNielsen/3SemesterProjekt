using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ResultInterfaces;

namespace Common
{
    /// <summary>
    /// Generic wrapper which makes it possible to handle errors and responses in a control manner.
    /// </summary>
    public class Result<T> : IResult<T>, IResultConflict<T>, IResultError<T>, IResultSuccess<T>
    {
        public T OriginalType { get; }
        public T? CurrentType { get; }
        public Exception? Exception { get; }
        public ResultType Outcome { get; private set; }
        public enum ResultType { Success, Conflict, Error }

        public Result(T originalType)
        {
            OriginalType = originalType;
        }

        public Result(T originalType, Exception exception)
        {
            OriginalType = originalType;
            Exception = exception;
        }

        public Result(T originalType, T currentType, Exception exception)
        {
            OriginalType = originalType;
            CurrentType = currentType;
            Exception = exception;
        }

        public bool IsSucces()
        {
            if (Outcome == ResultType.Success)
            {
                return true;
            }

            return false;
        }

        public IResultSuccess<T> GetSuccess()
        {
            return this;
        }

        public bool IsError()
        {
            if (Outcome == ResultType.Error)
            {
                return true;
            }

            return false;
        }

        public IResultError<T> GetError()
        {
            return this;
        }

        public bool IsConflict()
        {
            if (Outcome == ResultType.Conflict)
            {
                return true;
            }

            return false;
        }

        public IResultConflict<T> GetConflict()
        {
            return this;
        }

        /// <summary>
        /// Gives the wrapper a successful response
        /// </summary>
        public static Result<T> Success(T originalType)
        {
            Result<T> instance = new Result<T>(originalType);

            instance.Outcome = ResultType.Success;

            return instance;
        }

        /// <summary>
        /// Gives the wrapper a conflict response
        /// </summary>
        public static Result<T> Conflict(T originalType, T currentType, Exception exception)
        {
            Result<T> instance = new Result<T>(originalType, currentType, exception);

            instance.Outcome = ResultType.Conflict;

            return instance;
        }

        /// <summary>
        /// Gives the wrapper an error response
        /// </summary>
        public static Result<T> Error(T originalType, Exception exception)
        {
            Result<T> instance = new Result<T>(originalType, exception);

            instance.Outcome = ResultType.Error;

            return instance;
        }

        /// <summary>
        /// Gives the wrapper an error response without original type
        /// </summary>
        public static Result<T> Error(Exception exception)
        {
            Result<T> instance = new Result<T>(default!, exception);

            instance.Outcome = ResultType.Error;

            return instance;
        }
    }
}
