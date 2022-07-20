using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace Shared.Util.Result
{
    internal class ResultCommonLogic<TError>
    {
        public string CorrelationId { get; private set; }
        public bool IsFailure { get; }
        public bool IsSuccess => !IsFailure;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TError _error;

        public TError Error
        {
            [DebuggerStepThrough]
            get
            {
                if (IsSuccess)
                    throw new InvalidOperationException("There is no error message for success.");

                return _error;
            }
        }

        [DebuggerStepThrough]
        public ResultCommonLogic(bool isFailure, TError error, string correlationId)
        {
            if (isFailure)
            {
                if (error == null)
                    throw new ArgumentNullException(nameof(error), ResultMessages.ErrorObjectIsNotProvidedForFailure);
            }
            else
            {
                if (error != null)
                    throw new ArgumentException(ResultMessages.ErrorObjectIsProvidedForSuccess, nameof(error));
            }

            IsFailure = isFailure;
            _error = error;
            CorrelationId = correlationId;
        }
        public void SetCorrelationId(string correlationId)
        {
            CorrelationId = correlationId;
        }

        public void GetObjectData(SerializationInfo oInfo, StreamingContext oContext)
        {
            oInfo.AddValue("IsFailure", IsFailure);
            oInfo.AddValue("IsSuccess", IsSuccess);
            oInfo.AddValue("CorrelationId", CorrelationId);
            if (IsFailure)
            {
                oInfo.AddValue("Error", Error);
            }
        }


    }

    internal sealed class ResultCommonLogic : ResultCommonLogic<string>
    {
        [DebuggerStepThrough]
        public static ResultCommonLogic Create(bool isFailure, string error, string correlationId)
        {
            if (isFailure)
            {
                if (string.IsNullOrEmpty(error))
                    throw new ArgumentNullException(nameof(error), ResultMessages.ErrorMessageIsNotProvidedForFailure);
            }
            else
            {
                if (error != null)
                    throw new ArgumentException(ResultMessages.ErrorMessageIsProvidedForSuccess, nameof(error));
            }

            return new ResultCommonLogic(isFailure, error, correlationId);
        }

        public ResultCommonLogic(bool isFailure, string error, string correlationId) : base(isFailure, error, correlationId)
        {
        }
    }

    internal static class ResultMessages
    {
        public static readonly string ErrorObjectIsNotProvidedForFailure =
            "You have tried to create a failure result, but error object appeared to be null, please review the code, generating error object.";

        public static readonly string ErrorObjectIsProvidedForSuccess =
            "You have tried to create a success result, but error object was also passed to the constructor, please try to review the code, creating a success result.";

        public static readonly string ErrorMessageIsNotProvidedForFailure = "There must be error message for failure.";

        public static readonly string ErrorMessageIsProvidedForSuccess = "There should be no error message for success.";

        public static readonly string ErrorMessageIsCorrelationId = "You have tried to create a result, but correlationId appeared to be null or empty, please review the code.";
    }

    /// <summary>
    /// 
    /// </summary>
    public struct Result : ISerializable
    {
        private static readonly Result OkResult = new Result(false, null, string.Empty);

        void ISerializable.GetObjectData(SerializationInfo oInfo, StreamingContext oContext)
        {
            _logic.GetObjectData(oInfo, oContext);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ResultCommonLogic _logic;

        /// <summary>
        /// 
        /// </summary>
        public bool IsFailure => _logic.IsFailure;

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess => _logic.IsSuccess;

        /// <summary>
        /// 
        /// </summary>
        public string Error => _logic.Error;

        /// <summary>
        /// 
        /// </summary>
        public string CorrelationId => _logic.CorrelationId;

        [DebuggerStepThrough]
        private Result(bool isFailure, string error, string correlationId = null)
        {
            _logic = ResultCommonLogic.Create(isFailure, error, correlationId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="correlationId"></param>
        public void SetCorrelationId(string correlationId)
        {
            _logic.SetCorrelationId(correlationId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Result Ok()
        {
            return OkResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Result Ok(string correlationId)
        {
            return new Result(false, null, correlationId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Result Fail(string error, string correlationId = null)
        {
            return new Result(true, error, correlationId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Result<T> Ok<T>(T value, string correlationId = null)
        {
            return new Result<T>(false, value, null, correlationId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="error"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Result<T> Fail<T>(string error, string correlationId = null)
        {
            return new Result<T>(true, default(T), error, correlationId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="value"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Result<TValue, TError> Ok<TValue, TError>(TValue value, string correlationId = null) where TError : class
        {
            return new Result<TValue, TError>(false, value, default(TError), correlationId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TError"></typeparam>
        /// <param name="error"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Result<TValue, TError> Fail<TValue, TError>(TError error, string correlationId = null) where TError : class
        {
            return new Result<TValue, TError>(true, default(TValue), error, correlationId);
        }

        /// <summary>
        /// Returns first failure in the list of <paramref name="results"/>. If there is no failure returns success.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="results">List of results.</param>
        [DebuggerStepThrough]
        public static Result FirstFailureOrSuccess(string correlationId, params Result[] results)
        {
            foreach (var result in results)
            {
                if (result.IsFailure)
                    return Fail(result.Error, correlationId);
            }

            return Ok();
        }

        /// <summary>
        /// Returns failure which combined from all failures in the <paramref name="results"/> list. Error messages are separated by <paramref name="errorMessagesSeparator"/>. 
        /// If there is no failure returns success.
        /// </summary>
        /// <param name="errorMessagesSeparator">Separator for error messages.</param>
        /// <param name="correlationId"></param>
        /// <param name="results">List of results.</param>
        [DebuggerStepThrough]
        public static Result Combine(string errorMessagesSeparator, string correlationId, params Result[] results)
        {
            var failedResults = results.Where(x => x.IsFailure).ToList();

            if (!failedResults.Any())
                return Ok();

            var errorMessage = string.Join(errorMessagesSeparator, failedResults.Select(x => x.Error).ToArray());
            return Fail(errorMessage, correlationId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Result Combine(string correlationId, params Result[] results)
        {
            return Combine(", ", correlationId, results);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="correlationId"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Result Combine<T>(string correlationId, params Result<T>[] results)
        {
            return Combine(", ", correlationId, results);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="errorMessagesSeparator"></param>
        /// <param name="correlationId"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Result Combine<T>(string errorMessagesSeparator, string correlationId, params Result<T>[] results)
        {
            var untyped = results.Select(result => (Result)result).ToArray();
            return Combine(errorMessagesSeparator, correlationId, untyped);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Result<T> : ISerializable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ResultCommonLogic _logic;

        /// <summary>
        /// 
        /// </summary>
        public bool IsFailure => _logic.IsFailure;

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess => _logic.IsSuccess;

        /// <summary>
        /// 
        /// </summary>
        public string Error => _logic.Error;

        /// <summary>
        /// 
        /// </summary>
        public string CorrelationId => _logic.CorrelationId;

        void ISerializable.GetObjectData(SerializationInfo oInfo, StreamingContext oContext)
        {
            _logic.GetObjectData(oInfo, oContext);

            if (IsSuccess)
            {
                oInfo.AddValue("Data", Data);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="correlationId"></param>
        public void SetCorrelationId(string correlationId)
        {
            _logic.SetCorrelationId(correlationId);
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly T _data;

        /// <summary>
        /// 
        /// </summary>
        public T Data
        {
            [DebuggerStepThrough]
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("There is no value for failure.");

                return _data;
            }
        }

        [DebuggerStepThrough]
        internal Result(bool isFailure, T data, string error, string correlationId = null)
        {
            if (!isFailure && data == null)
                throw new ArgumentNullException(nameof(data));

            _logic = ResultCommonLogic.Create(isFailure, error, correlationId);
            _data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator Result(Result<T> result)
        {
            if (result.IsSuccess)
                return Result.Ok();
            else
                return Result.Fail(result.Error, result.CorrelationId);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TError"></typeparam>
    public struct Result<TValue, TError> : ISerializable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ResultCommonLogic<TError> _logic;

        /// <summary>
        /// 
        /// </summary>
        public bool IsFailure => _logic.IsFailure;

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess => _logic.IsSuccess;

        /// <summary>
        /// 
        /// </summary>
        public TError Error => _logic.Error;

        /// <summary>
        /// 
        /// </summary>
        public string CorrelationId => _logic.CorrelationId;

        void ISerializable.GetObjectData(SerializationInfo oInfo, StreamingContext oContext)
        {
            _logic.GetObjectData(oInfo, oContext);

            if (IsSuccess)
            {
                oInfo.AddValue("Data", Data);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TValue _data;

        /// <summary>
        /// 
        /// </summary>
        public TValue Data
        {
            [DebuggerStepThrough]
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException("There is no value for failure.");

                return _data;
            }
        }

        [DebuggerStepThrough]
        internal Result(bool isFailure, TValue data, TError error, string correlationId = null)
        {
            if (!isFailure && data == null)
                throw new ArgumentNullException(nameof(data));

            _logic = new ResultCommonLogic<TError>(isFailure, error, correlationId);
            _data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator Result(Result<TValue, TError> result)
        {
            if (result.IsSuccess)
                return Result.Ok();
            else
                return Result.Fail(result.Error.ToString(), result.CorrelationId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator Result<TValue>(Result<TValue, TError> result)
        {
            if (result.IsSuccess)
                return Result.Ok(result.Data, result.CorrelationId);
            else
                return Result.Fail<TValue>(result.Error.ToString(), result.CorrelationId);
        }
    }
}
