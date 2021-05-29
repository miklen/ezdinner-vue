using EzDinner.Core.Aggregates.FamilyAggregate;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzDinner.Infrastructure
{
    public class FamilyRepository : IFamilyRepository
    {
        private readonly CosmosClient _client;
        private readonly Container _container;
        public const string CONTAINER = "Families";

        public FamilyRepository(CosmosClient client, IConfiguration configuration)
        {
            _client = client;
            _container = _client.GetContainer(configuration.GetValue<string>("CosmosDb:Database"), CONTAINER);
        }

        public Task SaveAsync(Family family)
        {
            return _container.UpsertItemAsync(family);
        }

        public async Task<IEnumerable<Family>> GetFamilySelectorsAsync(Guid userId)
        {
            var sql = $"SELECT * FROM c WHERE c.ownerId = '{userId}' OR ARRAY_CONTAINS(c.familyMemberIds, '{userId}')";
            var queryDefinition = new QueryDefinition(sql);
            var queryResultSetIterator = _container.GetItemQueryIterator<Family>(queryDefinition);

            var families = new List<Family>();
            while (queryResultSetIterator.HasMoreResults)
            {
                foreach (var family in await queryResultSetIterator.ReadNextAsync())
                {
                    families.Add(family);
                }
            }
            return families;
        }

        public async Task<Family?> GetFamily(Guid familyId)
        {
            var sql = $"SELECT * FROM c WHERE c.id = '{familyId}' OR ARRAY_CONTAINS(c.familyMemberIds, '{familyId}')";
            var queryDefinition = new QueryDefinition(sql);
            var queryResultSetIterator = _container.GetItemQueryIterator<Family>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                foreach (var family in await queryResultSetIterator.ReadNextAsync())
                {
                    return family;
                }
            }
            return null;
        }
    }
}
