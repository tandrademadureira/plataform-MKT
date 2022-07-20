using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Shared.Api.HealthCheck.ViewModels;
using Shared.Util.Result;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using static Shared.Api.HealthCheck.HealthCheckOption;

namespace Shared.Api.HealthCheck
{
    /// <summary>
    /// Class will be used so that a controller inherits it, extending its existing methods for checking HealthCheck 
    /// </summary>
    /// <example>
    /// Using the root startup class.
    /// <code>
    /// public class CatalogController : HealthCheckBaseController
    /// {
    ///     your controller code
    /// }
    /// </code>
    /// </example>
    /// <code>
    public abstract class HealthCheckBaseController : ControllerBase
    {
        private readonly HealthCheckOption _option = new HealthCheckOption();

        /// <summary>
        /// Default contructor of class HealthCheckBaseController
        /// </summary>
        /// <param name="configuration">Instanse of IConfiguration</param>
        public HealthCheckBaseController(IConfiguration configuration)
        {
            configuration.GetSection("HealthChecks").Bind(_option);
        }

        /// <summary>
        /// Tests the communication between the micro-services internal to the corporation
        /// </summary>
        /// <param name="checkDependences">Boolean parameter that serves as a flag when calling checks</param>
        /// <returns></returns>
        [HttpGet]
        [Route("InternalIntegrations")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<HealthCheckViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckInternalIntegrations([FromQuery] bool checkDependences)
        {
            if (checkDependences)
            {
                List<HealthCheckViewModel> respose = new List<HealthCheckViewModel>();

                foreach (var item in _option.InternalIntegrations)
                {
                    respose.Add(await CheckInternalDependence(item));
                }

                return RestResult.CreateHttpResponse(respose);
            }

            return RestResult.CreateOkHttpResponse();
        }

        /// <summary>
        /// Tests the communication between the micro-services external to the corporation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ExternalIntegrations")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<HealthCheckViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CheckExternalIntegrations()
        {

            List<HealthCheckViewModel> respose = new List<HealthCheckViewModel>();

            foreach (var item in _option.ExternalIntegrations)
            {
                respose.Add(await CheckExternalDependence(item));
            }

            return RestResult.CreateHttpResponse(respose);
        }

        private async Task<HealthCheckViewModel> CheckInternalDependence(Integration integration)
        {
            try
            {
                if (integration.Url.Contains("checkDependences=true"))
                    return new HealthCheckViewModel { Name = integration.Name, Status = false };

                HttpClient httpClient = new HttpClient();
                var response = await httpClient.GetAsync(integration.Url);
                return new HealthCheckViewModel { Name = integration.Name, Status = response.IsSuccessStatusCode };
            }
            catch (System.Exception)
            {
                return new HealthCheckViewModel { Name = integration.Name, Status = false };
            }
        }

        private async Task<HealthCheckViewModel> CheckExternalDependence(Integration integration)
        {
            try
            {
                Ping ping = new Ping();

                PingReply result = await ping.SendPingAsync(integration.Url);
                return new HealthCheckViewModel { Name = integration.Name, Status = result.Status == IPStatus.Success };
            }
            catch (System.Exception)
            {
                return new HealthCheckViewModel { Name = integration.Name, Status = false };
            }
        }
    }
}
