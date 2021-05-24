using Casbin.Adapter.EFCore;
using EzDinner.Core.Aggregates.DinnerAggregate;
using EzDinner.Core.Aggregates.DishAggregate;
using EzDinner.Core.Aggregates.FamilyAggregate;
using EzDinner.Core.Aggregates.UserAggregate;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using System;

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


        public static IServiceCollection RegisterCasbin(this IServiceCollection services, IConfigurationSection section)
        {
            var connStrParts = section.GetValue<string>("ConnectionString").Split(';');
            var accountEndpoint = connStrParts[0].Substring(connStrParts[0].IndexOf('=') + 1);
            var accountKey = connStrParts[1].Substring(connStrParts[1].IndexOf('=') + 1);
            var options = new DbContextOptionsBuilder<CasbinDbContext<string>>()
              .UseCosmos(accountEndpoint, accountKey, databaseName: section.GetValue<string>("Database"))
              .Options;
            services.AddSingleton(_ => new CasbinDbContext<string>(options, new CasbinEntityConfiguration()));
            services.AddSingleton(s => new CasbinCosmosAdapter(s.GetRequiredService<CasbinDbContext<string>>()));
            return services;
        }

        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFamilyRepository, FamilyRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDishRepository, DishRepository>();
            services.AddScoped<IDinnerRepository, DinnerRepository>();
            return services;
        }

    }
}
