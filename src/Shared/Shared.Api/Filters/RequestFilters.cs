﻿using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Shared.Infra.Cqrs;
using Shared.Infra.Request;
using Shared.Util.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Shared.Api.Filters
{
    /// <summary>
    /// Filter responsible for adding important information to the request header.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RequestFilters : ActionFilterAttribute
    {
        private IEnumerable<string> HeaderKeysToExtract { get; } = new List<string> { Headers.OriginIp, Headers.OriginDevice };

        /// <summary>
        /// Default constructor to configure base request message.
        /// </summary>
        /// <example>
        /// In the host layer into configuration folder, create a new file that's name ***MvcOptionsConfigurations***.
        /// <para>Create a new method as below.</para>
        /// <code>
        /// public static MvcOptions ConfigureFilters(this MvcOptions options, IConfiguration configuration)
        /// {
        ///     options.Filters.Add(new RequestMessageFilter());
        ///     return options;
        /// }
        /// </code>
        /// </example>
        public RequestFilters() { }

        /// <summary>
        /// Default constructor to configure base request message with include new header keys.
        /// </summary>
        /// <param name="headerKeysToExtract">New keys for header.</param>
        /// <example>
        /// In the host layer into configuration folder, create a new file that's name ***MvcOptionsConfigurations***.
        /// <para>Create a new method as below.</para>
        /// <code>
        /// public static MvcOptions ConfigureFilters(this MvcOptions options, IConfiguration configuration)
        /// {
        ///     var headerKeysToExtract = new List<![CDATA[<string>]]>() { Headers.SagaAction };
        ///     
        ///     options.Filters.Add(new RequestMessageFilter(headerKeysToExtract));
        ///     return options;
        /// }
        /// </code>
        /// </example>
        public RequestFilters(IEnumerable<string> headerKeysToExtract)
            : this()
        {
            var newHeaderKeysToExtract = new List<string> { Headers.OriginIp, Headers.OriginDevice };
            if (headerKeysToExtract != null && headerKeysToExtract.Any())
            {
                foreach (var headerKeyToExtract in headerKeysToExtract)
                    if (!HeaderKeysToExtract.Any(it => it == headerKeyToExtract))
                        newHeaderKeysToExtract.Add(headerKeyToExtract);
            }

            HeaderKeysToExtract = newHeaderKeysToExtract;
        }

        /// <summary>
        /// The call that happens before the method action is performed.
        /// </summary>
        /// <param name="context">Context to action.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var keys = context.ActionArguments.Keys.ToList();

            foreach (string key in keys)
            {
                object paramValue = context.ActionArguments[key];

                var paramDescriptor = context.ActionDescriptor.Parameters.Where(it => it.Name == key).FirstOrDefault();

                if (paramDescriptor != null && !paramDescriptor.ParameterType.IsSubclassOf(typeof(BaseRequest)))
                    continue;

                if (paramValue == null)
                {
                    paramValue = Activator.CreateInstance(paramDescriptor.ParameterType);
                    context.ActionArguments[key] = paramValue;
                }

                if (!(paramValue is BaseRequest baseRequest))
                    baseRequest = default(BaseRequest);

                if (baseRequest != null)
                {
                    SetCorrelationIdToRequest(context, baseRequest);
                    SetAutorizationTokenToRequest(context, baseRequest);
                    SetOriginIp(context);
                    SetOriginDevice(context);
                    SetCustomHeaders(context, baseRequest);
                }
            }
        }

        private void SetCorrelationIdToRequest(ActionExecutingContext context, BaseRequest baseRequest)
        {
            var correlationId = context.HttpContext.Request.Headers[Headers.CorrelationId].ToString();

            if (string.IsNullOrWhiteSpace(correlationId) || !Guid.TryParse(correlationId, out Guid correlationIdParsed))
            {
                correlationId = Guid.NewGuid().ToString("N");
                context.HttpContext.Request.Headers.Add(Headers.CorrelationId, correlationId);
                context.HttpContext.Request.Headers[Headers.CorrelationId] = correlationId;
            }

            baseRequest.AddHeader(Headers.CorrelationId, correlationId);
        }
        private void SetAutorizationTokenToRequest(ActionExecutingContext context, BaseRequest baseRequest)
        {
            var authorizationToken = context.HttpContext.Request.Headers[Headers.Authorization].ToString();
            context.HttpContext.Request.Headers.Add(Headers.Authorization, authorizationToken);
            context.HttpContext.Request.Headers[Headers.Authorization] = authorizationToken;
            baseRequest.AddHeader(Headers.Authorization, authorizationToken);
        }

        private void SetOriginIp(ActionExecutingContext context)
        {
            if (!string.IsNullOrWhiteSpace(context.HttpContext.Request.Headers[Headers.OriginIp]))
                return;

            var clientIp = string.Empty;

            if (context.HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out StringValues header))
                if (header.Count > 0)
                    clientIp = header[0];

            if (string.IsNullOrWhiteSpace(clientIp))
                clientIp = context.HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();

            context.HttpContext.Request.Headers.Add(Headers.OriginIp, clientIp);

        }

        private void SetOriginDevice(ActionExecutingContext context)
        {
            if (!string.IsNullOrWhiteSpace(context.HttpContext.Request.Headers[Headers.OriginDevice]))
                return;

            if (context.HttpContext.Request.Headers.TryGetValue("User-Agent", out StringValues header))
            {
                var u = string.Empty;

                if (header.Count > 0)
                    u = header[0];

                if (!string.IsNullOrWhiteSpace(u))
                {
                    var b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    var v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    var us = u.Substring(0, 4);

                    if (b.IsMatch(u) || (string.IsNullOrEmpty(us) && v.IsMatch(us)))
                        context.HttpContext.Request.Headers.Add(Headers.OriginDevice, "Mobile");
                    else
                        context.HttpContext.Request.Headers.Add(Headers.OriginDevice, "Desktop");
                }
                else
                {
                    context.HttpContext.Request.Headers.Add(Headers.OriginDevice, "Desktop");
                }
            }
            else
            {
                context.HttpContext.Request.Headers.Add(Headers.OriginDevice, "Desktop");
            }
        }

        private void SetCustomHeaders(ActionExecutingContext context, BaseRequest baseRequest)
        {
            if (baseRequest == null)
                return;

            foreach (string headerKeyExtract in HeaderKeysToExtract)
            {
                var value = context.HttpContext.Request.Headers[headerKeyExtract].ToString();

                if (!string.IsNullOrWhiteSpace(value))
                    baseRequest.AddHeader(headerKeyExtract, value);
            }
        }
    }
}
