using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzDinner.Infrastructure
{
    public static class Setup
    {

        public static IServiceCollection RegisterMsGraph(this IServiceCollection services, IConfigurationSection adConfig)
        {
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
               .Create(adConfig.GetValue<string>("ClientId"))
               .WithTenantId(adConfig.GetValue<string>("TenantId"))
               .WithClientSecret(adConfig.GetValue<string>("ClientSecret"))
               .Build();

            ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);
            GraphServiceClient graphClient = new GraphServiceClient(authProvider);

            services.AddSingleton(graphClient);
            return services;
        }

    }
}
