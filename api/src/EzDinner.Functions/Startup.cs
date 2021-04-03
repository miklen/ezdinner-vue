using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using EzDinner.Core.Aggregates.FamilyMemberAggregate;
using EzDinner.Infrastructure;

[assembly: FunctionsStartup(typeof(EzDinner.Functions.Startup))]

namespace EzDinner.Functions
{
    public class Startup : FunctionsStartup
    {
        public Startup()
        {
        }

        IConfiguration Configuration { get; set; }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Get the azure function application directory. 'C:\whatever' for local and 'd:\home\whatever' for Azure
            var executionContextOptions = builder.Services.BuildServiceProvider()
                .GetService<IOptions<ExecutionContextOptions>>().Value;

            var currentDirectory = executionContextOptions.AppDirectory;

            // Get the original configuration provider from the Azure Function
            var configuration = builder.Services.BuildServiceProvider().GetService<IConfiguration>();

            // Create a new IConfigurationRoot and add our configuration along with Azure's original configuration 
            Configuration = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddConfiguration(configuration) // Add the original function configuration 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var adConfig = Configuration.GetSection("AzureAdB2C");

            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(adConfig.GetValue<string>("ClientId"))
                .WithTenantId(adConfig.GetValue<string>("TenantId"))
                .WithClientSecret(adConfig.GetValue<string>("ClientSecret"))
                .Build();

            ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);
            GraphServiceClient graphClient = new GraphServiceClient(authProvider);


            // Replace the Azure Function configuration with our new one
            builder.Services.AddSingleton(Configuration);
            builder.Services.AddSingleton(graphClient);

            builder.Services.AddScoped<IFamilyMemberRepository, FamilyMemberRepository>();
            
            ConfigureServices(builder.Services);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(sharedOptions =>
           {
               sharedOptions.DefaultScheme = Microsoft.Identity.Web.Constants.Bearer;
               sharedOptions.DefaultChallengeScheme = Microsoft.Identity.Web.Constants.Bearer;
           })
               .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2C"));
        }
    }
}
