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
using EzDinner.Core.Aggregates.UserAggregate;
using EzDinner.Infrastructure;
using EzDinner.Core.Aggregates.DinnerAggregate;

[assembly: FunctionsStartup(typeof(EzDinner.Functions.Startup))]

namespace EzDinner.Functions
{
    public class Startup : FunctionsStartup
    {
        public Startup()
        {
        }

        IConfiguration? Configuration { get; set; }

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
                .AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true) // add local development variables if present - file is not committed to git so won't be present for prod
                .Build();

            builder.Services.AddLogging();
            builder.Services.AddAutoMapper(typeof(Startup));

            // Replace the Azure Function configuration with our new one
            builder.Services.AddSingleton(Configuration);
            builder.Services.RegisterMsGraph(Configuration.GetSection("AzureAdB2C"));
            builder.Services.RegisterCosmosDb(Configuration.GetSection("CosmosDb"));
            builder.Services.RegisterRepositories();
            builder.Services.AddScoped<IDinnerService, DinnerService>();

            ConfigureServices(builder.Services);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(sharedOptions =>
           {
               sharedOptions.DefaultScheme = Microsoft.Identity.Web.Constants.Bearer;
               sharedOptions.DefaultChallengeScheme = Microsoft.Identity.Web.Constants.Bearer;
           })
               .AddMicrosoftIdentityWebApi(Configuration!.GetSection("AzureAdB2C"));
        }
    }
}
