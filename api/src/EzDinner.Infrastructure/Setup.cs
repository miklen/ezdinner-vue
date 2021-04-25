using EzDinner.Core.Aggregates.DinnerAggregate;
using EzDinner.Core.Aggregates.DishAggregate;
using EzDinner.Core.Aggregates.FamilyAggregate;
using EzDinner.Core.Aggregates.FamilyMemberAggregate;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
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
            var confidentialClientApplication = ConfidentialClientApplicationBuilder
               .Create(adConfig.GetValue<string>("ClientId"))
               .WithTenantId(adConfig.GetValue<string>("TenantId"))
               .WithClientSecret(adConfig.GetValue<string>("ClientSecret"))
               .Build();

            ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);
            GraphServiceClient graphClient = new GraphServiceClient(authProvider);

            services.AddSingleton(graphClient);
            return services;
        }

        public static IServiceCollection RegisterCosmosDb(this IServiceCollection services, IConfigurationSection section)
        {
            services.AddSingleton(_ => new CosmosClientBuilder(section.GetValue<string>("ConnectionString"))
                .WithSerializerOptions(new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase })
                .Build());
            return services;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFamilyRepository, FamilyRepository>();
            services.AddScoped<IFamilyMemberRepository, FamilyMemberRepository>();
            services.AddScoped<IDishRepository, DishRepository>();
            services.AddScoped<IDinnerRepository, DinnerRepository>();
            return services;
        }

    }
}
