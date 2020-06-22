using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartShareClient;
using SmartShareClient.Model;

namespace ExampleCoreProject
{
    public class Program
    {
        private static IConfiguration Configuration { get; set; }

        static async Task Main(string[] args)
        {
            var env = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");

            var isDevelopment = string.IsNullOrEmpty(env) ||
                                env.ToLower() == "development";

            var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true)
                    .AddJsonFile($"appsettings.{env}.json", optional: true)
                    .AddEnvironmentVariables();

            if (isDevelopment)
            {
                builder.AddUserSecrets<Program>();
            }

            Configuration = builder.Build();

            //await InitializeWithBuilderExtensions();
            //await InitializeWithConfiguration();
        }

        private static async Task InitializeWithBuilderExtensions()
        {
            var services = ConfigureServices(new ServiceCollection()).BuildServiceProvider();
            var client = services.GetRequiredService<ISmartShare>();

            var test = await client.GenerateTokenAsync();
        }

        private static async Task InitializeWithConfiguration()
        {
            var client = new SmartShare(Configuration);

            var test = await client.GenerateTokenAsync();

            var a = new ListaDocumentoRequest()
            {
                quantidade = 30,
                stApenasDocumentosAprovados = true,
                lstIndices = new System.Collections.Generic.List<Indice>()
                {
                    new Indice(1, "=", "")
                }
            };

            var documentos = await client.ListDocumentsAsync(a);
        }

        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddSmartShare(options => 
            {
                options.Endpoint = "";
                options.ClientId = "";
                options.ClientKey = "";
                options.User = "";
                options.Password = "";
            });

            return services;
        }
    }
}
