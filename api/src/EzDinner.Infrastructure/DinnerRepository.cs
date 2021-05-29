using EzDinner.Core.Aggregates.DinnerAggregate;
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
    public class DinnerRepository : IDinnerRepository
    {
        private readonly CosmosClient _client;
        private readonly Container _container;

        public DinnerRepository(CosmosClient client, IConfiguration configuration)
        {
            _client = client;
            _container = _client.GetContainer(configuration.GetValue<string>("CosmosDb:Database"), "Dinners");
        }

        public Task DeleteAsync(Dinner dinner)
        {
            return _container.DeleteItemAsync<Dinner>(dinner.Id.ToString(), new PartitionKey(dinner.PartitionKey.ToString()));
        }

        /// <summary>
        /// The serialization of DateTime is important when querying. Since CosmosDb stores datetimes as strings
        /// The format of the SQL query must match exactly the serialization format.
        /// 
        /// TODO: Store datetimes as UTC as that's what CosmosDb supports. https://docs.microsoft.com/en-us/azure/cosmos-db/working-with-dates
        /// 
        /// </summary>
        /// <param name="familyId"></param>
        /// <param name="exactDate"></param>
        /// <returns></returns>
        public async Task<Dinner?> GetAsync(Guid familyId, DateTime exactDate)
        {
            var sql = $"SELECT * FROM c WHERE c.familyId = '{familyId}' AND c.date = '{exactDate.Date:yyyy-MM-ddTHH:mm:sszzz}'";
            var queryDefinition = new QueryDefinition(sql);
            var queryResultSetIterator = _container.GetItemQueryIterator<Dinner>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                foreach (var dinner in await queryResultSetIterator.ReadNextAsync())
                {
                    return dinner;
                }
            }
            return null;
        }

        public async IAsyncEnumerable<Dinner> GetAsync(Guid familyId, DateTime fromDate, DateTime toDate)
        {
            var sql = $"SELECT * FROM c WHERE c.familyId = '{familyId}' AND c.date > '{fromDate:yyyy-MM-ddTHH:mm:sszzz}' and c.date < '{toDate:yyyy-MM-ddTHH:mm:sszzz}' ORDER BY c.date";
            var queryDefinition = new QueryDefinition(sql);
            var queryResultSetIterator = _container.GetItemQueryIterator<Dinner>(queryDefinition);

            while (queryResultSetIterator.HasMoreResults)
            {
                foreach (var dinner in await queryResultSetIterator.ReadNextAsync())
                {
                    yield return dinner;
                }
            }
        }

        public Task SaveAsync(Dinner dinner)
        {
            return _container.UpsertItemAsync(dinner);
        }
    }
}
