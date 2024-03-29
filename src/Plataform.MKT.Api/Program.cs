using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;

namespace Plataform.MKT.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        private static readonly string _namespace = typeof(Program).Namespace;
        private static readonly string _appName = _namespace;
        private static IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            _configuration = GetConfiguration();

            var host = CreateHostBuilder(args).Build();

            Log.Information("Starting web host ({ApplicationContext})...", _appName);


            host.Run();

        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationContext", _appName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.EventCollector("http://localhost:514/services/collector", "ded4701a-b612-4e68-9849-de898742a4d4")
                .CreateLogger();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            Log.Information("Configuring web host ({ApplicationContext})...", _appName);

            _configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(_configuration);

            Serilog.Debugging.SelfLog.Enable(msg => System.Diagnostics.Debug.WriteLine(msg));

            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var settings = config.Build();


                    var keyVaultEndpoint = settings["VaultURI"];

                    if (!string.IsNullOrEmpty(keyVaultEndpoint))
                    {
                        var azureServiceTokenProvider = new AzureServiceTokenProvider();
                        var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                        config.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
                    }
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.CaptureStartupErrors(false);
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseConfiguration(_configuration);
                    webBuilder.UseSerilog();
                });
        }
    }
}
