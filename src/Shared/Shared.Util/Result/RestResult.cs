using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared.Util.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Shared.Util.Result
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RestResult<T> : RestResult
    {
        /// <summary>
        /// 
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public RestResult(T data)
        {
            Data = data;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageResult<T> : RestResult
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<T> Page { get; }

        /// <summary>
        /// 
        /// </summary>
        public long Count { get; }

        /// <summary>
        /// 
        /// </summary>
        public int PageCount { get; }

        /// <summary>
        /// 
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentPage { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public PageResult(PagedList<T> data)
        {
            Page = data.Items;
            Count = data.TotalCount;
            PageCount = data.TotalPages;
            PageSize = data.ItemsPerPage;
            CurrentPage = data.CurrentPage;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RestResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string CorrelationId { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Success { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<string> Errors { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        protected int StatusCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected RestResult()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static RestResult Ok()
        {
            return new RestResult
            {
                Success = true,
                Errors = new List<string>(),
                StatusCode = 200
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static RestResult Fail(string error)
        {
            return new RestResult
            {
                Success = false,
                Errors = new List<string> { error },
                StatusCode = 400
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static RestResult Create(Exception ex)
        {
            return new RestResult
            {
                Success = false,
                Errors = new List<string> { ex.Message },
                StatusCode = 500
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static RestResult Create(ModelStateDictionary dict)
        {
            return new RestResult
            {
                Success = dict.IsValid,
                Errors = dict.IsValid
                ? new List<string>()
                : dict
                .Values
                .SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .ToList(),
                StatusCode = dict.IsValid ? 200 : 422
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static RestResult Create<T>(PagedList<T> result)
        {
            return new PageResult<T>(result)
            {
                Success = true,
                Errors = new List<string>(),
                StatusCode = 200
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static RestResult Create<T>(T result)
        {
            return new RestResult<T>(result)
            {
                Success = true,
                Errors = new List<string>(),
                StatusCode = 200
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static RestResult Create<T>(Result<T> result)
        {
            return new RestResult<T>(result.IsSuccess ? result.Data : default(T))
            {
                CorrelationId = result.CorrelationId,
                Success = result.IsSuccess,
                Errors = result.IsSuccess
                    ? new List<string>()
                    : result.Error
                        .Split(';')
                        .ToList(),
                StatusCode = result.IsSuccess ? 200 : 400
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static RestResult Create(Result result)
        {
            return new RestResult
            {
                CorrelationId = result.CorrelationId,
                Success = result.IsSuccess,
                Errors = result.IsSuccess
                    ? new List<string>()
                    : result.Error
                        .Split(';')
                        .ToList(),
                StatusCode = result.IsSuccess ? 200 : 400
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public static IActionResult CreateHttpResponse(ModelStateDictionary dict)
        {
            return Create(dict).ToHttp();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static IActionResult CreateHttpResponse(Exception ex)
        {
            return Create(ex).ToHttp();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult CreateHttpResponse<T>(T data)
        {
            return Create(data).ToHttp();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult CreateHttpResponse<T>(PagedList<T> data)
        {
            return Create(data).ToHttp();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult CreateHttpResponse<T>(Result<T> data)
        {
            return Create(data).ToHttp();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IActionResult CreateHttpResponse(Result data)
        {
            return Create(data).ToHttp();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IActionResult CreateOkHttpResponse()
        {
            return Ok().ToHttp();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static IActionResult CreateFailHttpResponse(string error)
        {
            return Fail(error).ToHttp();
        }

        private IActionResult ToHttp()
        {
            return new ObjectResult(this)
            {
                StatusCode = StatusCode
            };
        }
    }
}
