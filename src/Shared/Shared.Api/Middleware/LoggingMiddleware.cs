using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using Shared.Util.Common.Constants;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Api.Middleware
{
    /// <summary>
    /// Middleware responsible for capturing requests and responses, consolidating and logging in only one entry.
    /// </summary>
    /// <example>
    /// Using the root startup class.
    /// <code>
    /// public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    /// {
    /// app.UseMiddleware/<LoggingMiddleware/>();
    /// }
    /// </code>
    /// </example>
    /// <code>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        /// <summary>
        /// Default contructor for configure middleware.
        /// </summary>
        /// <param name="next">A function that can process an HTTP request.</param>
        /// <param name="logger">Instance class of ILogger</param>
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Method that invokes the next middleware.
        /// </summary>
        /// <param name="httpContext">The HTTPContext for the request.</param>
        /// <returns>A Task that, on completion, indicates the middleware has executed.</returns>
        public async Task Invoke(HttpContext httpContext)
        {
            using (LogContext.PushProperty("X-CorrelationId", httpContext.TraceIdentifier))
            {
                try
                {
                    var request = httpContext.Request;
                    if (request.Path.StartsWithSegments(new PathString("/api")))
                    {
                        var stopWatch = Stopwatch.StartNew();
                        var requestBodyContent = await ReadRequestBody(request);
                        var originalBodyStream = httpContext.Response.Body;
                        using (var responseBody = new MemoryStream())
                        {
                            var response = httpContext.Response;
                            response.Body = responseBody;

                            SafeLog(
                                stopWatch.ElapsedMilliseconds,
                                response.StatusCode,
                                request.Method,
                                request.Path,
                                request.QueryString.ToString(),
                                requestBodyContent,
                                request.Headers[Headers.CorrelationId].ToString(),
                                null);

                            await _next(httpContext);
                            stopWatch.Stop();

                            string responseBodyContent = null;
                            responseBodyContent = await ReadResponseBody(response);
                            await responseBody.CopyToAsync(originalBodyStream);

                            SafeLog(
                                stopWatch.ElapsedMilliseconds,
                                response.StatusCode,
                                request.Method,
                                request.Path,
                                request.QueryString.ToString(),
                                requestBodyContent,
                                responseBodyContent,
                                request.Headers[Headers.CorrelationId].ToString());
                        }
                    }
                    else
                    {
                        await _next(httpContext);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return bodyAsText;
        }

        private void SafeLog(
            long responseMillis,
            int statusCode,
            string method,
            string path,
            string queryString,
            string requestBody,
            string responseBody,
            string correlationId,
            int maxChars = 10000)
        {
            if (path.ToLower().StartsWith("/api/login"))
            {
                requestBody = "(Request logging disabled for /api/login)";
                responseBody = "(Response logging disabled for /api/login)";
            }

            if (requestBody.Length > maxChars)
                requestBody = $"(Truncated to {maxChars} chars) {requestBody.Substring(0, maxChars)}";

            if (responseBody?.Length > maxChars)
                responseBody = $"(Truncated to {maxChars} chars) {responseBody.Substring(0, maxChars)}";

            if (queryString.Length > maxChars)
                queryString = $"(Truncated to {maxChars} chars) {queryString.Substring(0, maxChars)}";

            var requestlog = Log
                .ForContext("QueryString", queryString)
                .ForContext("RequestBody", requestBody)
                .ForContext("ReponseBody", responseBody);

            requestlog.Information("Request => Path: {0}, Method: {1}, Status: {2}, RequestTime: {3}, CorrelationId: {4}", path, method, statusCode, responseMillis, correlationId);
        }
    }
}
