using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using EzDinner.Infrastructure;
using EzDinner.Core.Aggregates.DishAggregate;
using EzDinner.Core.Aggregates.DinnerAggregate;
using EzDinner.Core.Aggregates.FamilyAggregate;
using System.Collections.Generic;
using EzDinner.Application.Commands;
using EzDinner.Authorization.Core;
using Casbin.Adapter.EFCore;

namespace EzDinner.Functions
{
    public class Migrate
    {
        private readonly ILogger<Migrate> _logger;
        private readonly IAuthzService _authz;
        private readonly CosmosClient _cosmosClient;
        private readonly CasbinDbContext<string> _casbinContext;
        private readonly IConfiguration _configuration;

        public Migrate(ILogger<Migrate> logger, IAuthzService authz, CosmosClient cosmosClient, CasbinDbContext<string> casbinContext, IConfiguration configuration)
        {
            _logger = logger;
            _authz = authz;
            _cosmosClient = cosmosClient;
            _casbinContext = casbinContext;
            _configuration = configuration;
        }
        
        [FunctionName(nameof(Migrate))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "migrate")] HttpRequest req)
        {
            _logger.LogInformation("Migration started");
            var database = await EnsureDatabaseAndCollectionsCreated(_configuration);
            await _casbinContext.Database.EnsureCreatedAsync();
            await UpdateFamiliesPermissions(database);

            return new OkResult();
        }

        private async Task UpdateFamiliesPermissions(Database database)
        {
            var families = await GetFamilies(database);
            var updatePermissionsCommand = new UpdatePermissionsCommand(_authz);
            foreach (var family in families)
            {
                await updatePermissionsCommand.Handle(family);
            }
        }

        private static async Task<IEnumerable<Family>> GetFamilies(Database database)
        {
            var familiesContainer = database.GetContainer(FamilyRepository.CONTAINER);
            var query = new QueryDefinition("SELECT * FROM c");
            var familiesQuery = familiesContainer.GetItemQueryIterator<Family>(query);
            var families = new List<Family>();
            while (familiesQuery.HasMoreResults)
            {
                foreach (var family in await familiesQuery.ReadNextAsync())
                {
                    families.Add(family);
                }
            }
            return families;
        }

        private async Task<Database> EnsureDatabaseAndCollectionsCreated(IConfiguration configuration)
        {
            var dbName = configuration.GetValue<string>("CosmosDb:Database");
            var dbResponse = await _cosmosClient.CreateDatabaseIfNotExistsAsync(dbName);
            var db = dbResponse.Database;
            await db.CreateContainerIfNotExistsAsync(new ContainerProperties(DishRepository.CONTAINER, $"/{nameof(Dish.PartitionKey).ToCamelCase()}"));
            await db.CreateContainerIfNotExistsAsync(new ContainerProperties(DinnerRepository.CONTAINER, $"/{nameof(Dinner.PartitionKey).ToCamelCase()}"));
            await db.CreateContainerIfNotExistsAsync(new ContainerProperties(FamilyRepository.CONTAINER, $"/{nameof(Family.PartitionKey).ToCamelCase()}"));
            return db;
        }
    }

    public static class StringExtension
    {
        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }
    }
}
